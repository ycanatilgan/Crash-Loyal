using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrashLoyal.Arena
{
    public class DamageDealer : MonoBehaviour
    {
        Transform target, _transform;
        [SerializeField] float speed = 15f;
        bool areaImpact;
        int damage;
        Tower firedFrom;

        private void OnEnable()
        {
            _transform = transform;
        }

        public void SetParameters(Transform target, bool areaImpact, int damage, Tower firedFrom)
        {
            this.target = target;
            this.areaImpact = areaImpact;
            this.damage = damage;
            this.firedFrom = firedFrom;
        }

        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            _transform.position = Vector3.MoveTowards(_transform.position, target.position, speed * Time.deltaTime);
            _transform.LookAt(target);

            if (Vector3.Distance(_transform.position, target.position) < .02f)
            {
                if (target.GetComponent<IDamageable>().TakeDamage(damage))
                {
                    firedFrom.UnitDied();
                }

                Destroy(gameObject);
            }
        }
    }
}