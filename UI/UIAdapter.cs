using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CrashLoyal.Arena;
using CrashLoyal.Events;

namespace CrashLoyal.UI
{
    public class UIAdapter : MonoBehaviour
    {
        [SerializeField] Text countDown, elixirCounter;
        [SerializeField] Transform elixirBar;

        //CardStatistics[] deck;

        private void Start()
        {
            EventManager.KingTowerDestroyed += Finished;
        }

        public void StartWithParameters(int gameTimeInSec, float elixirSpeed, CardStatistics[] deck)
        {
            GetComponentInChildren<SlotManager>().StartWithParameters(deck);

            StartCoroutine(Timer(gameTimeInSec));
        }

        IEnumerator Timer(int gameTimeInSec)
        {
            int remainingSec = gameTimeInSec;

            while (remainingSec > 0)
            {
                countDown.text = SetCountDownText(remainingSec--);

                yield return new WaitForSecondsRealtime(1);
            }
        }

        string SetCountDownText(int remaining)
        {
            return remaining / 60 + ":" + (remaining % 60 == 0 ? "00" : (remaining % 60).ToString());
        }

        public void UpdateElixir(int elixir)
        {
            elixirCounter.text = elixir.ToString();

            Vector2 pos = elixirBar.localPosition;
            pos.x = (elixir - 10) * 60;
            elixirBar.localPosition = pos;
        }

        void Finished(bool _)
        {
            StopAllCoroutines();
        }
    }
}