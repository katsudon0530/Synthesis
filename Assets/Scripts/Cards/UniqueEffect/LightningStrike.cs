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
        card.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        //定数ダメージを与える
        MessageText.TextIn($"雷撃を放った！");
        yield return new WaitForSeconds(1.0f);

        int hit = (int)battle.card.Base.CardStatus.Attack_Status;
        int damage = battle.DamegeCalculation(hit, DamageType.True);
        MessageText.TextIn($"{damage}ダメージ与えた");
        if (battle.target.Life < 0)
        {
            yield break;
        }
        yield return new WaitForSeconds(1.0f);

        //帯電状態を付与する
        //帯電状態が付与されていない場合
        if (!battle.target.Base.Effects.Exists(e => e.Base == effectBase))
        {
            MessageText.TextIn(battle.target.EffectGenerator.EnemyGrantEffect(battle.target, effectBase));
        }
        //すでに帯電状態の場合
        else
        {
            StatusEffect effect = battle.target.Base.Effects.Find(e => e.Base == effectBase);
            effect.CountGrant++;
            effect.CountTurn = effectBase.EffectCount;
            MessageText.TextIn($"帯電状態を{effect.CountGrant}回付与した。");
        }
        yield return new WaitForSeconds(1.0f);

        //確率で相手は行動不能
        if(probability >= Random.value)
        {
            battle.target.Act = false;
            MessageText.TextIn($"{battle.target.Base.Name}はしびれている");
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