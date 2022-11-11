using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CrashLoyal.Arena;
using CrashLoyal.UI;
using CrashLoyal.Events;
using CrashLoyal.Cards;

namespace CrashLoyal.Manager
{
    public class FlowManager : Manager
    {
        [SerializeField] int gameTimeInSec;
        [SerializeField] float elixirSpeed;

        [SerializeField] CardStatistics[] deck;
        [SerializeField] UIAdapter uiAdapter;

        int currentElixir;

        Vector3 spellSpawnPos;

        private void Awake()
        {
            ShuffleDeck();

            uiAdapter.StartWithParameters(gameTimeInSec, elixirSpeed, deck);

            EventManager.PlaceUnit += PlaceCard;
            EventManager.KingTowerDestroyed += Finished;

            currentElixir = 5;
            StartCoroutine(FillElixir());

            spellSpawnPos = GetComponent<TowerManager>().FriendlyTowers[2].transform.position;
            spellSpawnPos.y += 4f;
        }

        IEnumerator FillElixir()
        {
            uiAdapter.UpdateElixir(currentElixir);

            while (true)
            {
                yield return new WaitForSeconds(elixirSpeed);

                if (currentElixir >= 10)
                    continue;

                uiAdapter.UpdateElixir(++currentElixir);
            }
        }

        void ShuffleDeck()
        {
            System.Random random = new System.Random();
            deck = deck.OrderBy(x => random.Next()).ToArray();
        }

        void PlaceCard(Vector3 pos, CardStatistics card, Slot slot)
        {
            if (card.elixir > currentElixir)
                return;

            currentElixir -= card.elixir;
            uiAdapter.UpdateElixir(currentElixir);

            GameObject newCard = Instantiate(card.cardPrefab);

            if (card.isSpell)
                newCard.GetComponent<Spell>().StartWithParameters(spellSpawnPos, pos);
            else
                newCard.transform.position = pos;

            EventManager.RecreateDeck(card, slot);
        }

        void Finished(bool _)
        {
            StopAllCoroutines();
        }
    }
}