using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject submitButton;
    [SerializeField] GameObject synthesisButton;

    [SerializeField] Result result;

    public UnityAction OnDecisionButton;

    public int ButtonID { get; set; }

    //ゲームの結果を表示する
    public void GameResult()
    {
        result.ShowResult();
    }

    //決定ボタン入力時に行うアクション
    public void OnDecision(int ID)
    {
        ButtonID = ID;
        submitButton.SetActive(false);
        synthesisButton.SetActive(false);
        OnDecisionButton?.Invoke();
    }

    //ボタン初期化を行う
    public void ResetUI()
    {
        submitButton.SetActive(true);
        synthesisButton.SetActive(true);
        MessageText.Panel(false);
    }
}
