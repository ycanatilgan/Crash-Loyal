using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CrashLoyal.Events;
using CrashLoyal.Arena;
using UnityEngine.UI;

namespace CrashLoyal.UI
{
    public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] Image itemImage;
        [SerializeField] Text elixirCost;

        SlotManager slotManager;
        Canvas myCanvas;
        RectTransform transform;
        Camera cam;

        Vector2 originalPos, featuredPos;

        CardStatistics currentCard;

        private void Start()
        {
            slotManager = GetComponentInParent<SlotManager>();
            transform = GetComponent<RectTransform>();
            myCanvas = GetComponentInParent<Canvas>();
            cam = Camera.main;

            originalPos = transform.anchoredPosition;
            featuredPos = originalPos;
            featuredPos.y += 50;

            EventManager.CardPicked += CardPicked;
            EventManager.KingTowerDestroyed += Finished;
        }

        public void SetForNewCard(CardStatistics card)
        {
            itemImage.sprite = card.cardImage;
            elixirCost.text = card.elixir.ToString();

            currentCard = card;
        }

        void CardPicked(Slot slot)
        {
            if (slot.Equals(this))
                return;

            StopAllCoroutines();
            StartCoroutine(StandIn());
        }

        IEnumerator StandOut()
        {
            while (true)
            {
                transform.anchoredPosition = Vector3.MoveTowards(transform.anchoredPosition, featuredPos, 3000 * Time.deltaTime);

                if (Vector3.Distance(transform.anchoredPosition, featuredPos) < .02f)
                    break;

                yield return null;
            }
        }

        IEnumerator StandIn()
        {
            Vector2 targetPos = transform.anchoredPosition;
            targetPos.y = 25;

            while (true)
            {
                transform.anchoredPosition = Vector3.MoveTowards(transform.anchoredPosition, originalPos, 3000 * Time.deltaTime);

                if (Vector3.Distance(transform.anchoredPosition, originalPos) < .01f)
                    break;

                yield return null;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(StandOut());

            EventManager.CardPicked(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            int layerMask = currentCard.isSpell ? 1 << 10 : 1 << 7;
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                EventManager.PlaceUnit(hit.point, currentCard, this);
                StartCoroutine(StandIn());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(StandOut());
            }

        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            transform.position = myCanvas.transform.TransformPoint(pos);
        }

        void Finished(bool _)
        {
            this.enabled = false;
        }
    }
}