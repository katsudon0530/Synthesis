using UnityEngine;

public class BattleContext
{
    public Card card;
    public Card flontCard;
    public Enemy target;

    public void SetContext(Card setCard,Card setFlontCard,Enemy SetEnemy)
    {
        card = setCard;
        flontCard = setFlontCard;
        target = SetEnemy;
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