using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject submitButton;
    [SerializeField] GameObject synthesisButton;
    [SerializeField] TMP_Text RestText;
    [SerializeField] TMP_Text synthesisCount;

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
            case TurnState.notSet:
            case TurnState.cardSet:
                RestSynthesis();
                ResetUI();
                break;
            case TurnState.start:
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

    //合成の残り回数を表示する
    public void RestSynthesis()
    {
        synthesisCount.text = $"あと{GameData.Instance.synthesisCount}回";
    }

    private void OnDestroy()
    {
        GameMaster.OnStateChanged -= OnStateChanged;
    }
}
