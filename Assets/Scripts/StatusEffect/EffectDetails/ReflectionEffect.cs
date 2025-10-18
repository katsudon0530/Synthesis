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

        enemy.Life -= damage;
        MessageText.TextIn($"{damage}の反射ダメージ与えた");
        if (enemy.Life < 0)
        {
            enemy.Life = 0;
        }

        yield return new WaitForSeconds(1.0f);
        yield break;
    }
}
