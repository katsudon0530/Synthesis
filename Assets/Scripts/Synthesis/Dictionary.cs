using System.Collections.Generic;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    [SerializeField] List<IdDataDouble> synthesisDouble = new List<IdDataDouble>();
    [SerializeField] List<IdDataTriple> synthesisTriple = new List<IdDataTriple>();

    public List<IdDataDouble> SynthesisDouble { get => synthesisDouble; set => synthesisDouble = value; }
    public List<IdDataTriple> SynthesisTriple { get => synthesisTriple; set => synthesisTriple = value; }

}

[System.Serializable]
public class IdDataDouble
{
    [Header("Card_1 + Card_2 = SynthesisCard_+")]
    public int card_1_ID;
    public int card_2_ID;
    public int SynthesisCard;
}

[System.Serializable]
public class IdDataTriple
{
    [Header("Card_1 + Card_2 + Card_3 = SynthesisCard_++")]
    public int card_1_ID;
    public int card_2_ID;
    public int card_3_ID;
    public int SynthesisCard;
}
