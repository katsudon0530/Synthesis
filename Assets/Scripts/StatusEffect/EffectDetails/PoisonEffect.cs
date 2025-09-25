using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "StatusEffect/PoisonEffect")]

public class PoisonEffect : EffectDetails
{
    [SerializeField] int defaultDamage;
    [SerializeField] int MaxDamage;

    public override IEnumerator Execute(StatusEffect statusEffect, Enemy enemy)
    {
        int damage = defaultDamage * statusEffect.CountGrant;
        if (damage > MaxDamage)
        {
            damage = MaxDamage;
        }

        enemy.Base.EnemyLife -= damage;
        MessageText.TextIn($"{damage}の毒ダメージを与えた");
        if (enemy.Base.EnemyLife < 0)
        {
            enemy.Base.EnemyLife = 0;
        }

        yield return new WaitForSeconds(1.0f);
        yield break;
    }
}
