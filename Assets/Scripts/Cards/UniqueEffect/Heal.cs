using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Heal")]
public class HealEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Player player, Enemy enemy, Deck deck, int TurnCount)
    {
        card.Base.PlayCondition = true;
    }

    //カードの効果処理
    public override IEnumerator Execute(Card card, Card flontCard, Player player, Enemy enemy)
    {
        int healValue = (int)FlontBuff(card, flontCard);

        if ((player.Life + healValue) > player.LifeMax)
        {
            healValue = player.LifeMax - player.Life;
        }
        player.Life += healValue;
        MessageText.message.text = $"{healValue}HPかいふくした";

        yield break;
    }

    //一枚前のカードの追加効果処理
    public float FlontBuff(Card card, Card flontCard)
    {
        float healValue = card.Base.CardStatus.Heal_Status;

        if (flontCard == null)
        {
            return healValue;
        }
        else
        {
            string cardName = flontCard.Base.CardName;
            FlontBuff foundBuff = card.Base.FlontBuff.Find(buff => buff.flontCard == cardName);

            if (foundBuff == null)
            {
                return (int)healValue;
            }
            healValue *= foundBuff.buff;
            return healValue;
        }

    }
}
