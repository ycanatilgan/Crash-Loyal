using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrashLoyal.Arena
{
    [CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Create New Spell Data", order = 1)]
    public class SpellData : ScriptableObject
    {
        public int damage;
        public float speed;
        public float area;
        public bool canHitAir;
    }
}
