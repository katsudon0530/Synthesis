using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "EnemyAction/GolemAction")]
public class GolemAction : EnemyAction
{
    [SerializeField, Header("確率"), Range(0, 100)]
    private List<int> Weights = new List<int>();
    public override IEnumerator Execute(Player player, Enemy enemy)
    {
        MessageText.message.text = $"{enemy.Base.Name1}の攻撃";
        yield return new WaitForSeconds(1f);
        MessageText.message.text = ($"{enemy.Base.Name1}のグレートパンチ！");
        yield return new WaitForSeconds(1.2f);

        int Hit = EnemyAttack(player, enemy);
        MessageText.message.text = $"{Hit}ダメージをうけた";
        yield break;
    }

    public int EnemyAttack(Player player, Enemy enemy)
    {

        int Hit = (int)(enemy.Base.EnemyAttack * Random.Range(0.8f, 1.1f));
        float Decrease = 1f - player.Defens / 100f;

        if (enemy.Base.Count == 0)
        {
            Hit = 2 * Hit;
        }
        Hit = (int)(Hit * Decrease);
        player.Life -= Hit;

        return Hit;
    }
}
