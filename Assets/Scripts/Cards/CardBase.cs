using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] string displayName;
    [SerializeField] string cardName;
    [SerializeField] SynthesisType synthesisType;
    [SerializeField] CardType type;
    [SerializeField] int id;
    [SerializeField] Sprite icon;
    [SerializeField] Color color;
    [SerializeField] bool playCondition;
    [TextArea]
    [SerializeField] string description;
    [TextArea]
    [SerializeField] string explanation;
    [SerializeField] CardStatus cardStatus;

    [SerializeField] UniqueEffect uniqueEffect;
    [SerializeField] List<FlontBuff> flontBuff = new List<FlontBuff>();


    public CardType Type { get => type; set => type = value; }
    public int ID { get => id; set => id = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public string Description { get => description; set => description = value; }
    public string DisplayName { get => displayName; set => displayName = value; }
    public string CardName { get => cardName; set => cardName = value; }
    public string Explanation { get => explanation; set => explanation = value; }
    public Color Color { get => color; set => color = value; }
    public SynthesisType SynthesisType { get => synthesisType; set => synthesisType = value; }
    public UniqueEffect UniqueEffect { get => uniqueEffect; set => uniqueEffect = value; }
    public List<FlontBuff> FlontBuff { get => flontBuff; set => flontBuff = value; }
    public CardStatus CardStatus { get => cardStatus; set => cardStatus = value; }
    public bool PlayCondition { get => playCondition; set => playCondition = value; }
}

public enum CardType
{
    Attack,
    Defense,
    Heal,
    exception,
}

public enum SynthesisType
{
    Normal,
    Plus,
    DoublePlus,
}

[Serializable]
public class CardStatus
{
    [SerializeField] float attack_Status;
    [SerializeField] float magic_Status;
    [SerializeField] float defense_Status;
    [SerializeField] float heal_Status;

    public float Attack_Status { get => attack_Status; set => attack_Status = value; }
    public float Magic_Status { get => magic_Status; set => magic_Status = value; }
    public float Defense_Status { get => defense_Status; set => defense_Status = value; }
    public float Heal_Status { get => heal_Status; set => heal_Status = value; }
}


[System.Serializable]
public class FlontBuff
{
    public float buff;
    public string flontCard ;
}



