using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] Dungeon dungeon;

    private void DungeonMove(int hierachy)
    {
        int dungeonEnemy = dungeon.Hierachies[hierachy].EnemyProbabilities[0].enemyBase.EnemyID;
        GameMaster.enemyNum = dungeonEnemy;
    }
}
