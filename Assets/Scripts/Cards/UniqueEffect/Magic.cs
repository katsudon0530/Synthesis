using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Magic")]
public class MagicEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.Base.PlayCondition = true;
    }

    //カードの効果処理
    public override IEnumerator Execute(Card card, Card flontCard, Enemy enemy)
    {
        int magicValue = (int)FlontBuff(card, flontCard);

        int Hit = (int)(magicValue * Random.Range(0.8f, 1.2f));
        float defense = 1f - enemy.Base.EnemyMagicDefense / 100f;
        int damage = (int)(Hit * defense);
        enemy.Base.EnemyLife -= damage;
        MessageText.TextIn($"{damage}魔法ダメージあたえた");
        if (enemy.Base.EnemyLife < 0)
        {
            enemy.Base.EnemyLife = 0;
        }

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
