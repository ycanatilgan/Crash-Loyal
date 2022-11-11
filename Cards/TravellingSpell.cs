using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrashLoyal.Arena;

namespace CrashLoyal.Cards
{
    public class TravellingSpell : Spell
    {
        [SerializeField] GameObject finalExplosion;

        public override void StartWithParameters(Vector3 startPos, Vector3 targetPos)
        {
            transform.position = startPos;

            StartCoroutine(TravelToTarget(targetPos));
        }

        IEnumerator TravelToTarget(Vector3 pos)
        {
            Transform _transform = transform;

            while (true)
            {
                yield return null;

                _transform.position = Vector3.MoveTowards(_transform.position, pos, spellData.speed * Time.deltaTime);

                if (Vector3.Distance(_transform.position, pos) < .02f)
                    break;
            }

            finalExplosion.SetActive(true);
            Destroy(gameObject, 4f);

            GetComponent<SphereCollider>().radius = spellData.area;
            GetComponent<Collider>().enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;

            if (other.tag == "Tower" || other.tag == "Unit")
                if (other.GetComponent<IDamageable>().Friendly != friendly)
                    other.GetComponent<IDamageable>().TakeDamage(spellData.damage);
        }
    }
}