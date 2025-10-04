using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : Singleton<Deck>
{

    [SerializeField] Text RestText;
    [SerializeField] DeckCustomize customize;

    public List<int> Base;
    public List<int> DeckAll;
    public List<int> cardDeck;

    public bool OnCustomize;

    private void Start()
    {
        OnCustomize = true;
    }
    //カスタマイズしたデッキをセットする
    public void DeckSet()
    {
        cardDeck = new List<int>(DeckAll);
        OnCustomize = false;
        customize.OffList();
    }


    //デッキの残り枚数を表示する
    public void RestDeck()
    {
        RestText.text = $"残り{cardDeck.Count}枚";
    }
    
    public void Setlist()
    {
        customize.DeckListOpen();
    }
}
