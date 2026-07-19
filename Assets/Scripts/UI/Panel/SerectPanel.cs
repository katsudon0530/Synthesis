using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class SerectPanel : MonoBehaviour
{
    //セレクトパネルを非表示
    public void DungeonButton()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if(currentScene != "GameScene")
            SceneManager.LoadScene("GameScene");
    }
    public void EnemyButton(int enemyID)
    {
        GameData.Instance.serectEnemyID = enemyID;
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "EnemyScene")
            SceneManager.LoadScene("EnemyScene");
    }
}
