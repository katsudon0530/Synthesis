using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Synthesis : MonoBehaviour
{
    [SerializeField] CardGenerator generator;
    [SerializeField] Dictionary dictionary;
    [SerializeField] Button SynthesisButton;
    int id = 0;


    //カードを合成する
    public void CardSynthesis(List<Card> card, List<int> Deck)
    {
        if(card.Count == 2)
        {
            id = DoubleSearchID(card[0].Base.ID, card[1].Base.ID);
        }
        else if(card.Count == 3)
        {
            id = TripleSearchID(card[0].Base.ID, card[1].Base.ID, card[2].Base.ID);
        }
        
        //合成したカードを手札に生成
        Deck.Add(id);
        Card synthesisCard = generator.Spawn(id);
        Player.Instance.SerCardToHand(synthesisCard);
        Hand.Instance.ResetPosition();

        //デッキから合成したカードを削除
        for (int i = 0; i < card.Count; i++)
        {
            int index = Deck.FindIndex(number => number == card[i].Base.ID);
            Deck.RemoveAt(index);
        }

        //合成した場のカードを壊す
        for (int i = 1; card.Count > 1;)
        {
            Destroy(card[i].gameObject);
            card.RemoveAt(i);
        }

    }

    //2枚で合成するカードを探す
    public int DoubleSearchID(int card1ID, int card2ID)
    {
        int SearchID = 0;

        IdDataDouble DoubleID = dictionary.SynthesisDouble.Find(id => id.card_1_ID == card1ID && id.card_2_ID == card2ID);
        
        if (DoubleID != null)
        {
            SearchID = DoubleID.SynthesisCard;
        }
        return SearchID;
    }

    //3枚で合成するカードを探す
    public int TripleSearchID(int card1ID,int card2ID, int card3ID)
    {
        int SearchID = 0;

        IdDataTriple TripleID = dictionary.SynthesisTriple.Find(id => id.card_1_ID ==card1ID &&  id.card_2_ID ==card2ID && id.card_3_ID ==card3ID);

        if (TripleID != null)
        {
            SearchID = TripleID.SynthesisCard;
        }

        return SearchID;

    }

    //カードを移動させて回転
    public IEnumerator Close(Card card)
    {
        float rotationAngle = 180.0f;
        float duration = 0.5f;

        float elapsedTime = 0.0f;
        Quaternion startRotation = card.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, rotationAngle, 0);

        //一回転目
        while (elapsedTime < duration)
        {
            // 経過時間に基づいて回転を線形補間
            card.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            if (200.0f < card.transform.eulerAngles.y && card.transform.eulerAngles.y < 270.0f)
            {
                card.Close();
            }
            yield return null; 
        }

        // 最後に目標の回転にぴったり合わせる
        card.transform.rotation = endRotation;
        MessageText.TextIn("カードを合成した");
        yield return new WaitForSeconds(0.8f);

        rotationAngle = 0.0f;
        elapsedTime = 0.0f;
        startRotation = card.transform.rotation;
        endRotation = Quaternion.Euler(0, rotationAngle, 0);
        //2回転目
        while (elapsedTime < duration)
        {
            // 経過時間に基づいて回転を線形補間
            card.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            if (270f < card.transform.eulerAngles.y )
            {
                card.Open();
                generator.ChangeCard(card,id);
            }
            yield return null;  // 次のフレームまで待機
        }

        // 最後に目標の回転にぴったり合わせる
        card.transform.rotation = endRotation;
    }

    public IEnumerator CardSlide(Card card, Vector2 goal, float slideDuration)
    {
        float elapsedTime = 0.0f;

        Vector2 start = card.transform.position;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;

            card.transform.position = Vector2.Lerp(start, goal, elapsedTime / slideDuration);
            yield return null;
        }
        card.transform.position = goal;
        yield break;
    }

    //裏面を表示
    public void OffSynthesisButton()
    {
        SynthesisButton.interactable = false;
    }

    //裏面を非表示
    public void OnSynthesisButton()
    {
        SynthesisButton.interactable = true;
    }
}
