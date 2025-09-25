using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Singleton<Player>
{
    //[SerializeField] Hand hand;
    //[SerializeField] SubmitPosition submitPosition;
    [SerializeField] StatusEffectGenerator statusEffectGenerator;

    [Header("プレイヤーベースステータス")]
    [SerializeField] int lifeMax;
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
    public List<Card> SubmitList { get => SubmitPosition.Instance.Submitlist; }
    public StatusEffectGenerator StatusEffectGenerator { get => statusEffectGenerator; set => statusEffectGenerator = value; }
    public List<StatusEffect> Effects { get => effects; set => effects = value; }



    public void SetPlayer(int handMax)
    {
        life = lifeMax;
        Hand.Instance.cardInterval = Hand.Instance.cardInterval * 6f / handMax;
        SetupNext();
    }

    //生成されたカードをリストに追加・カードクリック時の効果追加
    public void SerCardToHand(Card card)
    {
        Hand.Instance.Add(card);
        card.OnClickCard = SelectedCard;
    }

    //カードクリック時のリアクション
    public void SelectedCard(Card card)
    {
        if (card.Base.PlayCondition != true)
            return;

        if (card.transform.parent == SubmitPosition.Instance.transform)
        {
            SubmitPosition.Instance.ReRemove(card);
            SubmitPosition.Instance.SubmitCard = null;
            Hand.Instance.Add(card);
            Hand.Instance.RePosition(card);
            SubmitPosition.Instance.SubmitPositionIn();
            card.PosReset();
            card.effectReSet();

        }
        else if (SubmitPosition.Instance.SubmitCard != null)
        {
            return;
        }
        else if (card.transform.parent == Hand.Instance.transform)
        {
            SubmitPosition.Instance.Set(card);
            Hand.Instance.RemoveList(card);
            Hand.Instance.ResetPosition();
            card.PosReset();
        }

    }

    public void PlayConditionCheck(Enemy enemy, Deck deck)
    {

        for (int i = 0; i < Hand.Instance.List.Count; i++)
        {
            Hand.Instance.List[i].Base.UniqueEffect.PlayCondition(Hand.Instance.List[i], this, enemy, deck, GameMaster.TurnCount);
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
    public void SetupNext()
    {
        SubmitPosition.Instance.DeleteCard();
        Defens = 0;
        GetComponent<EffectCount>().StatusEffectCount(Effects, statusEffectGenerator.PlayerXY);
        Hand.Instance.gameObject.SetActive(true);
    }
}
