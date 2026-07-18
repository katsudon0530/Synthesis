using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.CullingGroup;
using static UnityEngine.GraphicsBuffer;
using CardMove;

public class Field : Singleton<Field>
{
    protected override bool IsPersistent => false;

    [SerializeField] GameObject _battleField;
    [SerializeField] GameObject _playerHand;
    [SerializeField] GameObject _enemyField;
    [SerializeField] GameObject cardGuide;
    public float cardInterval;

    private List<Card> _hand = new();
    private List<Card> _stand = new();

    private CardMove.CardMove cardMove = new CardMove.CardMove();

    public List<Card> Hand { get => _hand; set => _hand = value; }
    public List<Card> Stand { get => _stand; set => _stand = value; }
    public GameObject BattleField { get => _battleField; set => _battleField = value; }

    protected override void Awake()
    {
        base.Awake();

        GameMaster.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(TurnState state)
    {
        switch (state)
        {
            case TurnState.synthesis:
            case TurnState.battle:
                _playerHand.SetActive(false);
                cardGuide.SetActive(false);
                break;
            case TurnState.end:
                DeleteCard();
                break;
            case TurnState.start:
                cardGuide.SetActive(true);
                StartCoroutine(StartHand());
                break;
        }
    }
    //手札の配置を決定
    public void SettingHand(int handMax)
    {
        cardInterval *= (6f / handMax);
    }
    //カードをフィールドにセットする
    public void StandSet(Card card)
    {
        if (_stand.Count == 3)
            return;

        CardSet(card, _stand, _battleField);
    }

    //カードをハンドにセットする
    public void HandSet(Card card)
    {
        CardSet(card, _hand, _playerHand);
        card.effectReSet();
    }

    //リストにカードをセットして、位置を変更する
    public void CardSet(Card card, List<Card> cards, GameObject obj)
    {
        _hand.Remove(card);
        _stand.Remove(card);
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

    //生成されたカードをリストに追加・カードクリック時の効果追加
    public void SerCardToHand(Card card)
    {
        HandSet(card);
        card.OnClickCard = SelectedCard;
    }

    //fieldカードクリック時のリアクション
    public void SelectedCard(Card card)
    {
        if (card.Base.PlayCondition != true || GameMaster.turnState != TurnState.cardSet)
            return;

        if (card.transform.parent == _battleField.transform)
        {
            HandSet(card);
        }
        else if (Stand.Count >= 3)
        {
            return;
        }
        else if (card.transform.parent == _playerHand.transform)
        {
            StandSet(card);
        }

    }


    //リストの範囲をとってカードきれいに並べる
    public void HandPosition()
    {
        _hand.Sort((card0, card1) => card0.Base.ID - card1.Base.ID);
        for (int i = 0; i < _hand.Count; i++)
        {
            float posX = (i - _hand.Count / 2) * cardInterval;
            if (_hand.Count % 2 == 0)
                posX += cardInterval / 2;
            _hand[i].transform.localPosition = new Vector3(posX, 0);

            Canvas canvas = _hand[i].transform.GetChild(0).GetComponent<Canvas>();
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

    public IEnumerator StartHand()
    {
        _playerHand.SetActive(true);

        for (int i = 0; i < _hand.Count; i++)
        {
            Vector2 pos = _playerHand.transform.position;

            _hand[i].transform.localPosition = new Vector2((-_hand.Count / 2f) * cardInterval, 0f);

            pos.x = (i - _hand.Count / 2) * cardInterval;
            if (_hand.Count % 2 == 0)
                pos.x += cardInterval / 2;

            StartCoroutine(cardMove.Slide(_hand[i], pos, 0.5f));
        }

        yield return null;
    }

    private void OnDestroy()
    {
        GameMaster.OnStateChanged -= OnStateChanged;
    }

}
