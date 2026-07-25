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
        CardStatus buffStatus = FlontBuff(battle.card);

        int Hit = (int)(buffStatus.Magic_Status * Random.Range(0.8f, 1.2f));
        int damage = battle.DamegeCalculation(Hit, DamageType.Magic);

        battle.log.SendMessage($"{damage}魔法ダメージあたえた");

        yield break;
    }
}
