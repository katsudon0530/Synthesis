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
        int attackValue = FlontBuff(battle.card, battle.flontCard);

        player.Life -= CompenstionLife;
        MessageText.TextIn($"{CompenstionLife}HPの代償を払った！");
        yield return new WaitForSeconds(1.0f);

        int Hit = (int)(attackValue * Random.Range(0.8f, 1.2f));
        int damage = battle.DamegeCalculation(Hit, DamageType.Attack);
        MessageText.TextIn($"{damage}ダメージ与えた");

        yield break;
    }
    //一枚前のカードの追加効果処理
    public int FlontBuff(Card card, Card flontCard)
    {

        float attackValue = (int)card.Base.CardStatus.Attack_Status;


        if (flontCard == null)
        {
            return (int)attackValue;
        }
        else
        {
            string cardName = flontCard.Base.CardName;
            FlontBuff foundBuff = card.Base.FlontBuff.Find(buff => buff.flontCard == cardName);

            if (foundBuff == null)
            {
                return (int)attackValue;
            }
            attackValue *= foundBuff.buff;
            return (int)attackValue;
        }


    }
}