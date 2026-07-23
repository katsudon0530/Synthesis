using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Dice")]
public class DiceEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(Card card, Card flontCard,Enemy enemy)
    {
        int attackValue = FlontBuff(card, flontCard);

        int Hit = (int)(attackValue * Random.Range(0.8f, 1.2f));
        float defense = 1f - enemy.Defense / 100f;
        int damage = (int)(Hit * defense);
        enemy.Life -= damage;
        MessageText.TextIn($"{damage}ダメージ与えた");
        if (enemy.Life < 0)
        {
            enemy.Life = 0;
        }

        yield break;
    }
    //一枚前のカードの追加効果処理
    public int FlontBuff(Card card, Card flontCard)
    {
        
        float attackValue = (int)card.Base.CardStatus.Attack_Status;
        
        
        if (flontCard == null)
        {
            return (int)attackValue;
        }
        else
        {
            string cardName = flontCard.Base.CardName;
            FlontBuff foundBuff = card.Base.FlontBuff.Find(buff => buff.flontCard == cardName);

            if (foundBuff == null)
            {
                return (int)attackValue;
            }
            attackValue *= foundBuff.buff;
            return (int)attackValue;
        }
        

    }
}
