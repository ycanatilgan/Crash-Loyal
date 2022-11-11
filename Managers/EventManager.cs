using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrashLoyal.UI;

namespace CrashLoyal.Events
{
    public class EventManager
    {
        public static Action<Transform> PrincessTowerDestroyed;
        public static Action<bool> KingTowerDestroyed;

        //UI Events
        public static Action<Slot> CardPicked;
        public static Action<Vector3, CardStatistics, Slot> PlaceUnit;
        public static Action<CardStatistics, Slot> RecreateDeck;
    }
}