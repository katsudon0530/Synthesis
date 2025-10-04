using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : Singleton<Deck>
{
    protected override bool IsPersistent => true;

    public List<int> Base = new List<int> { 101, 101, 101, 101, 102, 102, 102, 102, 103, 103, 103, 103, 104, 104, 104};
    public List<int> DeckAll;
    public List<int> cardDeck;

    public bool OnCustomize;

    protected override void Awake()
    {
        base.Awake(); // これで親のDontDestroy処理が必ず実行される
    }

    private void Start()
    {
        OnCustomize = true;
        DeckAll = new List<int>(Base);
    }
    //カスタマイズしたデッキをセットする
    public void DeckSet()
    {
        if (DeckAll ==null)
            DeckAll = new List<int>(Base);
        cardDeck = new List<int>(DeckAll);
        OnCustomize = false;
    }
}
