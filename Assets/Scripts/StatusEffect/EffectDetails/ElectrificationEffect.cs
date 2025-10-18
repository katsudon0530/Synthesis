using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "StatusEffect/ElectrificationEffect")]

public class ElectrificationEffect : EffectDetails
{
    [SerializeField] int grants;
    public override IEnumerator Execute(StatusEffect statusEffect, Enemy enemy)
    {

        if (statusEffect.CountGrant >= grants)
        {
            MessageText.TextIn($"{enemy.Base.Name}に帯電が最大数溜まった！");
            enemy.Act = false;
            yield return new WaitForSeconds(1.0f);
            MessageText.TextIn($"{enemy.Base.Name}は行動不能になった！");
            statusEffect.CountTurn = 1;
        }

        yield return new WaitForSeconds(1.0f);
        yield break;
    }
}
