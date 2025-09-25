using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMaster : MonoBehaviour
{
    [SerializeField] CardGenerator cardGenerator;
    [SerializeField] Deck deck;
    [SerializeField] GameUI gameUI;
    [SerializeField] int handMax;

    Enemy enemy;
    EnemyManager enemyManager;
    SynthesisManager synthesisManager;
    CardManager cardManager;
    Coroutine nowTurn;

    public int enemyNum;
    public static bool cardSet { get; private set; }
    public static int TurnCount;

    private void Start()
    {
        gameUI.UISetUp();
        deck.DeckDefault();
        enemyManager = GetComponent<EnemyManager>();
        synthesisManager = GetComponent<SynthesisManager>();
        cardManager = GetComponent<CardManager>();
    }

    //ゲームスタート時のセットアップ内容
    public void Setup()
    {
        Player.Instance.SetPlayer(handMax);
        //SendCardTo();

        enemy = enemyManager.GenerateEnemy(enemyNum);
        synthesisManager.SetButton();
        deck.DeckListOpen();
        deck.DeckSet();

        SendCardTo();

        gameUI.OnDecisionButton = DecisionAction;
        TurnCount = 1;
        gameUI.StartGameUI(enemy);

        nowTurn = StartCoroutine(turn());

    }

    //手札を生成
    void SendCardTo()
    {
        int cardsum;

        if(deck.cardDeck.Count != 0)
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
        else
        {
            cardsum = 0;
        }

        for (int i = 0; i < cardsum; i++)
        {
            int num = Random.Range(0, deck.cardDeck.Count);
            Card card = cardGenerator.Spawn(deck.cardDeck[num]);
            deck.cardDeck.RemoveAt(num);
            card.effectReSet();
            Player.Instance.SerCardToHand(card);
        }
        Hand.Instance.ResetPosition();
        deck.RestDeck();
    }

    IEnumerator turn()
    {

        yield return new WaitForSeconds(0.2f);
        while (!cardSet)
        {
            Player.Instance.PlayConditionCheck(enemy, deck); 
            yield return null; 
        }

        //カードが選択され、決定か合成が押されるまで待機
        yield return new WaitUntil(() => CardSet());

        gameUI.OffCardGuide();
        //どちらのボタンが押されたのかを判別してそのアクションを実行
        switch (gameUI.ButtonID)
        {
            case 0:
                //決定ボタンが押された場合
                yield return StartCoroutine(cardManager.CardBattle(enemy));
                break;
            case 1:
                //合成ボタンが押された場合
                yield return StartCoroutine(synthesisManager.CardSynthesis());
                break;
        }
        yield return new WaitForSeconds(0.7f);
        //敵に付与されている状態の処理
        yield return StartCoroutine(enemyManager.EnemyEffectBoot(enemy));


        //エネミーの行動を行う
        yield return StartCoroutine(enemyManager.EnemyTurn(enemy));


        //自分に付与されている状態の処理
        yield return StartCoroutine(Player.Instance.PlayerEffectBoot(enemy));

        //次のターンに向けてセットアップを行う
        SetupNextTurn();
        //ターンの始めに戻る
        nowTurn = StartCoroutine(turn());
    }

    //次ターンに向けてのリセットと準備
    void SetupNextTurn()
    {
        Debug.Log($"敵のLife：{enemy.Base.EnemyLife}");

        enemy.EnemyReSet();
        Player.Instance.SetupNext();
        
    
        gameUI.ResetUI();
        synthesisManager.SetButton();
        deck.DeckListOpen();
        SendCardTo();

        TurnCount += 1;

        cardSet = false;
    }

    void DecisionAction()
    {
        cardSet = true;
    }

    bool CardSet()
    {
        return cardSet;
    }

    private void Update()
    {
        if(enemy == null)
        {

        }
        else if (Player.Instance.Life <= 0 || enemy.Base.EnemyLife <= 0)
        {
            StopCoroutine(nowTurn);
            StartCoroutine(Result());
        }
    }

    IEnumerator Result()
    {
        if(Player.Instance.Life <= 0)
        {
            MessageText.TextIn("アナタは力尽きた");
        }
        yield return new WaitForSeconds(1f);
        gameUI.GameResult();
    }

}
