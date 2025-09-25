using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] Text playerLifeText;
    [SerializeField] Text TurnText;
    [SerializeField] GameObject deckPanel;
    [SerializeField] GameObject submitButton;
    [SerializeField] GameObject synthesisButton;

    [SerializeField] GameObject massagePanel;
    [SerializeField] GameObject serectPanel;
    [SerializeField] GameObject cardGuide;
    [SerializeField] RulePanelUI rulePanelUI;
    [SerializeField] Result result;

    public UnityAction OnDecisionButton;
    private int buttonID;
    Player player;
    Enemy enemy;

    public int ButtonID { get => buttonID; }


    //UIの非表示化
    public void UISetUp()
    {
        massagePanel.SetActive(false);
        deckPanel.SetActive(false);
        result.OffResultPanel();
        rulePanelUI.SetRuleText();
        player = Player.Instance;
    }

    //ライフの表示・変更
    //経過ターン数をカウントする
    private void Update()
    {
        if(player != null)
        {
            if (player.Life <= 0)
            {
                player.Life = 0;
            }
            playerLifeText.text = $"{player.Life}HP";
        }

        TurnText.text = $"ターン {GameMaster.TurnCount}";
    }

    //ゲームの結果を表示する
    public void GameResult()
    {
        result.ShowResult(player.Life);
    }


    //ルール説明の遷移とボタン  
    public void RuleTextAZ(string ProceedReturn)
    {
        rulePanelUI.RuleTextChange(ProceedReturn);
    }


    //決定ボタン入力時に行うアクション
    public void OnDecision(int ID)
    {
        buttonID = ID;
        submitButton.SetActive(false);
        synthesisButton.SetActive(false);
        OnDecisionButton?.Invoke();
    }

    //ボタン初期化を行う
    public void ResetUI()
    {
        submitButton.SetActive(true);
        synthesisButton.SetActive(true);
        massagePanel.SetActive(false);
        cardGuide.SetActive(true);
    }


    //メッセージパネルを表示・非表示
    public void OnMassegePanel()
    {
        massagePanel.GetComponentInChildren<Text>().text = "";
        massagePanel.SetActive(true);
    }
    public void OffMassegePanel()
    {
        massagePanel.SetActive(false);
    }

    //セレクトパネルを非表示
    public void StartGameUI(Enemy enemyIn)
    {
        serectPanel.SetActive(false);
        enemy = enemyIn;
    }
    
    //カードガイドの非表示
    public void OffCardGuide()
    {
        cardGuide.SetActive(false);
    }
}
