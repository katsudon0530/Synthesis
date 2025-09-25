using UnityEngine;

[CreateAssetMenu]

public class StatusEffectBase : ScriptableObject
{
    [Header("種類・名前")]
    [SerializeField] Sprite icon;
    [SerializeField] string displayName;
    [SerializeField] string effectName;
    [SerializeField] StatusEffectType type;

    [Space(10)]
    [Header("持続ターン")]
    [SerializeField] int effectCount;
    [TextArea]
    [SerializeField] string description;

    [SerializeField] EffectDetails effectDetails;

    public StatusEffectType Type { get => type; set => type = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public string Description { get => description; set => description = value; }
    public int EffectCount { get => effectCount; set => effectCount = value; }
    public EffectDetails EffectDetails { get => effectDetails; set => effectDetails = value; }
    public string EffectName { get => effectName; set => effectName = value; }
    public string DisplayName { get => displayName; set => displayName = value; }
}

public enum StatusEffectType
{
    Buff,
    DeBuff,
}
