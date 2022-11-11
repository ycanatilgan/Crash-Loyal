using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrashLoyal.Arena
{
    [CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Create New Tower Data", order = 1)]
    public class TowerStatistics : ScriptableObject
    {
        public int hitPoint;
        public int damage;
        public float range;
        public GameObject arrow;
        public bool isKingTower;
    }
}