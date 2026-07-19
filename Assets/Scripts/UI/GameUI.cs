using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject submitButton;
    [SerializeField] GameObject synthesisButton;
    [SerializeField] TMP_Text RestText;

    private void Awake()
    {
        GameMaster.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(TurnState state)
    {
        switch (state)
        {
            case TurnState.battle:
            case TurnState.synthesis:
                BattleUI();
                break;
            case TurnState.cardSet:
                ResetUI();
                break;

            case TurnState.end:
                RestDeck();
                break;
        }
    }

    //ボタン初期化を行う
    public void ResetUI()
    {
        submitButton.SetActive(true);
        synthesisButton.SetActive(true);
        MessageText.Panel(false);
    }

    //バトル中のボタンを消す
    public void BattleUI()
    {
        submitButton.SetActive(false);
        synthesisButton.SetActive(false);
    }


    //デッキの残り枚数を表示する
    public void RestDeck()
    {
        RestText.text = $"残り{Deck.Instance.cardDeck.Count}枚";
    }

    private void OnDestroy()
    {
        GameMaster.OnStateChanged -= OnStateChanged;
    }
}
