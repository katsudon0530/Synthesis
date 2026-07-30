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
    end,
    notSet,
}

public class GameMaster : MonoBehaviour
{
    [SerializeField] int handMax;
    [SerializeField] int synthesisCount;
    [SerializeField] BattleLog battleLog;

    private EnemyManager enemyManager;
    private CardManager cardManager;

    private Deck deck => Deck.Instance;
    private Player player => Player.Instance;
    private Field field => Field.Instance;

    public Coroutine nowTurn;
    public Coroutine segmentTurn;

    public static event Action OnGameOver;
    public static event Action<TurnState> OnStateChanged;
    public static TurnState turnState { get; private set; }

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyManager.Initialize(battleLog);
        cardManager = GetComponent<CardManager>();
        cardManager.Initialize(battleLog);
        Set();
    }

    //ゲームスタート時のセットアップ内容
    public void Set()
    {
        GameData.Instance.gameTurn = 1;
        GameData.Instance.synthesisCount = synthesisCount;
        deck.DeckSet();
        player.SetPlayer();
        field.SettingHand(handMax);
        turnState = TurnState.notSet;
    }

    public Enemy EnemySet(int id)
    {
        return enemyManager.GenerateEnemy(id);
    }

    //ターンを開始する
    public IEnumerator TurnStart()
    {
        yield return nowTurn = StartCoroutine(turn());
    }

    //手札を生成
    private void SetHand()
    {
        int deckCount = deck.cardDeck.Count;
        int handCount = field.Hand.Count;

        if (deckCount == 0 && handCount == 0)
        {
            deck.cardDeck = new List<int>(deck.DeckAll);
            deckCount = deck.cardDeck.Count;
        }

        int cardsum = handMax - handCount;
        if (cardsum > deckCount)
            cardsum = deckCount;

        for (int i = 0; i < cardsum; i++)
        {
            var card = deck.Draw();
            field.SerCardToHand(card);
        }
    }

    public IEnumerator turn()
    {
        SetHand();
        ChangeState(TurnState.start);

        while (true)
        {
            ChangeState(TurnState.cardSet);
            while (turnState == TurnState.cardSet)
            {
                player.PlayConditionCheck(enemyManager.currentEnemy, field.Hand);
                yield return null;
            }

            if(turnState == TurnState.synthesis)
            {
                yield return segmentTurn = StartCoroutine(cardManager.CardSynthesis());
                continue;
            }

            //決定ボタンが押された場合
            if (turnState == TurnState.battle)
            {
                yield return segmentTurn = StartCoroutine(cardManager.CardBattle(enemyManager.currentEnemy));
                break;
            }

        }

        //敵に付与されている状態の処理
        yield return segmentTurn = StartCoroutine(enemyManager.EnemyEffectBoot());

        //エネミーの行動を行う
        yield return segmentTurn = StartCoroutine(enemyManager.EnemyTurn());

        //自分に付与されている状態の処理
        yield return segmentTurn = StartCoroutine(player.PlayerEffectBoot(enemyManager.currentEnemy));

        //次のターンに向けてセットアップを行う
        SetupNextTurn();
    }

    //次ターンに向けてのリセットと準備
    void SetupNextTurn()
    {
        GameData.Instance.gameTurn += 1;

        enemyManager.EndSet();
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
        if (player.Life <= 0)
        {
            battleLog.SendMessage("アナタは力尽きた");
        }
        yield return new WaitForSeconds(1f);
        OnGameOver?.Invoke();
    }
}
