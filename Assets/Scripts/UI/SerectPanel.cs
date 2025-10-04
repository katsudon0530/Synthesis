using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class SerectPanel : MonoBehaviour
{
    //セレクトパネルを非表示
    public void EnemyButton(int enemyID)
    {
        GameMaster.enemyNum = enemyID;
        string currentScene = SceneManager.GetActiveScene().name;
        if(currentScene != "GameScene")
            SceneManager.LoadScene("GameScene");
    }
}
