using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyBase : ScriptableObject
{
    [Header("エネミーの種類")]
    [SerializeField] int enemyID;
    [SerializeField] Sprite icon;
    [SerializeField] string enemyName;
    [SerializeField] EnemyType type;
    [Space(10)]
    [Header("エネミーステータス")]
    [SerializeField] int enemyLifeMax;
    [SerializeField] int baseAttack;
    [SerializeField] int baseDefense;
    [SerializeField] int baseMagicDefense;
    [Space(10)]
    [Header("強攻撃")]
    [SerializeField] int enemyCount;
    [TextArea]
    [SerializeField] string description;

    [SerializeField] List<parLifeEnemy> situation = new List<parLifeEnemy>();

    [Space(10)]
    List<StatusEffect> effects = new List<StatusEffect>();

    [SerializeField] List<ActionList> actionLists;

    public int EnemyID { get => enemyID; set => enemyID = value; }
    public EnemyType Type { get => type; set => type = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public string Description { get => description; set => description = value; }
    public string Name { get => enemyName; set => enemyName = value; }
    public int EnemyAttack { get => baseAttack; set => baseAttack = value; }
    public int EnemyLifeMax { get => enemyLifeMax; set => enemyLifeMax = value; }
    public int EnemyDefense { get => baseDefense; set => baseDefense = value; }
    public int EnemyMagicDefense { get => baseMagicDefense; set => baseMagicDefense = value; }
    public int EnemyCount { get => enemyCount; set => enemyCount = value; }
    public List<parLifeEnemy> Situation { get => situation; set => situation = value; }
    public List<StatusEffect> Effects { get => effects; set => effects = value; }
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