using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Dice")]
public class DiceEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        int dice = Random.Range(1, 7);
        Player player = Player.Instance;

        battle.log.SendMessage($"さいころを振った！");
        yield return new WaitForSeconds(1.0f);
        battle.log.SendMessage($"出た目は{dice}！");
        yield return new WaitForSeconds(1.0f);

        CardStatus buffStatus = FlontBuff(battle.card);

        switch (dice)
        {
            case 1:
                Attack(battle, buffStatus);
                break;
            case 2:
                Magic(battle, buffStatus);
                break;
            case 3:
                Shield(battle, buffStatus);
                break;
            case 4:
                Heal(battle, buffStatus);
                break;
            case 5:
                battle.log.SendMessage($"はずれ！");
                break;
            case 6:
                battle.log.SendMessage($"あたり！");
                break;
        }

        yield break;
    }
    private void Attack(BattleContext battle,CardStatus status)
    {

        int Hit = (int)(status.Attack_Status * Random.Range(0.8f, 1.2f));
        int damage = battle.DamegeCalculation(Hit, DamageType.Attack);

        battle.log.SendMessage($"{damage}ダメージ与えた");
    }
    private void Magic(BattleContext battle, CardStatus status)
    {
        int Hit = (int)(status.Magic_Status * Random.Range(0.8f, 1.2f));
        int damage = battle.DamegeCalculation(Hit, DamageType.Magic);

        battle.log.SendMessage($"{damage}魔法ダメージあたえた");
    }
    private void Shield(BattleContext battle, CardStatus status)
    {
        Player player = Player.Instance;

        player.Defens += (int)status.Defense_Status;

        if (player.Defens > 100)
        {
            player.Defens = 100;
        }
        battle.log.SendMessage($"{player.Defens}ぼうぎょがあがった");
    }
    private void Heal(BattleContext battle, CardStatus status)
    {
        Player player = Player.Instance;

        if ((player.Life + status.Heal_Status) > player.LifeMax)
        {
            status.Heal_Status = player.LifeMax - player.Life;
        }
        player.Life += (int)status.Heal_Status;
        battle.log.SendMessage($"{(int)status.Heal_Status}HPかいふくした");
    }
}
