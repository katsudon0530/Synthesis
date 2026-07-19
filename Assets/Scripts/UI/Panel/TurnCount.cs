using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnCount : MonoBehaviour
{
    [SerializeField] TMP_Text TurnText;

    void Update()
    {
        TurnText.text = $"ターン {GameData.Instance.gameTurn}";
    }
}
