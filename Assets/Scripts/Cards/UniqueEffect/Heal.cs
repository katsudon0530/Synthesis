using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Heal")]
public class HealEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }

    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        Player player = Player.Instance;
        CardStatus buffStatus = FlontBuff(battle.card);

        if ((player.Life + buffStatus.Heal_Status) > player.LifeMax)
        {
            buffStatus.Heal_Status = player.LifeMax - player.Life;
        }
        player.Life += (int)buffStatus.Heal_Status;
        battle.log.SendMessage($"{(int)buffStatus.Heal_Status}HPかいふくした");

        yield break;
    }
}
