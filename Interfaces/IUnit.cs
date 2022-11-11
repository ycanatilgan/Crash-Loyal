using UnityEngine;
using CrashLoyal.Arena;

namespace CrashLoyal.Cards
{
    public interface IUnit
    {
        void Hit();
        void CalculatePathToTower();
        void OnTriggerEnter(Collider other);
    }
}