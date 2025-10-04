using System.Collections.Generic;
using UI;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public class DeckCustomize : MonoBehaviour
{
    [SerializeField] CardGenerator generator;
    [SerializeField] GameObject deckContents;
    [SerializeField] GameObject cardContents;
    [SerializeField] GameObject Contents;
    [SerializeField] GameObject Contents2;
    [SerializeField] GameObject ScrollScale;
    [SerializeField] GameObject CardList;
    [SerializeField] ButtonUI exitButton;

    [SerializeField] int DeckWidth = 5;
    [SerializeField] int DeckHeight = 270;
    [SerializeField] float cardInterval = 210;

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
        deck.DeckAll = new List<int>(deck.Base);
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
        if (card.transform.parent == deckContents.transform)
        {
            int dest = deck.DeckAll.FindIndex(number => number == card.Base.ID);
            deck.DeckAll.RemoveAt(dest);

            Destroy(LookDeck[dest].gameObject);
            LookDeck.RemoveAt(dest);

            deckArignment();
            card.PosReset();
        }
        else if (deck.DeckAll.Count < 15 && card.transform.parent == cardContents.transform)
        {
            deck.DeckAll.Add(card.Base.ID);
            Card newCard = generator.Spawn(card.Base.ID);
            SerCardToCustom(newCard);
            newCard.transform.SetParent(deckContents.transform);
            LookDeck.Add(newCard);

            deckArignment();
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

        deckArignment();
    }

    //カスタマイズするためのカードを表示する
    public void CustomCardListOpen()
    {

        foreach (CardBase i in generator.CardBases)
        {
            if (i.SynthesisType == SynthesisType.Normal)
            {
                Card card = generator.Spawn(i.ID);
                LookCards.Add(card);
                card.transform.SetParent(cardContents.transform);
                SerCardToCustom(card);
            }
        }
        CustomCardArignment();
    }

    //デッキをソートして配置する
    public void deckArignment()
    {
        Vector2 currentScale;
        int posX;
        float posY;
        int DeckLookCount = DeckWidth;

        int a = (LookDeck.Count + DeckWidth - 1) / DeckWidth;
        LookDeck.Sort((card0, card1) => card0.Base.ID - card1.Base.ID);
        deck.DeckAll.Sort();

        //スクロール縦幅の設定
        currentScale = Contents.GetComponent<RectTransform>().sizeDelta;
        currentScale.y = DeckHeight * a + 50;
        Contents.GetComponent<RectTransform>().sizeDelta = currentScale;

        //横の幅を設定する
        currentScale = ScrollScale.GetComponent<RectTransform>().sizeDelta;
        currentScale.x = 240f * DeckLookCount;
        ScrollScale.GetComponent<RectTransform>().sizeDelta = currentScale;
        ScrollScale.transform.localPosition = new Vector3(currentScale.x / 2 - 825, -25);

        for (int i = 0; i < a; i++)
        {
            if (a % 2 == 0)
            {
                posY = (float)((i - a / 2) * -DeckHeight - DeckHeight / 2);
            }
            else
            {
                posY = (float)((i - a / 2) * -DeckHeight);
            }

            if (LookDeck.Count <= ((i + 1) * DeckWidth))
            {
                DeckLookCount = LookDeck.Count - i * DeckWidth;
                ;
            }
            for (int j = 0; j < DeckLookCount; j++)
            {
                if (DeckWidth % 2 == 0)
                {
                    posX = (int)((j - DeckWidth / 2) * cardInterval + cardInterval / 2);
                }
                else
                {
                    posX = (int)((j - DeckWidth / 2) * cardInterval);
                }
                LookDeck[j + i * DeckWidth].transform.localPosition = new Vector3(posX, posY);
            }
        }
    }

    //カスタマイズカードを並べる
    public void CustomCardArignment()
    {
        Vector2 currentScale;
        float posY;

        LookCards.Sort((card0, card1) => card0.Base.ID - card1.Base.ID);

        //スクロール縦幅の設定
        currentScale = Contents2.GetComponent<RectTransform>().sizeDelta;
        currentScale.y = DeckHeight * LookCards.Count + 50;
        Contents2.GetComponent<RectTransform>().sizeDelta = currentScale;

        for (int i = 0; i < LookCards.Count; i++)
        {
            if (LookCards.Count % 2 == 0)
            {
                posY = (float)((i - LookCards.Count / 2) * -DeckHeight - DeckHeight / 2);
            }
            else
            {
                posY = (float)((i - LookCards.Count / 2) * -DeckHeight);
            }


            LookCards[i].transform.localPosition = new Vector3(0, posY);

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
