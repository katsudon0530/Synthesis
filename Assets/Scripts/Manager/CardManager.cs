using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{

    [SerializeField] Synthesis synthesis;
    private Deck deck;
    private SubmitPosition field;

    public void Awake()
    {
        deck = Deck.Instance;
        field = SubmitPosition.Instance;
    }


    //カードバトル・勝敗判定
    public IEnumerator CardBattle(Enemy enemy)
    {
        MessageText.Panel(true);

        Player player = Player.Instance;

        yield return new WaitForSeconds(1.2f);
        for (int i = 0; i < field.SubmitList.Count; i++)
        {
            Card card = field.SubmitList[i];
            Card flontCard = null;
            if (i != 0)
            {
                flontCard = field.SubmitList[i - 1];
            }
            card.transform.position += Vector3.up * 0.2f;

            //カードの効果処理
            yield return StartCoroutine(card.Base.UniqueEffect.Execute(card, flontCard, player, enemy));

            yield return new WaitForSeconds(1.2f);
        }

        yield return new WaitForSeconds(1f);
        //敵の状態表示
        yield return StartCoroutine(enemy.EnemySituation());

        player.PastLife = player.Life;
        MessageText.Panel(false);
    }


    //カードを合成する
    public IEnumerator CardSynthesis()
    {
        MessageText.Panel(true);

        Vector2 goal = field.SubmitList[0].transform.position;

        for (int i = 1; i < field.SubmitList.Count; i++)
        {
            StartCoroutine(synthesis.CardSlide(field.SubmitList[i], goal, 0.5f));
        }
        yield return new WaitForSeconds(0.7f);

        //合成カードに変化させほかのカードを壊す
        synthesis.CardSynthesis(field.SubmitList, deck.DeckAll);
        yield return StartCoroutine(synthesis.CardSlide(field.SubmitList[0], SubmitPosition.Instance.transform.position, 0.7f));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(synthesis.Close(field.SubmitList[0]));
        yield return new WaitForSeconds(1f);

        MessageText.Panel(false);
    }
}
