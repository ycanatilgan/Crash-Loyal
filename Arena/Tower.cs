using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrashLoyal.Cards;
using CrashLoyal.Events;

namespace CrashLoyal.Arena
{
    public class Tower : MonoBehaviour, IDamageable
    {
        Animator princessAnimator;
        HealthBar healthBar;

        Vector3 arrowSpawnPoint;

        [SerializeField] private bool friendly;

        [SerializeField] TowerStatistics towerData;

        public int hitPoints;
        public List<IDamageable> enemiesInRange = new List<IDamageable>();

        IDamageable currentTarget;
        bool isAttacking, isActive;

        public bool Friendly
        {
            get
            {
                return friendly;
            }
        }

        private void Start()
        {
            princessAnimator = GetComponentInChildren<Animator>();
            healthBar = GetComponentInChildren<HealthBar>();

            arrowSpawnPoint = princessAnimator.transform.position;
            arrowSpawnPoint.y += 3f;

            hitPoints = towerData.hitPoint;
            GetComponent<CapsuleCollider>().radius = towerData.range;

            EventManager.KingTowerDestroyed += GameFinished;

            if (towerData.isKingTower)
                EventManager.PrincessTowerDestroyed += ActivateTower;
            else
                isActive = true;
        }

        void ActivateTower(Transform t)
        {
            if (t.GetComponent<Tower>().Friendly != Friendly)
                return;

            isActive = true;
            SetNewTarget();
        }

        void AddToRangeList(IDamageable newUnit)
        {
            enemiesInRange.Add(newUnit);

            if (!isAttacking && isActive)
                SetNewTarget();
        }

        void SetNewTarget()
        {
            if(enemiesInRange.Count == 0)
            {
                princessAnimator.SetBool("Attack", false);
                isAttacking = false;
                return;
            }

            isAttacking = true;
            princessAnimator.SetBool("Attack", true);

            float shortest = 100;
            foreach (IDamageable unit in enemiesInRange) 
            {
                if(unit.Equals(null))
                {
                    enemiesInRange.Remove(unit);
                    SetNewTarget();
                    break;
                }    

                float distance = Vector3.Distance(transform.position, unit.transform.position);
                if (distance < shortest)
                {
                    shortest = distance;
                    currentTarget = unit;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) 
                return;

            if (other.tag.Equals("Unit"))
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

        public void Hit()
        {
            if (currentTarget == null || currentTarget.Equals(null))
            {
                UnlockToTarget();
                return;
            }

            GameObject go = Instantiate(towerData.arrow);
            go.transform.position = arrowSpawnPoint;
            go.GetComponent<DamageDealer>().SetParameters(currentTarget.transform, false, towerData.damage, this);
        }

        public void UnitDied()
        {
            UnlockToTarget();
        }

        void UnlockToTarget()
        {
            enemiesInRange.Remove(currentTarget);
            currentTarget = null;
            SetNewTarget();
        }

        public bool TakeDamage(int damage)
        {
            if(!isActive)
                isActive = true;

            hitPoints -= damage;

            healthBar.UpdateHealth(hitPoints, towerData.hitPoint);

            if (hitPoints <= 0)
            {
                DestroyTower();
                return true;
            }
            else
                return false;
        }

        void DestroyTower()
        {
            if (!towerData.isKingTower)
                EventManager.PrincessTowerDestroyed(transform);
            else
                EventManager.KingTowerDestroyed(Friendly);

            Destroy(gameObject);
        }

        void GameFinished(bool friendly)
        {
            if (friendly == Friendly)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            EventManager.KingTowerDestroyed -= GameFinished;
            EventManager.PrincessTowerDestroyed -= ActivateTower;
        }
    }
}