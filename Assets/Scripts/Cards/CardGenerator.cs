using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{

    [SerializeField] Card cardPrefab;
    [SerializeField] List<CardBase> cardBases;

    public List<CardBase> CardBases { get => cardBases; set => cardBases = value; }

    //ナンバーからカードを生成する
    public Card Spawn(int id)
    {
        CardBase spawnCard = cardBases.Find(x => x.ID == id);
        Card card = Instantiate(cardPrefab);
        card.Set(spawnCard);
        return card;
    }

    //カードの情報を更新する
    public Card ChangeCard(Card card, int id)
    {
        CardBase spawnCard = cardBases.Find(x => x.ID == id);
        card.Set(spawnCard);
        return card;
    }

}
