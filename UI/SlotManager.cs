using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using CrashLoyal.Events;
using CrashLoyal.Arena;
using System;

namespace CrashLoyal.UI
{
    public class SlotManager : MonoBehaviour
    {
        [SerializeField] List<Slot> slots;

        CardStatistics[] deck;

        public void StartWithParameters(CardStatistics[] deck)
        {
            this.deck = deck;
            CreateDeckView();

            EventManager.RecreateDeck += CardPlaced;
        }

        void CreateDeckView()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].SetForNewCard(deck[i]);
            }
        }

        private void CardPlaced(CardStatistics card, Slot slot)
        {
            int slotIndex = slots.IndexOf(slot);
            slots[slotIndex].SetForNewCard(deck[4]);
            slots[4].SetForNewCard(deck[5]);

            CardStatistics cardToReplace = deck[slotIndex];
            deck[slotIndex] = deck[4];
            for (int i = 5; i < deck.Length; i++)
                deck[i - 1] = deck[i];

            deck[deck.Length - 1] = cardToReplace;
        }
    }
}