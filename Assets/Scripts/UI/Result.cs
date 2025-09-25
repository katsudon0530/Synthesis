using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;
    [SerializeField] Text TurnResultText;

    //リザルトパネルを非表示化
    public void OffResultPanel()
    {
        resultPanel.SetActive(false);
    }

    //ゲームの結果を表示する
    public void ShowResult(int playerLife)
    {
        if (playerLife <= 0)
            ShowGameResult("LOSE");

        else if (playerLife >= 0)
            ShowGameResult("WIN");
    }

    //ゲームの勝敗をパネルで表示
    public void ShowGameResult(string result)
    {
        resultPanel.SetActive(true);
        //resultPanel.GetComponentInChildren<Text>().text = result ;
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
}
