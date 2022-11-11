using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using CrashLoyal.Manager;
using CrashLoyal.Arena;
using CrashLoyal.Events;

namespace CrashLoyal.Cards
{
    public class Unit : MonoBehaviour, IDamageable
    {
        [SerializeField] public UnitData unitStatistic;

        [SerializeField] protected bool friendly;

        NavMeshAgent agent;
        protected Animator animator;
        HealthBar healthBar;

        protected int hitPoints;

        protected float randomUpdateTime;

        protected IDamageable lockedTarget;

        public bool Friendly
        {
            get
            {
                return friendly;
            }
        }

        protected TowerManager arenaManager;

        public List<IDamageable> enemiesInRange = new List<IDamageable>();

        protected void Start()
        {
            EventManager.KingTowerDestroyed += GameFinished;
            EventManager.PrincessTowerDestroyed += RecalculatePath;

            arenaManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TowerManager>();

            animator = GetComponentInChildren<Animator>();
            healthBar = GetComponentInChildren<HealthBar>();

            if (!unitStatistic.isAirUnit)
            {
                agent = GetComponent<NavMeshAgent>();
                agent.speed = unitStatistic.walkSpeed;
            }

            hitPoints = unitStatistic.hitPoint;
            randomUpdateTime = Random.Range(.1f, .2f);

            StartCoroutine(ChechForEnemies());
        }

        protected virtual IEnumerator ChechForEnemies()
        {
            SetTarget();

            while (true)
            {
                yield return new WaitForSeconds(randomUpdateTime);

                if (agent.isStopped || enemiesInRange.Count == 0)
                    continue;

                SetTarget();
            }
        }

        protected virtual void SetTarget()
        {
            if (enemiesInRange.Count == 0)
            {
                CalculatePathToTower();
                return;
            }

            IDamageable targetUnit = null;
            float shortest = 100;
            foreach (IDamageable unit in enemiesInRange)
            {
                if (unit.Equals(null))
                {
                    enemiesInRange.Remove(unit);
                    SetTarget();
                    return;
                }

                float distance = Vector3.Distance(transform.position, unit.transform.position);
                if (distance < shortest)
                {
                    shortest = distance;
                    targetUnit = unit;
                }
            }

            if (shortest > unitStatistic.range)
            {
                agent.SetDestination(targetUnit.transform.position);
                agent.isStopped = false;
            }
            else
            {
                StartCoroutine(StartAttacking(targetUnit));
            }
        }

        public virtual void CalculatePathToTower()
        {
            agent.isStopped = false;
            Vector3 targetPos = arenaManager.GetTargetBuilding(transform.position, friendly);
            agent.SetDestination(targetPos);
        }

        void AddToRangeList(IDamageable newUnit)
        {
            if (!unitStatistic.canHitAir)
                if (newUnit.transform.tag == "Unit")
                    if (newUnit.transform.GetComponent<Unit>().unitStatistic.isAirUnit)
                        return;
                

            enemiesInRange.Add(newUnit);
        }

        protected virtual IEnumerator StartAttacking(IDamageable targetUnit)
        {
            lockedTarget = targetUnit;

            animator.SetBool("Attacking", true);
            agent.isStopped = true;

            while (true)
            {
                yield return new WaitForSeconds(.05f);

                if (lockedTarget == null || lockedTarget.Equals(null) || Vector3.Distance(transform.position, lockedTarget.transform.position) > unitStatistic.range)
                {
                    UnlockToTarget();
                    break;
                }

                Vector3 flat = lockedTarget.transform.position;
                flat.y = transform.position.y;
                transform.LookAt(flat);
            }
        }

        protected void UnlockToTarget()
        {
            animator.SetBool("Attacking", false);
            lockedTarget = null;
            SetTarget();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;

            if (other.tag.Equals("Unit") || other.tag.Equals("Tower"))
                if (other.GetComponent<IDamageable>().Friendly != Friendly)
                    AddToRangeList(other.GetComponent<IDamageable>());
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger)
                return;

            if (other.tag.Equals("Unit") || other.tag.Equals("Tower"))
                if (enemiesInRange.Contains(other.GetComponent<IDamageable>()))
                    enemiesInRange.Remove(other.GetComponent<IDamageable>());
        }

        public bool TakeDamage(int damage)
        {
            hitPoints -= damage;

            healthBar.UpdateHealth(hitPoints, unitStatistic.hitPoint);

            if (hitPoints <= 0)
            {
                Destroy(gameObject);
                StopAllCoroutines();
                return true;
            }
            else
                return false;
        }

        protected virtual void GameFinished(bool friendly)
        {
            this.enabled = false;
            StopAllCoroutines();
            animator.speed = 0f;
            agent.isStopped = true;
        }

        void RecalculatePath(Transform _)
        {
            SetTarget();
        }

        private void OnDestroy()
        {
            EventManager.KingTowerDestroyed -= GameFinished;
            EventManager.PrincessTowerDestroyed -= RecalculatePath;
        }
    }
}
