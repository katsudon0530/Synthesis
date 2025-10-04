using System.Collections.Generic;
using UnityEngine;

public class DrawCard : MonoBehaviour
{
    [SerializeField] CardGenerator cardGenerator;

    void DrawCardMax(Player player, Deck deck, int handMax)
    {
        int cardsum = 0;

        if (deck.cardDeck.Count != 0)
        {
            cardsum = handMax - Hand.Instance.List.Count;
            if (cardsum > deck.cardDeck.Count)
            {
                cardsum = deck.cardDeck.Count;
            }
        }
        else if (deck.cardDeck.Count == 0 && Hand.Instance.List.Count == 0)
        {
            deck.cardDeck = new List<int>(deck.DeckAll);
            cardsum = handMax;
            if (cardsum > deck.cardDeck.Count)
            {
                cardsum = deck.cardDeck.Count;
            }
        }
        else if (deck.cardDeck.Count == 0)
        {
            cardsum = 0;
        }

        for (int i = 0; i < cardsum; i++)
        {
            int num = Random.Range(0, deck.cardDeck.Count);
            Card card = cardGenerator.Spawn(deck.cardDeck[num]);
            deck.cardDeck.RemoveAt(num);
            card.effectReSet();
            player.SerCardToHand(card);
        }
        Hand.Instance.ResetPosition();
    }
}
