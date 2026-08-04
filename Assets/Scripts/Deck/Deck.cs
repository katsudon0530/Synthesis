using System.Collections.Generic;
using UnityEngine;

public class Deck : Singleton<Deck>
{
    protected override bool IsPersistent => true;

    public List<int> Base = new List<int> { 101, 101, 101, 101, 101, 102, 102, 102, 102, 103, 103, 103, 103, 104, 104, 104, 104, 108, 108, 110};
    public List<int> CustomDeck;
    public List<int> DeckAll;
    public List<int> cardDeck;

    public bool OnCustomize = true;
    private GameObject generator;

    private void Awake()
    {
        base.Awake();
        //OnCustomize = true;
        DeckAll = new List<int>(Base);
        CustomDeck = new List<int>(Base);
    }
    //カスタマイズしたデッキをセットする
    public void DeckSet()
    {
        if (CustomDeck == null)
            CustomDeck = new List<int>(Base);
        DeckAll = new List<int>(CustomDeck);
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

    private void SceneChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.costom:
                Deck.Instance.OnCustomize = true;
                break;
            case GameState.title:
            case GameState.singleGame:
            case GameState.dungeon:
                Deck.Instance.OnCustomize = false;
                DeckAll = new List<int>(CustomDeck);
                break;

        }
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += SceneChange;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= SceneChange;
    }
}
