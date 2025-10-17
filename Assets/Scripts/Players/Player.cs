using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public void SetPlayer(int handMax)
    {
        life = lifeMax;
        Field.Instance.cardInterval *= (6f / handMax);
        if(GetComponent<EffectCount>() == null)
            gameObject.AddComponent<EffectCount>();
        GameMaster.OnStateChanged += SetupNext;
    }

    //生成されたカードをリストに追加・カードクリック時の効果追加
    public void SerCardToHand(Card card)
    {
        Field.Instance.HandSet(card);
        card.OnClickCard = SelectedCard;
    }

    //カードクリック時のリアクション
    public void SelectedCard(Card card)
    {
        if (card.Base.PlayCondition != true || GameMaster.turnState != TurnState.cardSet)
            return;

        if (card.transform.parent == Field.Instance.BattleField.transform)
        {
            Field.Instance.HandSet(card);
        }
        else if (Field.Instance.Stand.Count >= 3)
        {
            return;
        }
        else if (card.transform.parent == Field.Instance.PlayerHand.transform)
        {
            Field.Instance.StandSet(card);
        }

    }

    public void PlayConditionCheck(Enemy enemy, Deck deck)
    {
        var cards = Field.Instance.Hand;
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Base.UniqueEffect.PlayCondition(cards[i], this, enemy, deck, GameMaster.TurnCount);
        }
        
    }


    public IEnumerator PlayerEffectBoot(Enemy enemy)
    {
        MessageText.Panel(true);


        if (effects.Count == 0)
        {
            Debug.Log("何もない");
            yield break;
        }

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
        if (state == TurnState.end)
        {
            Field.Instance.DeleteCard();
            Defens = 0;
            GetComponent<EffectCount>().StatusEffectCount(effects);
            Field.Instance.PlayerHand.SetActive(true);
        }

    }
}
