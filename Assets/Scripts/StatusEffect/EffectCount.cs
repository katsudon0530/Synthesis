using System.Collections.Generic;
using UnityEngine;

public class EffectCount : MonoBehaviour
{
    public void StatusEffectCount(List<StatusEffect> Effects, Vector2 xy)
    {
        Effects.RemoveAll(a => a == null);
        if (Effects.Count == 0)
            return;
        Vector2 firstEffect = Effects[0].transform.localPosition ;

        for (int i = Effects.Count - 1; i >= 0; i--)
        {
            Effects[i].EffectCountDown(Effects);
        }

        alignment(Effects, firstEffect);
    }

    public void alignment(List<StatusEffect> statusEffects, Vector2 xy)
    {
        for (int i = 0; i < statusEffects.Count; i++)
        {
            if (statusEffects[i] == null) continue;

            float posX = xy.x + i * 0.8f;
            statusEffects[i].transform.localPosition = new Vector2(posX, xy.y);

        }
    }
}
