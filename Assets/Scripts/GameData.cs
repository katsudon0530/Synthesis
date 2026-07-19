using UnityEngine;

public class GameData:Singleton<GameData>
{
    protected override bool IsPersistent => true;

    public int serectEnemyID {  get; set; }
    public int gameTurn { get; set; }
    public int dungeonFroor { get; set; }

}
