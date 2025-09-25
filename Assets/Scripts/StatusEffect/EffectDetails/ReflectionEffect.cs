using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "StatusEffect/ReflectionEffect")]

public class ReflectionEffect : EffectDetails
{
    [SerializeField] float Magnification;
    public override IEnumerator Execute(StatusEffect statusEffect, Enemy enemy)
    {
        
        int damage = Player.Instance.PastLife - Player.Instance.Life;
        damage = (int)(damage * Magnification);

        enemy.Base.EnemyLife -= damage;
        MessageText.TextIn($"{damage}の反射ダメージ与えた");
        if (enemy.Base.EnemyLife < 0)
        {
            enemy.Base.EnemyLife = 0;
        }

        yield return new WaitForSeconds(1.0f);
        yield break;
    }
}
