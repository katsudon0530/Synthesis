using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public abstract class UniqueEffect : ScriptableObject
{
    public abstract void PlayCondition(Card card, Enemy enemy);
    public abstract IEnumerator Execute(BattleContext battle);

    public CardStatus FlontBuff(Card card)
    {
        CardStatus status = new CardStatus(card.Base.CardStatus);
        int index = Field.Instance.Stand.IndexOf(card);

        if (index == 0)
        {
            return status;
        }
        else
        {
            Card flontCard = Field.Instance.Stand[index - 1];
            string cardName = flontCard.Base.CardName;
            FlontBuff foundBuff = card.Base.FlontBuff.Find(buff => buff.flontCard == cardName);

            if (foundBuff != null)
            {
                status.MultiplyAllStatus(foundBuff.buff);
            }
            return status;
        }
    }
}
