using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCount : MonoBehaviour
{
    [SerializeField] Text TurnText;

    void Update()
    {
        TurnText.text = $"ターン {GameMaster.TurnCount}";
    }
}
