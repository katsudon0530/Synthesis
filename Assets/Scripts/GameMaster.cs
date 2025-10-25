using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState 
{
    start,
    cardSet,
    battle,
    synthesis,
    enemy,
    end
}

public class GameMaster : MonoBehaviour
{
    [SerializeField] int handMax;

    private EnemyManager enemyManager;
    private CardManager cardManager;

    private Deck deck;
    private Enemy enemy;
    public static Coroutine nowTurn;
    public static Coroutine segmentTurn;

    public static event Action OnGameOver;
    public static event Action<TurnState> OnStateChanged;
    public static TurnState turnState { get; private set; }

    public static int TurnCount;
    public static int enemyNum = 101;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        cardManager = GetComponent<CardManager>();
        deck = Deck.Instance;
        Set();
    }

    //ゲームスタート時のセットアップ内容
    public void Set()
    {
        TurnCount = 1;
        deck.DeckSet();
        Player.Instance.SetPlayer(handMax);

        //TurnStart();
    }

    public Enemy EnemySet(int num)
    {
        enemy = enemyManager.GenerateEnemy(num);
        return enemy;
    }
    //ターンを開始する
    public IEnumerator TurnStart()
    {
        yield return nowTurn = StartCoroutine(turn());
    }

    //手札を生成
    private void SetHand()
    {
        int cardsum;
        int deckCount = deck.cardDeck.Count;
        int handCount = Field.Instance.Hand.Count;

        if (deckCount == 0 && handCount == 0)
        {
            deck.cardDeck = new List<int>(deck.DeckAll);
            deckCount = deck.cardDeck.Count;
        }

        cardsum = handMax - handCount;
        if (cardsum > deckCount)
            cardsum = deckCount;

        for (int i = 0; i < cardsum; i++)
        {
            deck.Draw();
        }
    }

    public IEnumerator turn()
    {
        ChangeState(TurnState.start);
        SetHand();
        ChangeState(TurnState.cardSet);
        while (turnState == TurnState.cardSet)
        {
            Player.Instance.PlayConditionCheck(enemy, deck); 
            yield return null; 
        }

        //カードが選択され、決定か合成が押されるまで待機
        yield return new WaitUntil(() => turnState != TurnState.cardSet);

        Field.Instance.PlayerHand.SetActive(false);

        //どちらのボタンが押されたのかを判別してそのアクションを実行
        switch (turnState)
        {
            case TurnState.battle:
                //決定ボタンが押された場合
                yield return segmentTurn = StartCoroutine(cardManager.CardBattle(enemy));
                break;
            case TurnState.synthesis:
                //合成ボタンが押された場合
                yield return segmentTurn = StartCoroutine(cardManager.CardSynthesis());
                break;
        }
        //敵に付与されている状態の処理
        yield return segmentTurn = StartCoroutine(enemyManager.EnemyEffectBoot(enemy));

        //エネミーの行動を行う
        yield return segmentTurn = StartCoroutine(enemyManager.EnemyTurn(enemy));

        //自分に付与されている状態の処理
        yield return segmentTurn = StartCoroutine(Player.Instance.PlayerEffectBoot(enemy));

        //次のターンに向けてセットアップを行う
        SetupNextTurn();
    }

    //次ターンに向けてのリセットと準備
    void SetupNextTurn()
    {
        Debug.Log($"敵のLife：{enemy.Life}");

        TurnCount += 1;
        if(enemy != null)
            enemy.EnemyReSet();
        ChangeState(TurnState.end);
    }

    public static void ChangeState(TurnState newState)
    {
        if (turnState != newState)
        {
            //Debug.Log($"[GameState] {turnState} → {newState}");
            turnState = newState;
            OnStateChanged?.Invoke(newState);
        }
    }

    public IEnumerator ResultTurn()
    {
        if (Player.Instance.Life <= 0)
        {
            MessageText.TextIn("アナタは力尽きた");
        }
        Field.Instance.PlayerHand.SetActive(false);
        Field.Instance.BattleField.SetActive(false);
        yield return new WaitForSeconds(1f);
        OnGameOver?.Invoke();
    }
}
