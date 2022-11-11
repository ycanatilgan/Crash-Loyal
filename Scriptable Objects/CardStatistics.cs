using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Create New Card Data", order = 1)]
public class CardStatistics : ScriptableObject
{
    public GameObject cardPrefab;
    public Sprite cardImage;
    public int elixir;
    public bool isSpell;
}
