using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Naught")]
public class NaughtEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }

    //カードの効果処理
    public override IEnumerator Execute(Card card, Card flontCard, Enemy enemy)
    {
        int naughtValue = (int)FlontBuff(card, flontCard);

        MessageText.TextIn("何も起こらない");

        yield break;
    }

    //一枚前のカードの追加効果処理
    public float FlontBuff(Card card, Card flontCard)
    {
        string cardName = card.Base.CardName;
        float naughtValue = card.Base.CardStatus.Heal_Status;

        if (flontCard == null)
        {
            return naughtValue;
        }
        else
        {
            FlontBuff foundBuff = flontCard.Base.FlontBuff.Find(buff => buff.flontCard == cardName);

            if (foundBuff == null)
            {
                return (int)naughtValue;
            }
            naughtValue *= foundBuff.buff;
            return naughtValue;
        }

    }
}
