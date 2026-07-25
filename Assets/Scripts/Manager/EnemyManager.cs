using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyGenerator enemyGenerator;
    private BattleLog battleLog;
    public Enemy currentEnemy { get; private set; }

    //エネミーを生成してフィールドにセット
    public Enemy GenerateEnemy(int enemyNum)
    {
        currentEnemy = enemyGenerator.SpawnEnemy(enemyNum);
        Field.Instance.SetEnemy(currentEnemy);
        return currentEnemy;
    }

    //エネミーの行動
    public IEnumerator EnemyTurn()
    {
        if (currentEnemy == null)
            yield break;
        if (currentEnemy.Life == 0)
        {
            currentEnemy.EnemyDestroy();
            yield break;
        }

        MessageText.Panel(true);

        if (currentEnemy.Act == false)
        {
            battleLog.SendMessage($"{currentEnemy.Base.Name}は動けない！");
        }
        else
        {
            //敵の攻撃
            List<ActionList> list = currentEnemy.Base.ActionLists;
            float num = Random.Range(0f, 100f);
            float per = 0;
            for (int i = 0;i < list.Count; i++)
            {
                per += list[i].percent;
                if (per > num)
                {
                    yield return StartCoroutine(list[i].enemyAction.Execute(currentEnemy));
                    break;
                }
            } 
        }

        yield return new WaitForSeconds(1.5f);
        currentEnemy.EnemyCountDown();
        MessageText.Panel(false);
    }



    public IEnumerator EnemyEffectBoot()
    {
        if (currentEnemy.Base.Effects.Count == 0 || currentEnemy.Life == 0)
        {
            yield break;
        }

        MessageText.Panel(true);
        
        foreach (StatusEffect effect in currentEnemy.Base.Effects)
        {
            if(effect != null)
            {
                yield return StartCoroutine(effect.Base.EffectDetails.Execute(effect, currentEnemy));
            }
        }
        MessageText.Panel(false);
        yield return new WaitForSeconds(1.0f);

        yield break;
    }

    public void EndSet()
    {
        Debug.Log($"敵のLife：{currentEnemy.Life}");

        if (currentEnemy != null)
            currentEnemy.EnemyReSet();
    }

    public void Initialize(BattleLog masterLog)
    {
        this.battleLog = masterLog;
    }
}
