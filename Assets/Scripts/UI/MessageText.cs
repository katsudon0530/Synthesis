using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MessageText : MonoBehaviour
{
    static Text message;
    static GameObject messagePanel;

    public void Awake()
    {
        message = GetComponentInChildren<Text>();
        if(message == null)
        {
            Debug.LogError("Textが入っていません");
        }
        messagePanel = this.gameObject;
        Panel(false);
    }

    public static void TextIn(string nowText)
    {
        message.text = nowText;   
    }
    public static void ReSet()
    {
        message.text = "";
    }

    public static void Panel(bool condition)
    {
        ReSet();
        messagePanel.SetActive(condition);
    }
}
