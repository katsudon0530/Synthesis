using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "EnemyAction/SlimeAction")]
public class SlimeAction : EnemyAction
{
    [SerializeField, Header("確率"), Range(0, 100)]
    private List<int> Weights = new List<int>();
    public override IEnumerator Execute(Enemy enemy)
    {
        MessageText.TextIn($"{enemy.Base.Name1}の攻撃");
        yield return new WaitForSeconds(1f);
        MessageText.TextIn(($"{enemy.Base.Name1}の溶解液！"));
        yield return new WaitForSeconds(1.2f);

        int Hit = EnemyAttack(enemy);
        MessageText.TextIn($"{Hit}ダメージをうけた");
        yield break;
    }

    public int EnemyAttack(Enemy enemy)
    {

        int Hit = (int)(enemy.Base.EnemyAttack * Random.Range(0.8f, 1.1f));
        float Decrease = 1f - Player.Instance.Defens / 100f;

        if (enemy.Base.Count == 0)
        {
            Hit = 2 * Hit;
        }
        Hit = (int)(Hit * Decrease);
        Player.Instance.Life -= Hit;

        return Hit;
    }
}
