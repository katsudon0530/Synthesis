using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;
using static UnityEngine.UIElements.VisualElement;

public class SimpleModeManager : MonoBehaviour
{
    [SerializeField] GameMaster gameMaster;

    private Enemy enemy;

    private void Awake()
    {
        if (gameMaster == null)
        {
            gameMaster = FindAnyObjectByType<GameMaster>();
            if (gameMaster == null)
                Debug.LogWarning("シーン上にgameMasterが存在していません");
        }
    }

    private void Start()
    {
        StartCoroutine(DungeonMode());
    }

    public IEnumerator DungeonMode()
    {
        if(GameData.Instance.serectEnemyID == 0)
        {
            GameData.Instance.serectEnemyID = 101;
        }
        
        enemy = gameMaster.EnemySet(GameData.Instance.serectEnemyID);

        while (enemy != null)
        {
            yield return StartCoroutine(gameMaster.TurnStart());
        }

        yield return StartCoroutine(gameMaster.ResultTurn());
    }

}
