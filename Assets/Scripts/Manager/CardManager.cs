using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    //カードバトル・勝敗判定
    public IEnumerator CardBattle(Player player, Enemy enemy, GameUI gameUI)
    {
        gameUI.OnMassegePanel();
        Hand.Instance.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.2f);
        for (int i = 0; i < player.SubmitList.Count; i++)
        {
            Card card = player.SubmitList[i];
            Card flontCard = null;
            if (i != 0)
            {
                flontCard = player.SubmitList[i - 1];
            }
            card.transform.position += Vector3.up * 0.2f;

            //カードの効果処理
            yield return StartCoroutine(card.Base.UniqueEffect.Execute(card, flontCard, player, enemy));

            yield return new WaitForSeconds(1.2f);
        }

        yield return new WaitForSeconds(1f);
        //敵の状態表示
        yield return StartCoroutine(enemy.EnemySituation(MessageText.message));

        player.PastLife = player.Life;
        gameUI.OffMassegePanel();
    }
}
