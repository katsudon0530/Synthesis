using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/PosonPotion")]
public class PosonPotionEffect : UniqueEffect
{
    [SerializeField] StatusEffectBase effectBase;
    
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {

        battle.log.SendMessage($"毒ポーションを投げつけた！");
        yield return new WaitForSeconds(1.0f);

        //すでに毒状態が付与されていた場合
        if (battle.target.Base.Effects.Exists(e => e.Base == effectBase))
        {
            StatusEffect effect = battle.target.Base.Effects.Find(e => e.Base == effectBase);
            effect.CountTurn = effectBase.EffectCount;
            effect.CountGrant++;
            Debug.Log(effect.CountGrant);
            battle.log.SendMessage($"毒状態を延長した！");
            yield return new WaitForSeconds(1.0f);
            yield break;
        }

        //毒状態を付与する
        battle.log.SendMessage(battle.target.EffectGenerator.EnemyGrantEffect(battle.target, effectBase));

        yield break;
    }

}
