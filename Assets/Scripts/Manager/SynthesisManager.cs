using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynthesisManager : MonoBehaviour
{
    [SerializeField] Synthesis synthesis;
    private Deck deck;

    public void Awake()
    {
         deck = Deck.Instance;
    }

    public void SetButton()
    {
        synthesis.OffSynthesisButton();
    }

    //カードを合成する
    public IEnumerator CardSynthesis()
    {
        MessageText.Panel(true);
        Hand.Instance.gameObject.SetActive(false);
        SubmitPosition feild = SubmitPosition.Instance;

        Vector2 goal = feild.Submitlist[0].transform.position;

        for (int i = 1; i < feild.Submitlist.Count; i++)
        {
            StartCoroutine(synthesis.CardSlide(feild.Submitlist[i], goal, 0.5f));
        }
        yield return new WaitForSeconds(0.7f);

        //合成カードに変化させほかのカードを壊す
        synthesis.CardSynthesis(feild.Submitlist, deck.DeckAll);
        yield return StartCoroutine(synthesis.CardSlide(feild.Submitlist[0], SubmitPosition.Instance.transform.position, 0.7f));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(synthesis.Close(feild.Submitlist[0]));
        yield return new WaitForSeconds(1f);

        MessageText.Panel(false);
    }
}
