using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrashLoyal.Arena;

namespace CrashLoyal.Cards
{
    public class MeleeUnit : Unit, IUnit
    {
        public void Hit()
        {
            if (lockedTarget == null || lockedTarget.Equals(null))
            {
                enemiesInRange.Remove(lockedTarget);
                UnlockToTarget();
                return;
            }

            if (lockedTarget.TakeDamage(unitStatistic.damage))
            {
                enemiesInRange.Remove(lockedTarget);
                UnlockToTarget();
            }
        }
    }

}