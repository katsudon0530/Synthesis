using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] List<EnemyBase> enemyBases;

    public Enemy SpawnEnemy(int ID)
    {
        Enemy enemy = Instantiate(enemyPrefab);
        EnemyBase spawnEnemy = enemyBases.Find(x => x.EnemyID == ID);
        enemy.SetEnemy(spawnEnemy);
        return enemy;
    }
}
