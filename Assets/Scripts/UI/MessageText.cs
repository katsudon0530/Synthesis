using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MessageText : MonoBehaviour
{
    public static Text message;

    public void Awake()
    {
        message = GetComponent<Text>();
    }

    public static void TextIn(string nowText)
    {
        message.text = nowText;   
    }
    public static void ReSet()
    {
        message.text = "";
    }
    public static void Set(bool aa)
    {
    }
}