using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon")]
public class Dungeon : ScriptableObject
{
    [SerializeField] List<Hierachy> hierachies = new List<Hierachy>();

    public List<Hierachy> Hierachies { get => hierachies; set => hierachies = value; }
}

