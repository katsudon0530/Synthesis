using System.Collections.Generic;
using UI;
using UnityEngine;


public class DeckCustomize : MonoBehaviour
{
    [SerializeField] CardGenerator generator;
    [SerializeField] GameObject deckContents;
    [SerializeField] GameObject cardContents;
    [SerializeField] GameObject CardList;
    [SerializeField] ButtonUI exitButton;

    [Header("デッキカスタムリスト")]
    [SerializeField] CardLayoutSetting deckSetting;
    [Header("カスタムカードリスト")]
    [SerializeField] CardLayoutSetting cardSetting;

    public List<Card> LookDeck;
    public List<Card> LookCards;

    private Deck deck;

    //ゲーム開始時にデッキをデフォルト状態にする
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        gameObject.SetActive(false);
        deck = Deck.Instance;
    }

    public void OnPanel()
    {
        gameObject.SetActive(true);
        DeckListOpen();
        CustomCardListOpen();
    }

    public void SerCardToCustom(Card card)
    {
        card.OnClickCard = SelectedDeckCard;
    }

    //カードクリック時のリアクション
    public void SelectedDeckCard(Card card)
    {
        if (!deck.OnCustomize)
            return;
        if (card.Base.SynthesisType != SynthesisType.Normal)
            return;
        if (card.transform.parent == deckContents.transform)
        {
            int dest = deck.DeckAll.FindIndex(number => number == card.Base.ID);
            deck.DeckAll.RemoveAt(dest);

            dest = LookDeck.FindIndex(number => number.InstanceId == card.InstanceId);
            Destroy(LookDeck[dest].gameObject);
            LookDeck.RemoveAt(dest);

            Alignment(LookDeck,deckSetting);
        }
        else if (deck.DeckAll.Count < 15 && card.transform.parent == cardContents.transform)
        {
            deck.DeckAll.Add(card.Base.ID);
            Card newCard = generator.Spawn(card.Base.ID);
            SerCardToCustom(newCard);
            newCard.transform.SetParent(deckContents.transform);
            LookDeck.Add(newCard);

            Alignment(LookDeck, deckSetting);
        }

        CustomizeCompletion();
    }

    public void OffList()
    {
        CardList.SetActive(false);
    }

    //デッキリストの一覧を表示する
    public void DeckListOpen()
    {

        for (int i = deckContents.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(deckContents.transform.GetChild(i).gameObject);
        }

        if(LookDeck != null)
            LookDeck.Clear();

        for (int i = 0; i < deck.DeckAll.Count; i++)
        {
            Card card = generator.Spawn(deck.DeckAll[i]);
            LookDeck.Add(card);
            card.transform.SetParent(deckContents.transform);
            SerCardToCustom(card);
        }

        Alignment(LookDeck, deckSetting);
    }

    //カスタマイズするためのカードを表示する
    public void CustomCardListOpen()
    {
        for (int i = cardContents.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(cardContents.transform.GetChild(i).gameObject);
        }

        if (LookCards != null)
            LookCards.Clear();

        List<Card> allCards = generator.AllCardsSpawn();
        foreach (Card card in allCards)
        {
            card.transform.localScale = Vector3.one * 0.8f;
            LookCards.Add(card);
            card.transform.SetParent(cardContents.transform);
            SerCardToCustom(card);
        }
        Alignment(LookCards, cardSetting);
    }
    //リストのカードを指定されたコンテンツに並べる
    public void Alignment(List<Card> cardList, CardLayoutSetting set)
    {
        int posX;
        float posY;
        int CardLookCount = set.Width;

        int a = (cardList.Count + set.Width - 1) / set.Width;
        cardList.Sort((card0, card1) => card0.Base.ID - card1.Base.ID);

        //スクロール縦幅の設定
        set.Contents.sizeDelta = new Vector2(set.Contents.sizeDelta.x, set.Height * a + 50);

        //横の幅を設定する
        set.Scroll.sizeDelta = new Vector2(set.CardWidth * CardLookCount, set.Scroll.sizeDelta.y);
        //set.Scroll.localPosition = new Vector3(240f * CardLookCount / 2 + 350, -120);

        for (int i = 0; i < a; i++)
        {
            if (a % 2 == 0)
            {
                posY = (float)((i - a / 2) * -set.Height - set.Height / 2);
            }
            else
            {
                posY = (float)((i - a / 2) * - set.Height);
            }

            if (cardList.Count <= ((i + 1) * set.Width))
            {
                CardLookCount = cardList.Count - i * set.Width;
                ;
            }
            for (int j = 0; j < CardLookCount; j++)
            {
                if (set.Width % 2 == 0)
                {
                    posX = (int)((j - set.Width / 2) * set.Interval + set.Interval / 2);
                }
                else
                {
                    posX = (int)((j - set.Width / 2) * set.Interval);
                }
                cardList[j + i * set.Width].transform.localPosition = new Vector3(posX, posY);
            }
        }
    }

    //デッキが15枚以下の時戻るボタンを消す
    void CustomizeCompletion()
    {
        if (deck.DeckAll.Count == 15)
        {
            exitButton.Interactable = true;
        }
        else
        {
            exitButton.Interactable = false;
        }
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class CardLayoutSetting
{
    public int CardWidth;
    public int Width;
    public int Height;
    public float Interval;

    public RectTransform Scroll;
    public RectTransform Contents;
}
