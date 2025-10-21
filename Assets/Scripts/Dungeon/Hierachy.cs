using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Hierachy")]
public class Hierachy : ScriptableObject
{
    [SerializeField] List<EnemyProbability> enemyProbabilities = new List<EnemyProbability>();

    public List<EnemyProbability> EnemyProbabilities { get => enemyProbabilities; set => enemyProbabilities = value; }
}

[System.Serializable]
public class EnemyProbability
{
    [Range(0, 100)] public float percent;
    public EnemyBase　enemyBase;
}