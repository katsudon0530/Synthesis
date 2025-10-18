using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "StatusEffect/ParalysisEffect")]

public class ParalysisEffect : EffectDetails
{

    [SerializeField] List<ProbabilityCount> probabilities = new List<ProbabilityCount>();
    public override IEnumerator Execute(StatusEffect statusEffect, Enemy enemy)
    {
        ProbabilityCount found = probabilities.Find(count => count.count == statusEffect.CountTurn);
        if (found == null)
        {
            found = probabilities[0];
        }

        //このターン体力が減少していると発動しない
        if (enemy.DecreaseLife)
        {
            MessageText.TextIn($"ダメージを受けて麻痺が無効化された！");
            yield return new WaitForSeconds(1.0f);
            yield break;
        }

        //エフェクトカウントを参照してターンごとの割合で行動不能になる
        if (Random.value <= found.probability)
        {
            MessageText.TextIn($"麻痺の効果が発動した！");
            enemy.Act = false;
        }
        else
        {
            MessageText.TextIn($"麻痺は発動しなかった！");
        }

        yield return new WaitForSeconds(1.0f);
        yield break;
    }
}

[System.Serializable]
public class ProbabilityCount
{
    public int count;
    public float probability;
}

