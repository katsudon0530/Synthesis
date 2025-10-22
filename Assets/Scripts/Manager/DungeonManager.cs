using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;
using static UnityEngine.UIElements.VisualElement;

public enum FlowState
{
    move,
    turn,
    result,
}
public class DungeonManager : MonoBehaviour
{
    [SerializeField] GameMaster gameMaster;
    [SerializeField] Dungeon dungeon;
    private Enemy enemy;

    public static FlowState flowState;
    public static event Action<FlowState> OnStateChanged;

    private void Awake()
    {
        if (gameMaster == null)
        {
            gameMaster = FindObjectOfType<GameMaster>();
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
        ;
        for (int i = 0; i < dungeon.Hierachies.Count; i++)
        {
            if (enemy != null)
                enemy.EnemyDestroy();
            DungeonMove(i);
            while (enemy.Life > 0)
            {
                yield return StartCoroutine(gameMaster.TurnStart());
            }
        }
    }


    private void DungeonMove(int hierachy)
    {
        int dungeonEnemy = dungeon.Hierachies[hierachy].EnemyProbabilities[0].enemyBase.EnemyID;
        enemy = gameMaster.EnemySet(dungeonEnemy);
    }
    

    public static void ChangeState(FlowState newState)
    {
        if (flowState != newState)
        {
            //Debug.Log($"[GameState] {turnState} → {newState}");
            flowState = newState;
            OnStateChanged?.Invoke(newState);
        }
    }
}
