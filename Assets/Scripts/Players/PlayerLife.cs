using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] Text playerLifeText;


    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.Life <= 0)
        {
            Player.Instance.Life = 0;
        }
        playerLifeText.text = $"{Player.Instance.Life}HP";
    }
}
