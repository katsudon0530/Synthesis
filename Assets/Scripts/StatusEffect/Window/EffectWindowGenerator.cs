using UnityEngine;

public class EffectWindowGenerator : MonoBehaviour
{
    [SerializeField] EffectWindow effectWindowPrefab;
    [SerializeField] Vector2 effectXY;

    EffectWindow effectWindow = null;
    public EffectWindow SpawnWindow(StatusEffect effect)
    {
        //他にカードウィンドウあればそれを破壊する
        if (effectWindow != null)
        {
            effectWindow.WindowDestroy();
        }

        effectWindow = Instantiate(effectWindowPrefab);
        effectWindow.InfoSet(effect);
        effectWindow.transform.position = effectXY;

        return effectWindow;
    }

}
