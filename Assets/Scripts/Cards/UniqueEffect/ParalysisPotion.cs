using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "UniqueEffects/ParalysisPotion")]
public class ParalysisPotionEffect : UniqueEffect
{
    [SerializeField] StatusEffectBase effectBase;

    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {

        battle.log.SendMessage($"麻痺ポーションを投げつけた！");  
        yield return new WaitForSeconds(1.0f);

        battle.log.SendMessage(battle.target.EffectGenerator.EnemyGrantEffect(battle.target, effectBase));

        yield break;
    }
}
