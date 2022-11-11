using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrashLoyal.Arena;

namespace CrashLoyal.Cards
{
    public class Spell : MonoBehaviour
    {
        [SerializeField] protected SpellData spellData;

        [SerializeField] public bool friendly;

        public virtual void StartWithParameters(Vector3 startPos, Vector3 targetPos)
        {

        }

        public void StartWithParameters(Vector3 startPos)
        {

        }
    }
}