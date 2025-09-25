using System.Collections.Generic;
using UnityEngine;

public class StatusEffectGenerator : MonoBehaviour
{
    [SerializeField] StatusEffect statuEffectPrefab;
    [SerializeField] List<StatusEffectBase> effectBases;
    [SerializeField] Vector2 xy;
    [SerializeField] Vector2 playerXY;

    public Vector2 Xy { get => xy; set => xy = value; }
    public Vector2 PlayerXY { get => playerXY; set => playerXY = value; }

    public string EnemyGrantEffect(Enemy enemy,StatusEffectBase effectBase)
    {

        if (enemy.Base.Effects.Count <= 5)
        {
            StatusEffect statusEffect = Spawn(effectBase);
            enemy.Base.Effects.Add(statusEffect);
           
            statusEffect.transform.SetParent(enemy.gameObject.transform);
            enemy.GetComponent<EffectCount>().alignment(enemy.Base.Effects, Xy);
            return ($"{statusEffect.Base.DisplayName}状態にした！");
        }
        else
        {
            return ($"デバフは付与されなかった");
        }
    }
    public string PlayerGrantEffect(Player player, StatusEffectBase effectBase)
    {

        if (player.Effects.Count <= 5)
        {
            StatusEffect statusEffect = Spawn(effectBase);
            player.Effects.Add(statusEffect);

            statusEffect.transform.SetParent(player.gameObject.transform);
            player.GetComponent<EffectCount>().alignment(player.Effects, playerXY);
            return ($"{statusEffect.Base.DisplayName}状態になった！");
        }
        else
        {
            return ($"バフは付与されなかった");
        }
    }
    //バフを生成する
    public StatusEffect Spawn(StatusEffectBase Base)
    {
        StatusEffect statusEffect = Instantiate(statuEffectPrefab);
        if (Base == null)
        {
            Debug.Log($"バフは存在しません");
        }
        statusEffect.SetEffect(Base);
        statusEffect.transform.position = Xy;

        return statusEffect;
    }

}
