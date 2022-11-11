using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrashLoyal.Arena
{
    [CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Create New Unit Data", order = 1)]
    public class UnitData : ScriptableObject
    {
        public int hitPoint;
        public int damage;
        public float walkSpeed;
        public float range;
        public bool isWinCondition;
        public bool isAirUnit;
        public bool canHitAir;
    }
}
