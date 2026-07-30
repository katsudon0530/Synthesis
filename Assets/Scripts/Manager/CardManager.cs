using System.Collections;
using UnityEngine;
using CardMove;

public class CardManager : MonoBehaviour
{

    [SerializeField] Synthesis synthesis;
    private BattleLog battleLog;
    private Deck deck => Deck.Instance;
    private Field field => Field.Instance;
    private Player player => Player.Instance;

    private CardMove.CardMove cardMove = new CardMove.CardMove();


    //カードバトル・勝敗判定
    public IEnumerator CardBattle(Enemy enemy)
    {
        MessageText.Panel(true);

        yield return new WaitForSeconds(1.2f);
        for (int i = 0; i < field.Stand.Count; i++)
        {
            Card card = field.Stand[i];

            card.transform.position += Vector3.up * 0.2f;

            //カードの効果処理
            BattleContext context = new BattleContext();
            context.SetContext(card, enemy, battleLog);
            yield return StartCoroutine(card.Base.UniqueEffect.Execute(context));

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

        if (GameData.Instance.synthesisCount > 0)
        {
            Vector2 goal = field.Stand[0].transform.position;

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

            GameData.Instance.synthesisCount--;
        }
        //一応の処理使わない筈
        else
        {
            field.ReturnHand();
            battleLog.SendMessage("合成はもうできない！");
            yield return new WaitForSeconds(1f);
        }
        MessageText.Panel(false);

        yield return new WaitForSeconds(0.5f);
    }

    public void Initialize(BattleLog masterLog)
    {
        this.battleLog = masterLog;
    }
}
