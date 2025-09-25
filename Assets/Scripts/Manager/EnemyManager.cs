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
    public IEnumerator EnemyTurn(Enemy enemy,GameUI gameUI, int TurnCount)
    {
        gameUI.OnMassegePanel();

        if(enemy.Base.Act == false)
        {
            MessageText.TextIn($"{enemy.Base.Name1}は動けない！");
        }
        else
        {
            //敵の攻撃
            yield return StartCoroutine(enemy.Base.EnemyAction.Execute(enemy));
        }

        yield return new WaitForSeconds(1.5f);
        enemy.EnemyCountDown();
        gameUI.OffMassegePanel();
    }



    public IEnumerator EnemyEffectBoot(Enemy enemy, GameUI gameUI)
    {
        gameUI.OnMassegePanel();


        if (enemy.Base.Effects.Count == 0)
        {
            Debug.Log("何もない");
            yield break;
        }
        
        foreach (StatusEffect effect in enemy.Base.Effects)
        {
            if(effect != null)
            {
                yield return StartCoroutine(effect.Base.EffectDetails.Execute(effect, enemy));
            }
        }
        gameUI.OffMassegePanel();
        yield return new WaitForSeconds(1.0f);

        yield break;
    }
}
