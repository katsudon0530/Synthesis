using UnityEngine;


public class DeckCustomize : MonoBehaviour
{
    [SerializeField] GameObject deckContents;
    [SerializeField] GameObject cardContents;
    [SerializeField] GameObject exitButton;

    public Deck deck;
    
    public void SerCardToCustom(Card card)
    {
        card.OnClickCard = SelectedDeckCard;
    }

    //カードクリック時のリアクション
    public void SelectedDeckCard(Card card)
    {
        if (card.transform.parent == deckContents.transform)
        {
            int dest = deck.DeckAll.FindIndex(number => number == card.Base.ID);
            deck.DeckAll.RemoveAt(dest);

            Destroy(deck.LookDeck[dest].gameObject);
            deck.LookDeck.RemoveAt(dest);

            deck.deckArignment();
            card.PosReset();
        }
        else if (deck.DeckAll.Count < 15 && card.transform.parent == cardContents.transform)
        {
            deck.DeckAll.Add(card.Base.ID);
            Card newCard = deck.Generator.Spawn(card.Base.ID);
            SerCardToCustom(newCard);
            newCard.transform.SetParent(deckContents.transform);
            deck.LookDeck.Add(newCard);

            deck.deckArignment();
        }

        CustomizeCompletion();

    }

    //デッキが15枚以下の時戻るボタンを消す
    void CustomizeCompletion()
    {
        if (deck.DeckAll.Count == 15)
        {
            exitButton.SetActive(true);
        }
        else
        {
            exitButton.SetActive(false);
        }
        
    }
}
