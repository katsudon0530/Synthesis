using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/BattleLog")]
public class BattleLog : ScriptableObject
{
    public event Action<string> OnMessage;

    public void SendMessage(string message)
    {
        OnMessage?.Invoke(message);
    }
}