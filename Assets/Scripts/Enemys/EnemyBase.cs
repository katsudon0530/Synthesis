using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyBase : ScriptableObject
{
    [Header("エネミーの種類")]
    [SerializeField] Sprite icon;
    [SerializeField] string Name;
    [SerializeField] EnemyType type;
    [Space(10)]
    [Header("エネミーステータス")]
    [SerializeField] int enemyLifeMax;
    int enemyLife;
    [SerializeField] int enemyAttack;
    [SerializeField] int enemyDefense;
    [SerializeField] int enemyMagicDefense;
    [Space(10)]
    [Header("強攻撃")]
    [SerializeField] int enemyCount;
    int count;
    bool act;
    [TextArea]
    [SerializeField] string description;

    [SerializeField] List<parLifeEnemy> situation = new List<parLifeEnemy>();

    [Space(10)]
    List<StatusEffect> effects = new List<StatusEffect>();

    [SerializeField] EnemyAction enemyAction;
    [SerializeField] List<ActionList> actionLists;



    public EnemyType Type { get => type; set => type = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public string Description { get => description; set => description = value; }
    public string Name1 { get => Name; set => Name = value; }
    public int EnemyAttack { get => enemyAttack; set => enemyAttack = value; }
    public int EnemyLife { get => enemyLife; set => enemyLife = value; }
    public int EnemyLifeMax { get => enemyLifeMax; set => enemyLifeMax = value; }
    public int EnemyDefense { get => enemyDefense; set => enemyDefense = value; }
    public int EnemyMagicDefense { get => enemyMagicDefense; set => enemyMagicDefense = value; }
    public int EnemyCount { get => enemyCount; set => enemyCount = value; }
    public int Count { get => count; set => count = value; }
    public List<parLifeEnemy> Situation { get => situation; set => situation = value; }
    public EnemyAction EnemyAction { get => enemyAction; set => enemyAction = value; }
    public List<StatusEffect> Effects { get => effects; set => effects = value; }
    public bool Act { get => act; set => act = value; }
    public List<ActionList> ActionLists { get => actionLists; set => actionLists = value; }
}

public enum EnemyType
{
    Slime,
    Golem,
    Dragon,
}

[System.Serializable]
public class parLifeEnemy
{
    [Range(0, 1)]
    public float restLife;
    [TextArea]
    public string situationText;
}

[System.Serializable]
public class ActionList
{
    public string textMessage;
    [Range(0, 100)] public float percent;
    public EnemyAction enemyAction;
}