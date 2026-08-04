using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameData:Singleton<GameData>
{
    protected override bool IsPersistent => true;

    public int serectEnemyID {  get; set; }
    public int gameTurn { get; set; }
    public int dungeonFroor { get; set; }
    public int synthesisCount { get; set; }
    public List<int> gameDeck { get; set; }

}
