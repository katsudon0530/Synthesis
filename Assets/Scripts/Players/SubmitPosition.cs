using System.Collections.Generic;
using UnityEngine;

public class SubmitPosition : Singleton<SubmitPosition>
{
    [SerializeField] Synthesis synthesis;
    [SerializeField] GameObject cardGuide;
    Card submitCard;

    public List<Card> submitlist = new();
    public Card SubmitCard { get => submitCard; set => submitCard = value; }
    public List<Card> Submitlist { get => submitlist; set => submitlist = value; }


    //カードをフィールドにセットする
    public void Set(Card card)
    {
        if (submitlist.Count < 2 )
        {
            Add(card);
            if (submitlist.Count > 1)
            {
                synthesis.OnSynthesisButton();
            }
        }
        else if (submitlist.Count == 2 )
        {
            Add(card);
            SubmitCard = card;
        }
        else
        {
            return;
        }
        effectSwitch(submitlist);
    }

    //場のカードを消去する
    public void DeleteCard()
    {
        for (int i = 0; i < submitlist.Count; i++)
        {
            Destroy(submitlist[i].gameObject);
        }
        submitlist.Clear();
        submitCard = null;
    }

    //カードをリストに追加
    void Add(Card card)
    {
        submitlist.Add(card);
        card.transform.SetParent(this.transform);
        card.transform.position = transform.position;
        SubmitPositionIn();
    }

    //カードをリストから削除
    public void ReRemove(Card card)
    {
        submitlist.Remove(card);
        if (submitlist.Count < 2)
        {
            synthesis.OffSynthesisButton();
        }
    }

    //フィールドに置かれたカードの位置を整列させる
    public void SubmitPositionIn()
    {
        for (int i = 0; i < submitlist.Count; i++)
        {
            float posX = 2.5f * (i - 1f);
            submitlist[i].transform.localPosition = new Vector3(posX, 0);

            Transform childTransform = submitlist[i].transform.GetChild(0);
            Canvas canvas = childTransform.GetComponent<Canvas>();
            canvas.sortingOrder = 2 - i;
        }
        effectSwitch(submitlist);
    }

    //どのエフェクトが付くかを判別する
    public void effectSwitch(List<Card> submitCards)
    {
        for(int i = 0;i < submitCards.Count;i++)
        {
            submitCards[i].effectReSet();

            if (i == 0) { }

            else if (submitCards[i - 1] == null )
            {
                submitCards[i].effectReSet();
            }
            else
            {
                string cardName = submitCards[i - 1].Base.CardName;
                FlontBuff foundBuff = submitCards[i].Base.FlontBuff.Find(buff => buff.flontCard == cardName);
                if (foundBuff != null)
                {
                    effectJudgement(foundBuff.buff, submitCards[i]);
                }
            }
        }
    }

    //エフェクトを表示する
    private void effectJudgement(float magnification,Card card)
    {
        if (magnification != 1f)
        {
            card.effectSet(magnification);
        }
        else
        {
            card.effectReSet();
        }
    }

    private void Update()
    {
        cardGuide.SetActive(GameMaster.CardSet ? false : true);
    }
}
