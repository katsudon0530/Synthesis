using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Reflector")]
public class ReflectorEffect : UniqueEffect
{
    [SerializeField] StatusEffectBase effectBase;
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }

    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        Player player = Player.Instance;
        CardStatus status = FlontBuff(battle.card);

        player.Defens += (int)status.Defense_Status;

        if (player.Defens > 100)
        {
            player.Defens = 100;
        }
        battle.log.SendMessage($"{player.Defens}ぼうぎょがあがった");

        //すでに反射状態が付与されていた場合
        if (battle.target.Base.Effects.Exists(e => e.Base == effectBase))
        {
            //StatusEffect effect = enemy.Base.Effects.Find(e => e.Base == effectBase);
            battle.log.SendMessage($"すでに反射状態だ！");
            yield return new WaitForSeconds(1.0f);
            yield break;
        }

        battle.log.SendMessage(battle.target.EffectGenerator.PlayerGrantEffect(player, effectBase));


        yield break;
    }
}
