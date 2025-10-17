using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.CullingGroup;

public enum TurnState { start, cardSet, battle, synthesis, enemy, end }
public class GameMaster : MonoBehaviour
{
    [SerializeField] int handMax;

    private EnemyManager enemyManager;
    private CardManager cardManager;

    private Deck deck;
    private Enemy enemy;
    private Coroutine nowTurn;

    public static event Action OnGameOver;
    public static event Action<TurnState> OnStateChanged;
    public static TurnState turnState { get; set; }

    public static int TurnCount;
    public static int enemyNum;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        cardManager = GetComponent<CardManager>();
        deck = Deck.Instance;
    }

    //ゲームスタート時のセットアップ内容
    public void Start()
    {
        Player.Instance.SetPlayer(handMax);
        enemy = enemyManager.GenerateEnemy(enemyNum);
        deck.DeckSet();

        turnState = TurnState.cardSet;
        TurnCount = 1;

        nowTurn = StartCoroutine(turn());
    }

    //手札を生成
    private void SetHand()
    {
        int cardsum;
        int deckCount = deck.cardDeck.Count;
        int handCount = Field.Instance.Hand.Count;

        if (deckCount == 0 && handCount == 0)
            deck.cardDeck = new List<int>(deck.DeckAll);

        cardsum = handMax - handCount;
        if (cardsum > deckCount)
            cardsum = deckCount;

        for (int i = 0; i < cardsum; i++)
        {
            deck.Draw();
        }
    }

    IEnumerator turn()
    {
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
                yield return StartCoroutine(cardManager.CardBattle(enemy));
                break;
            case TurnState.synthesis:
                //合成ボタンが押された場合
                yield return StartCoroutine(cardManager.CardSynthesis());
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

        TurnCount += 1;
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

    private void Update()
    {
        if(Player.Instance == null || enemy == null)
        {

        }
        else if (Player.Instance.Life <= 0 || enemy.Base.EnemyLife <= 0)
        {
            if (nowTurn != null)
                StopCoroutine(nowTurn);
            StartCoroutine(ResultTurn());
        }
    }

    IEnumerator ResultTurn()
    {
        if(Player.Instance.Life <= 0)
        {
            MessageText.TextIn("アナタは力尽きた");
        }
        Field.Instance.PlayerHand.SetActive(false);
        Field.Instance.BattleField.SetActive(false);
        yield return new WaitForSeconds(1f);
        OnGameOver?.Invoke();
    }
}
