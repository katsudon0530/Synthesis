using System.Collections;
using UnityEngine;
using CardMove;

public class CardManager : MonoBehaviour
{

    [SerializeField] Synthesis synthesis;
    private Deck deck;
    private Field field;

    public void Awake()
    {
        deck = Deck.Instance;
        field = Field.Instance;
    }


    //カードバトル・勝敗判定
    public IEnumerator CardBattle(Enemy enemy)
    {
        MessageText.Panel(true);

        Player player = Player.Instance;

        yield return new WaitForSeconds(1.2f);
        for (int i = 0; i < field.Stand.Count; i++)
        {
            Card card = field.Stand[i];
            Card flontCard = null;
            if (i != 0)
            {
                flontCard = field.Stand[i - 1];
            }
            card.transform.position += Vector3.up * 0.2f;

            //カードの効果処理
            yield return StartCoroutine(card.Base.UniqueEffect.Execute(card, flontCard, enemy));

            yield return new WaitForSeconds(1.2f);
        }

        yield return new WaitForSeconds(1f);
        //敵の状態表示
        yield return StartCoroutine(enemy.EnemySituation());

        player.PastLife = player.Life;
        MessageText.Panel(false);

        yield return new WaitForSeconds(0.5f);
    }


    //カードを合成する
    public IEnumerator CardSynthesis()
    {
        MessageText.Panel(true);

        Vector2 goal = field.Stand[0].transform.position;
        CardMove.CardMove cardMove = new CardMove.CardMove();

        for (int i = 1; i < field.Stand.Count; i++)
        {
            StartCoroutine(cardMove.Slide(field.Stand[i], goal, 0.5f));
        }
        yield return new WaitForSeconds(0.7f);
        goal = field.BattleField.transform.position;
        //合成カードに変化させほかのカードを壊す
        synthesis.CardSynthesis(field.Stand, deck.DeckAll);
        yield return StartCoroutine(cardMove.Slide(field.Stand[0], goal, 0.7f));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(synthesis.Close(field.Stand[0]));
        yield return new WaitForSeconds(1f);

        MessageText.Panel(false);

        yield return new WaitForSeconds(0.5f);
    }
}
