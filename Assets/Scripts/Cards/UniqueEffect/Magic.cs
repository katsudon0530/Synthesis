using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Magic")]
public class MagicEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }

    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        int magicValue = (int)FlontBuff(battle.card, battle.flontCard);

        int Hit = (int)(magicValue * Random.Range(0.8f, 1.2f));
        int damage = battle.DamegeCalculation(Hit, DamageType.Magic);

        MessageText.TextIn($"{damage}魔法ダメージあたえた");

        yield break;
    }

    //一枚前のカードの追加効果処理
    public float FlontBuff(Card card, Card flontCard)
    {

        float magicValue = card.Base.CardStatus.Magic_Status;
        if (flontCard == null)
        {
            return magicValue;
        }
        else
        {
            string cardName = flontCard.Base.CardName;
            FlontBuff foundBuff = card.Base.FlontBuff.Find(buff => buff.flontCard == cardName);

            if (foundBuff == null)
            {
                return (int)magicValue;
            }
            magicValue *= foundBuff.buff;
            return magicValue;
        }

    }
}
