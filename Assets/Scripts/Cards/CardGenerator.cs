using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{

    [SerializeField] Card cardPrefab;
    private Dictionary<int, CardBase> cardsById;


    private void Awake()
    {
        cardsById = Resources.LoadAll<CardBase>("CardBase").ToDictionary(card => card.ID);
    }

    //ナンバーからカードを生成する
    public Card Spawn(int id)
    {
        Card card = Instantiate(cardPrefab);
        card.Set(cardsById[id]);
        return card;
    }

    //カードの情報を更新する
    public Card ChangeCard(Card card, int id)
    {
        card.Set(cardsById[id]);
        return card;
    }

    public List<Card> AllCardsSpawn()
    {
        List<Card> allCards = new List<Card>();
        foreach(var cards in cardsById)
        {
            var card = Spawn(cards.Key);
            allCards.Add(card);
        }
        return allCards;
    }

}
