using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class Field : Singleton<Field>
{
    protected override bool IsPersistent => false;

    [SerializeField] GameObject _battleField;
    [SerializeField] GameObject _playerHand;
    [SerializeField] GameObject cardGuide;
    public float cardInterval;

    private List<Card> _hand = new();
    private List<Card> _stand = new();

    public List<Card> Hand { get => _hand; set => _hand = value; }
    public List<Card> Stand { get => _stand; set => _stand = value; }
    public GameObject BattleField { get => _battleField; set => _battleField = value; }
    public GameObject PlayerHand { get => _playerHand; set => _playerHand = value; }


    //カードをフィールドにセットする
    public void StandSet(Card card)
    {
        if (_stand.Count == 3)
            return;

        RemoveCard(card);
        CardSet(card, _stand, _battleField);
    }

    //カードをハンドにセットする
    public void HandSet(Card card)
    {
        RemoveCard(card);
        CardSet(card, _hand, _playerHand);
        card.effectReSet();
    }

    //カードをリストから削除
    private void RemoveCard(Card card)
    {
        foreach (var list in new[] { _hand, _stand })
            list.Remove(card);
    }

    //リストにカードをセットして、位置を変更する
    public void CardSet(Card card, List<Card> cards, GameObject obj)
    {
        RemoveCard(card);
        cards.Add(card);
        card.transform.SetParent(obj.transform);
        FieldPosition();
        HandPosition();
    }

    //場のカードを消去する
    public void DeleteCard()
    {
        for (int i = 0; i < _stand.Count; i++)
        {
            Destroy(_stand[i].gameObject);
        }
        _stand.Clear();
    }


    //リストの範囲をとってカードきれいに並べる
    public void HandPosition()
    {
        _hand.Sort((card0, card1) => card0.Base.ID - card1.Base.ID);
        for (int i = 0; i < _hand.Count; i++)
        {
            if (_hand.Count % 2 == 0)
            {
                float posX = (i - _hand.Count / 2) * cardInterval + cardInterval / 2;
                _hand[i].transform.localPosition = new Vector3(posX, 0);
            }
            else
            {
                float posX = (i - _hand.Count / 2) * cardInterval;
                _hand[i].transform.localPosition = new Vector3(posX, 0);
            }
            Transform childTransform = _hand[i].transform.GetChild(0);
            Canvas canvas = childTransform.GetComponent<Canvas>();
            canvas.sortingOrder = i;

        }
    }


    //フィールドに置かれたカードの位置を整列させる
    public void FieldPosition()
    {
        for (int i = 0; i < _stand.Count; i++)
        {
            float posX = 2.5f * (i - 1f);
            _stand[i].transform.localPosition = new Vector3(posX, 0);

            Transform childTransform = _stand[i].transform.GetChild(0);
            Canvas canvas = childTransform.GetComponent<Canvas>();
            canvas.sortingOrder = 2 - i;
        }
        effectSwitch();
    }



    //どのエフェクトが付くかを判別する
    public void effectSwitch()
    {
        for (int i = 0; i < _stand.Count; i++)
        {
            _stand[i].effectReSet();

            if (i == 0) { }

            else if (_stand[i - 1] == null)
            {
                _stand[i].effectReSet();
            }
            else
            {
                string cardName = _stand[i - 1].Base.CardName;
                FlontBuff foundBuff = _stand[i].Base.FlontBuff.Find(buff => buff.flontCard == cardName);
                if (foundBuff != null)
                {
                    _stand[i].effectSet(foundBuff.buff);
                }
            }
        }
    }


    private void Update()
    {
        cardGuide.SetActive(GameMaster.CardSet ? false : true);
    }
}
