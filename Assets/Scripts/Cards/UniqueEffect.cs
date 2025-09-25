using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public abstract class UniqueEffect : ScriptableObject
{
    public abstract void PlayCondition(Card card, Player player, Enemy enemy, Deck deck, int TurnCount);
    public abstract IEnumerator Execute(Card card ,Card flontCard, Player player, Enemy enemy);

    
}
