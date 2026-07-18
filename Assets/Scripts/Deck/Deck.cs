using System.Collections.Generic;
using UnityEngine;

public class Deck : Singleton<Deck>
{
    protected override bool IsPersistent => true;

    public List<int> Base = new List<int> { 101, 101, 101, 101, 102, 102, 102, 102, 103, 103, 103, 103, 104, 104, 104};
    public List<int> DeckAll;
    public List<int> cardDeck;

    public bool OnCustomize = true;
    private GameObject generator;

    private void Start()
    {
        //OnCustomize = true;
        DeckAll = new List<int>(Base);
    }
    //カスタマイズしたデッキをセットする
    public void DeckSet()
    {
        if (DeckAll ==null)
            DeckAll = new List<int>(Base);
        cardDeck = new List<int>(DeckAll);
        
        OnCustomize = false;
        generator = GameObject.FindWithTag("CardGenerator");
        if (generator == null)
            Debug.LogError("CardGeneratorが存在しません");
    }

    //カードを手札に生成する
    public Card Draw()
    {
        int num = UnityEngine.Random.Range(0, cardDeck.Count);
        Card card = generator.GetComponent<CardGenerator>().Spawn(cardDeck[num]);
        cardDeck.RemoveAt(num);
        return card;
    }
}
