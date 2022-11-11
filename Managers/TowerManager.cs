using System;
using System.Collections.Generic;
using UnityEngine;
using CrashLoyal.Events;

namespace CrashLoyal.Manager
{
    public class TowerManager : Manager
    {
        [SerializeField] List<Transform> friendlyTowers, enemyTowers;

        public List<Transform> FriendlyTowers
        {
            get
            {
                return friendlyTowers;
            }
        }

        private void Awake()
        {
            EventManager.PrincessTowerDestroyed += PrincessTowerDestroyed;
        }

        public Vector3 GetTargetBuilding(Vector3 unit, bool friendly)
        {
            List<Transform> selectedTowers = friendly ? enemyTowers : friendlyTowers;
                
            Vector3 towerPos = Vector3.zero;
            float shortest = 1000;

            for (int i = 0; i < 2 && i < selectedTowers.Count; i++)
            {
                float distance = Mathf.Abs(selectedTowers[i].position.x - unit.x);

                if (distance < shortest)
                {
                    towerPos = selectedTowers[i].position;
                    shortest = distance;
                }
            }

            return towerPos;
        }

        public void PrincessTowerDestroyed(Transform tower)
        {
            if (enemyTowers.Contains(tower))
                enemyTowers.Remove(tower);
            else if (friendlyTowers.Contains(tower))
                friendlyTowers.Remove(tower);
        }

    }
}
