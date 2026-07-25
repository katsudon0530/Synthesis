using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Compenstion")]
public class CompenstionEffect : UniqueEffect
{
    [SerializeField] int CompenstionLife;
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = false;
        if (Player.Instance.Life > CompenstionLife)
            card.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        Player player = Player.Instance;
        CardStatus buffStatus = FlontBuff(battle.card);


        player.Life -= CompenstionLife;
        battle.log.SendMessage($"{CompenstionLife}HPの代償を払った！");
        yield return new WaitForSeconds(1.0f);

        int Hit = (int)(buffStatus.Attack_Status * Random.Range(0.8f, 1.2f));
        int damage = battle.DamegeCalculation(Hit, DamageType.Attack);
        battle.log.SendMessage($"{damage}ダメージ与えた");

        yield break;
    }
}