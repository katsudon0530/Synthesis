using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "EnemyAction/GolemAction")]
public class GolemAction : EnemyAction
{
    private List<int> Weights = new List<int>();
    public override IEnumerator Execute(Enemy enemy)
    {
        MessageText.TextIn($"{enemy.Base.Name}の攻撃");
        yield return new WaitForSeconds(1f);
        MessageText.TextIn(($"{enemy.Base.Name}のグレートパンチ！"));
        yield return new WaitForSeconds(1.2f);

        int Hit = EnemyAttack(enemy);
        MessageText.TextIn($"{Hit}ダメージをうけた");
        yield break;
    }

    public int EnemyAttack( Enemy enemy)
    {

        int Hit = (int)(enemy.Attack * Random.Range(0.8f, 1.1f));
        float Decrease = 1f - Player.Instance.Defens / 100f;

        if (enemy.Count == 0)
        {
            Hit = 2 * Hit;
        }
        Hit = (int)(Hit * Decrease);
        Player.Instance.Life -= Hit;

        return Hit;
    }
}
