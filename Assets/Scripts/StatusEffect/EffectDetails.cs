using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class EffectDetails : ScriptableObject
{
    public abstract IEnumerator Execute(StatusEffect statusEffect, Player player, Enemy enemy);
}
