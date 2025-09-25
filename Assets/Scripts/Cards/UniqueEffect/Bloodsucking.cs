using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Bloodsucking")]
public class BloodsuckingEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Player player, Enemy enemy, Deck deck, int TurnCount)
    {
        card.Base.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(Card card, Card flontCard, Player player, Enemy enemy)
    {
        int attackValue = FlontBuff(card, flontCard);

        int Hit = (int)(attackValue * Random.Range(0.8f, 1.2f));
        float defense = 1f - enemy.Base.EnemyDefense / 100f;
        int damage = (int)(Hit * defense);
        enemy.Base.EnemyLife -= damage;
        MessageText.TextIn($"{damage}ダメージ与えた");
        yield return new WaitForSeconds(1.0f);
        if (enemy.Base.EnemyLife < 0)
        {
            enemy.Base.EnemyLife = 0;
        }

        if ((player.Life + damage) > player.LifeMax)
        {
            damage = player.LifeMax - player.Life;
        }
        player.Life += damage;
        MessageText.TextIn($"吸血で{damage}HP吸い取った");

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

