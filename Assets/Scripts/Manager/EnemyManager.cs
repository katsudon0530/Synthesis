using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyGenerator enemyGenerator;
    [SerializeField] GameObject enemyFiled;

    //エネミーを生成してフィールドにセット
    public Enemy GenerateEnemy(int enemyNum)
    {
        Enemy enemy = enemyGenerator.SpawnEnemy(enemyNum);
        enemy.transform.SetParent(enemyFiled.transform);
        enemy.transform.localPosition = new Vector3(0, 0, 0);
        return enemy;
    }


    //エネミーの行動
    public IEnumerator EnemyTurn(Enemy enemy)
    {
        if (enemy.Life == 0)
        {
            enemy.EnemyDestroy();
            yield break;
        }

        MessageText.Panel(true);

        if (enemy.Act == false)
        {
            MessageText.TextIn($"{enemy.Base.Name}は動けない！");
        }
        else
        {
            //敵の攻撃
            List<ActionList> list = enemy.Base.ActionLists;
            float num = Random.Range(0f, 100f);
            float per = 0;
            for (int i = 0;i < list.Count; i++)
            {
                per += list[i].percent;
                if (per > num)
                {
                    yield return StartCoroutine(list[i].enemyAction.Execute(enemy));
                    break;
                }
            } 
        }

        yield return new WaitForSeconds(1.5f);
        enemy.EnemyCountDown();
        MessageText.Panel(false);
    }



    public IEnumerator EnemyEffectBoot(Enemy enemy)
    {
        if (enemy.Base.Effects.Count == 0 || enemy.Life == 0)
        {
            yield break;
        }

        MessageText.Panel(true);
        
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
