using System.Collections.Generic;
using UnityEngine;

public class Hand : Singleton<Hand>
{
    protected override bool IsPersistent => false;

    List<Card> list = new();
    public float cardInterval;

    public List<Card> List { get => list; set => list = value; }


    //カードをリストに追加
    public void Add(Card card)
    {
        list.Add(card);
        card.transform.SetParent(this.transform);
        ResetPosition();
    }

    //カードをリストから削除
    public void RemoveList(Card card)
    {
        list.Remove(card);
    }

    //リストの範囲をとってカードきれいに並べる
    public void ResetPosition()
    {
        list.Sort((card0,card1) => card0.Base.ID - card1.Base.ID);
        for (int i = 0; i < list.Count; i++)
        {
            if(list.Count % 2 == 0)
            {
                float posX = (i - list.Count / 2) * cardInterval + cardInterval / 2;
                list[i].transform.localPosition = new Vector3(posX, 0);
            }
            else
            {
                float posX = (i - list.Count / 2) * cardInterval;
                list[i].transform.localPosition = new Vector3(posX, 0);
            }
            Transform childTransform = list[i].transform.GetChild(0);
            Canvas canvas = childTransform.GetComponent<Canvas>();
            canvas.sortingOrder = i;

        }
    }

}
