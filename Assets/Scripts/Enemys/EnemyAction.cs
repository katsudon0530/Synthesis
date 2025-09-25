using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyAction : ScriptableObject
{
    public abstract IEnumerator Execute(Enemy enemy);
}
