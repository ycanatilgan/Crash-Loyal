using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrashLoyal.Cards;
using CrashLoyal.Arena;
using CrashLoyal.Events;
using CrashLoyal.Manager;
using UnityEngine.AI;
using System.Drawing;

public class AirMeleeUnit : MeleeUnit
{
    float speed;

    bool isStopped;

    Vector3 targetPos;

    private void Start()
    {
        base.Start();

        speed = unitStatistic.walkSpeed;
        StartCoroutine(MoveToPath());
    }

    protected override IEnumerator ChechForEnemies()
    {
        SetTarget();

        while (true)
        {
            yield return new WaitForSeconds(randomUpdateTime);

            if (isStopped || enemiesInRange.Count == 0)
                continue;

            SetTarget();
        }
    }

    protected override void SetTarget()
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
            targetPos = targetUnit.transform.position;
            isStopped = false;
        }
        else
        {
            StartCoroutine(StartAttacking(targetUnit));
        }
    }

    protected override IEnumerator StartAttacking(IDamageable targetUnit)
    {
        lockedTarget = targetUnit;

        animator.SetBool("Attacking", true);
        isStopped = true;

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

    public override void CalculatePathToTower()
    {
        isStopped = false;
        Vector3 targetPos = arenaManager.GetTargetBuilding(transform.position, friendly);
        this.targetPos = targetPos;
    }

    protected override void GameFinished(bool friendly)
    {
        this.enabled = false;
        StopAllCoroutines();
        animator.speed = 0f;
        isStopped = true;
    }

    IEnumerator MoveToPath()
    {
        while (true)
        {
            yield return null;

            if (targetPos == Vector3.zero || isStopped)
                continue;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            Vector3 relativePos = targetPos - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 5 * Time.deltaTime);
        }
    }
}