using UnityEngine;

public class WindowGenerator : MonoBehaviour
{
    [SerializeField] CardWindow windowPrefab;   
    [SerializeField] Vector2 xy;

    CardWindow cardWindow = null;

    //カードの詳細情報が記されたウィンドウを生成する
    public CardWindow SpawnWindow(CardBase cardBase)
    {
        //他にカードウィンドウあればそれを破壊する
        if (cardWindow != null)
        {
            cardWindow.WindowDestroy();
        }

        cardWindow = Instantiate(windowPrefab);
        cardWindow.InfoSet(cardBase);
        cardWindow.transform.position = xy;

        return cardWindow;
    }
}
