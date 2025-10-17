using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject submitButton;
    [SerializeField] GameObject synthesisButton;
    [SerializeField] Text RestText;

    private void Awake()
    {
        GameMaster.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(TurnState state)
    {
        switch (state)
        {
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


    //デッキの残り枚数を表示する
    public void RestDeck()
    {
        RestText.text = $"残り{Deck.Instance.cardDeck.Count}枚";
    }
}
