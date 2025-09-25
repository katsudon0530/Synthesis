using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynthesisManager : MonoBehaviour
{
    [SerializeField] Synthesis synthesis;
    [SerializeField] Deck deck;


    public void SetButton()
    {
        synthesis.OffSynthesisButton();
    }

    //カードを合成する
    public IEnumerator CardSynthesis(Player player,GameUI gameUI)
    {
        gameUI.OnMassegePanel();
        Hand.Instance.gameObject.SetActive(false);

        Vector2 goal = player.SubmitList[0].transform.position;

        for (int i = 1; i < player.SubmitList.Count; i++)
        {
            StartCoroutine(synthesis.CardSlide(player.SubmitList[i], goal, 0.5f));
        }
        yield return new WaitForSeconds(0.7f);

        //合成カードに変化させほかのカードを壊す
        synthesis.CardSynthesis(player.SubmitList, deck.DeckAll);
        yield return StartCoroutine(synthesis.CardSlide(player.SubmitList[0], SubmitPosition.Instance.transform.position, 0.7f));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(synthesis.Close(player.SubmitList[0]));
        yield return new WaitForSeconds(1f);

        gameUI.OffMassegePanel();
    }
}
