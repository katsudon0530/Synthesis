using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] TMP_Text playerLifeText;
    private Player player;

    private void Start()
    {
        player = Player.Instance;
    }

    private void Update()
    {
        if (Player.Instance == null) return;
        if (player.Life <= 0)
        {
            player.Life = 0;
        }
        playerLifeText.text = $"{player.Life}HP";
    }


}
