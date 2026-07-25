using System;
using UnityEngine;

public class BattleContext
{
    public Card card;
    public Enemy target;
    public BattleLog log;

    public void SetContext(Card setCard,Enemy SetEnemy, BattleLog battleLog)
    {
        card = setCard;
        target = SetEnemy;
        log = battleLog;
    }
    public int DamegeCalculation(int hit,DamageType type)
    {
        int damage = 0;
        if(target != null)
            damage = target.ReceiveDamage(hit,type);
        return damage;
    }
}

public struct DamageRequest
{
    public int Hit;
    public Card card;
    public Enemy Target;
}