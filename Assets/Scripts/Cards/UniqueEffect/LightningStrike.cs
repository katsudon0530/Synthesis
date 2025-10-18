using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/LightningStrike")]
public class LightningStrikeEffect : UniqueEffect
{
    [SerializeField] StatusEffectBase effectBase;
    [SerializeField] float probability;

    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.Base.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(Card card, Card flontCard, Enemy enemy)
    {
        //定数ダメージを与える
        MessageText.TextIn($"雷撃を放った！");
        yield return new WaitForSeconds(1.0f);

        int damage = (int)card.Base.CardStatus.Attack_Status;
        enemy.Life -= damage;
        MessageText.TextIn($"{damage}ダメージ与えた");
        if (enemy.Life < 0)
        {
            enemy.Life = 0;
            yield break;
        }
        yield return new WaitForSeconds(1.0f);

        //帯電状態を付与する
        //帯電状態が付与されていない場合
        if (!enemy.Base.Effects.Exists(e => e.Base == effectBase))
        {
            MessageText.TextIn(enemy.EffectGenerator.EnemyGrantEffect(enemy, effectBase));
        }
        //すでに帯電状態の場合
        else
        {
            StatusEffect effect = enemy.Base.Effects.Find(e => e.Base == effectBase);
            effect.CountGrant++;
            effect.CountTurn = effectBase.EffectCount;
            MessageText.TextIn($"帯電状態を{effect.CountGrant}回付与した。");
        }
        yield return new WaitForSeconds(1.0f);

        //確率で相手は行動不能
        if(probability >= Random.value)
        {
            enemy.Act = false;
            MessageText.TextIn($"{enemy.Base.Name}はしびれている");
            yield return new WaitForSeconds(1.0f);
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