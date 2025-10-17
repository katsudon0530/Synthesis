using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public abstract class UniqueEffect : ScriptableObject
{
    public abstract void PlayCondition(Card card, Enemy enemy);
    public abstract IEnumerator Execute(Card card ,Card flontCard, Enemy enemy);

    
}
