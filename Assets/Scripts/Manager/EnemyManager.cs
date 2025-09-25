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
    public IEnumerator EnemyTurn(Player player, Enemy enemy,GameUI gameUI, int TurnCount)
    {
        gameUI.OnMassegePanel();

        if(enemy.Base.Act == false)
        {
            MessageText.message.text = $"{enemy.Base.Name1}は動けない！";
        }
        else
        {
            //敵の攻撃
            yield return StartCoroutine(enemy.Base.EnemyAction.Execute(player, enemy));
            if (gameUI.Result(TurnCount))
                yield break;
        }

        yield return new WaitForSeconds(1.5f);
        enemy.EnemyCountDown();
        gameUI.OffMassegePanel();
    }



    public IEnumerator EnemyEffectBoot(Player player, Enemy enemy, GameUI gameUI)
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
                yield return StartCoroutine(effect.Base.EffectDetails.Execute(effect,player, enemy));
            }
        }
        gameUI.OffMassegePanel();
        yield return new WaitForSeconds(1.0f);

        yield break;
    }
}
