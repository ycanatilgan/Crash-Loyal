using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrashLoyal.Events;

namespace CrashLoyal.Manager
{
    public class Manager : MonoBehaviour
    {
        private void Start()
        {
            EventManager.KingTowerDestroyed += Finish;
        }

        protected virtual void Finish(bool friendly) 
        {
            return;
        }
    }
}