using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/ResetDack")]
public class ResetDack : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }
    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        var field = Field.Instance;
        var deck = Deck.Instance;

        foreach (Card card in field.Hand)
        {
            Destroy(card.gameObject);
        }

        field.Hand.Clear();
        deck.cardDeck.Clear();

        battle.log.SendMessage($"デッキと手札をリセットした！");

        yield break;
    }
}
