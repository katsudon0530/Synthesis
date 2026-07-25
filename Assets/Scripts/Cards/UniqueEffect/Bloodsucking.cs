using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Bloodsucking")]
public class BloodsuckingEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        CardStatus status = FlontBuff(battle.card); 
        Player player = Player.Instance;

        int Hit = (int)(status.Attack_Status * Random.Range(0.8f, 1.2f));
        int damage = battle.DamegeCalculation(Hit, DamageType.Attack);

        battle.log.SendMessage($"{damage}ダメージ与えた");
        yield return new WaitForSeconds(1.0f);

        if ((player.Life + damage) > player.LifeMax)
        {
            damage = player.LifeMax - player.Life;
        }
        player.Life += damage;
        battle.log.SendMessage($"吸血で{damage}HP吸い取った");

        yield break;
    }
}

