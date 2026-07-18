using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class Player : Singleton<Player>
{
    protected override bool IsPersistent => true;

    [Header("プレイヤーベースステータス")]
    [SerializeField] int lifeMax = 100;
    [Space(10)]
    [Header("実数値")]
    [SerializeField] int life;
    [SerializeField] int defens;

    int pastLife;
    List<StatusEffect> effects = new List<StatusEffect>();

    public int LifeMax { get => lifeMax; set => lifeMax = value; }
    public int Defens { get => defens; set => defens = value; }
    public int Life { get => life; set => life = value; }
    public int PastLife { get => pastLife; set => pastLife = value; }
    public List<StatusEffect> Effects { get => effects; set => effects = value; }

    public void SetPlayer()
    {
        life = lifeMax;
        if(GetComponent<EffectCount>() == null)
            gameObject.AddComponent<EffectCount>();
        GameMaster.OnStateChanged += SetupNext;
    }


    public void PlayConditionCheck(Enemy enemy, List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Base.UniqueEffect.PlayCondition(cards[i], enemy);
        }
    }


    public IEnumerator PlayerEffectBoot(Enemy enemy)
    {
        if (effects.Count == 0)
        {
            Debug.Log("何もない");
            yield break;
        }

        MessageText.Panel(true);

        foreach (StatusEffect effect in effects)
        {
            if (effect != null)
            {
                yield return StartCoroutine(effect.Base.EffectDetails.Execute(effect, enemy));
            }
        }
        MessageText.Panel(false);
        yield return new WaitForSeconds(1.0f);

        yield break;
    }

    //次のターンでの関数のリセット
    public void SetupNext(TurnState state)
    {
        switch (state)
        {
            case TurnState.end:
                Defens = 0;
                GetComponent<EffectCount>().StatusEffectCount(effects);
                break;
        }

    }
    private void OnDestroy()
    {
        GameMaster.OnStateChanged -= SetupNext;
    }
}
