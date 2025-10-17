using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyGenerator enemyGenerator;
    [SerializeField] EnemyFiled enemyFiled;

    //エネミーを生成してフィールドにセット
    public Enemy GenerateEnemy(int enemyNum)
    {
        Enemy enemy = enemyGenerator.SpawnEnemy(enemyNum);
        enemyFiled.AddEnemy(enemy);
        return enemy;
    }


    //エネミーの行動
    public IEnumerator EnemyTurn(Enemy enemy)
    {
        MessageText.Panel(true);

        if (enemy.Base.Act == false)
        {
            MessageText.TextIn($"{enemy.Base.Name1}は動けない！");
        }
        else
        {
            //敵の攻撃
            int count = enemy.Base.ActionLists.Count;
            yield return StartCoroutine(enemy.Base.EnemyAction.Execute(enemy));
        }

        yield return new WaitForSeconds(1.5f);
        enemy.EnemyCountDown();
        MessageText.Panel(false);
    }



    public IEnumerator EnemyEffectBoot(Enemy enemy)
    {
        MessageText.Panel(true);

        if (enemy.Base.Effects.Count == 0)
        {
            yield break;
        }
        
        foreach (StatusEffect effect in enemy.Base.Effects)
        {
            if(effect != null)
            {
                yield return StartCoroutine(effect.Base.EffectDetails.Execute(effect, enemy));
            }
        }
        MessageText.Panel(false);
        yield return new WaitForSeconds(1.0f);

        yield break;
    }
}
