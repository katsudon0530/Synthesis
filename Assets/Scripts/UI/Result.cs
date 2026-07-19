using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Result : MonoBehaviour
{
    [SerializeField] TMP_Text resultText;
    [SerializeField] TMP_Text TurnResultText;

    //リザルトパネルを非表示化
    public void Awake()
    {
        gameObject.SetActive(false);
        GameMaster.OnGameOver += ShowResult;
    }

    //ゲームの結果を表示する
    public void ShowResult()
    {
        gameObject.SetActive(true);
        if (Player.Instance.Life <= 0)
            ShowGameResult("LOSE");

        else if (Player.Instance.Life >= 0)
            ShowGameResult("WIN");
    }

    //ゲームの勝敗をパネルで表示
    public void ShowGameResult(string result)
    {
        gameObject.SetActive(true);
        resultText.text = result;
        if (result == "WIN")
        {
            TurnResultText.gameObject.SetActive(true);
            TurnResultText.text = $"経過ターン：{GameData.Instance.gameTurn}";
        }
        else
        {
            TurnResultText.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameMaster.OnGameOver -= ShowResult;
    }
}
