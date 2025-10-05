using System.Collections.Generic;
using UnityEngine;

public class SubmitPosition : Singleton<SubmitPosition>
{
    protected override bool IsPersistent => false;

    [SerializeField] Synthesis synthesis;
    [SerializeField] GameObject cardGuide;

    public List<Card> submitList = new();
    public List<Card> SubmitList { get => submitList; set => submitList = value; }


    //カードをフィールドにセットする
    public void Set(Card card)
    {
        if (submitList.Count < 2 )
        {
            AddCard(card);
        }
        else if (submitList.Count == 2 )
        {
            AddCard(card);
        }
        else
        {
            return;
        }
        effectSwitch(submitList);
    }

    //場のカードを消去する
    public void DeleteCard()
    {
        for (int i = 0; i < submitList.Count; i++)
        {
            Destroy(submitList[i].gameObject);
        }
        submitList.Clear();
    }

    //カードをリストに追加
    void AddCard(Card card)
    {
        submitList.Add(card);
        card.transform.SetParent(this.transform);
        card.transform.position = transform.position;
        SubmitPositionIn();
    }

    //カードをリストから削除
    public void RemoveCard(Card card)
    {
        submitList.Remove(card);
    }

    //フィールドに置かれたカードの位置を整列させる
    public void SubmitPositionIn()
    {
        for (int i = 0; i < submitList.Count; i++)
        {
            float posX = 2.5f * (i - 1f);
            submitList[i].transform.localPosition = new Vector3(posX, 0);

            Transform childTransform = submitList[i].transform.GetChild(0);
            Canvas canvas = childTransform.GetComponent<Canvas>();
            canvas.sortingOrder = 2 - i;
        }
        effectSwitch(submitList);
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
