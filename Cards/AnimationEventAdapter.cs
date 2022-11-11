using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrashLoyal.Cards;
using CrashLoyal.Arena;

[RequireComponent(typeof(Animator))]
public class AnimationEventAdapter : MonoBehaviour
{
    IUnit unit;
    Tower tower;

    private void OnEnable()
    {
        unit = GetComponentInParent<IUnit>();
        tower = GetComponentInParent<Tower>();
    }

    void AttackEvent()
    {
        unit?.Hit();
        tower?.Hit();
    }
}
