using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulePanelUI : MonoBehaviour
{
    [SerializeField] GameObject rulePanel;
    [SerializeField] Text ruleText;
    [TextArea]
    [SerializeField] List<string> ruleAllText;
    [SerializeField] GameObject proceedButton;
    [SerializeField] GameObject returnButton;

    int textCount;

    public void SetRuleText()
    {
        rulePanel.SetActive(false);
        returnButton.SetActive(false);
        textCount = 0;
        ruleText.text = ruleAllText[textCount];
    }

    //ルール説明の遷移とボタン  
    public void RuleTextChange(string around)
    {
        
        if (around == "Proceed")
            textCount++;
        else if (around == "Return")
            textCount--;

        ruleText.text = ruleAllText[textCount];

        if (textCount == 0)
        {
            returnButton.SetActive(false);
        }

        else if (textCount + 1 == ruleAllText.Count)
        {
            proceedButton.SetActive(false);
        }

        else
        {
            returnButton.SetActive(true);
            proceedButton.SetActive(true);
        }

    }
}
