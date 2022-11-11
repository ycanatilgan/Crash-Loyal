using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrashLoyal.Arena
{
    public interface IDamageable
    {
        Transform transform { get; }
        public bool TakeDamage(int damage);
        public bool Friendly { get; }
    }
}