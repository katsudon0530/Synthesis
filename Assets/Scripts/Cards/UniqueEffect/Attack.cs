using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Attack")]
public class AttackEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        CardStatus buffStatus = FlontBuff(battle.card);

        int Hit = (int)(buffStatus.Attack_Status * Random.Range(0.8f, 1.2f));
        int damage = battle.DamegeCalculation(Hit,DamageType.Attack);

        battle.log.SendMessage($"{damage}ダメージ与えた");

        yield break;
    }
}
