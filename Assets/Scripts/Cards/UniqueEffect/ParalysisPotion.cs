using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "UniqueEffects/ParalysisPotion")]
public class ParalysisPotionEffect : UniqueEffect
{
    [SerializeField] StatusEffectBase effectBase;

    public override void PlayCondition(Card card, Player player, Enemy enemy, Deck deck, int TurnCount)
    {
        card.Base.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(Card card, Card flontCard, Player player, Enemy enemy)
    {

        MessageText.TextIn($"麻痺ポーションを投げつけた！");  
        yield return new WaitForSeconds(1.0f);

        MessageText.TextIn(enemy.StatusEffectGenerator.EnemyGrantEffect(enemy, effectBase));

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
