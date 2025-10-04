using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class SerectPanel : MonoBehaviour
{
    //セレクトパネルを非表示
    public void EnemyButton(int enemyID)
    {
        GameMaster.enemyNum = enemyID;
        gameObject.SetActive(false);
    }
}
