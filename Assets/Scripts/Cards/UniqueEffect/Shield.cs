using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Shield")]
public class ShieldEffect : UniqueEffect
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

        player.Defens += (int)buffStatus.Defense_Status;
        
        if (player.Defens > 100)
        {
            player.Defens = 100;
        }
        battle.log.SendMessage($"{player.Defens}ぼうぎょがあがった");

        yield break;
    }
}

