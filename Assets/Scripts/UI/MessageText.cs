using UnityEngine;
using UnityEngine.UI;

public class MessageText : MonoBehaviour
{
    public static Text message;

    public void Awake()
    {
        message = GetComponent<Text>();
    }
}
