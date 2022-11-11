using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrashLoyal.Arena
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Transform healthBar;

        [SerializeField] GameObject child;

        public void UpdateHealth(int newHealth, int maxHealth)
        {
            if (!child.activeSelf)
                child.SetActive(true);

            var result = Mathf.Lerp(-1.85f, 0f, Mathf.InverseLerp(0, maxHealth, newHealth));

            Vector3 pos = healthBar.localPosition;
            pos.x = result;
            healthBar.localPosition = pos;
        }
    }
}