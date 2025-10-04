using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] Text resultText;
    [SerializeField] Text TurnResultText;

    //リザルトパネルを非表示化
    public void Awake()
    {
        gameObject.SetActive(false);
    }

    //ゲームの結果を表示する
    public void ShowResult()
    {
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
            TurnResultText.text = $"経過ターン：{GameMaster.TurnCount}";
        }
        else
        {
            TurnResultText.gameObject.SetActive(false);
        }
    }

    //タイトルに戻る
    public void OnTitleButton()
    {
        SceneManager.LoadScene("Title");
    }

    //シーンをリセットする
    public void ResetButton()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
