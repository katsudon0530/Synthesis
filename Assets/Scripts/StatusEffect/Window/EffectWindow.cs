using UnityEngine;
using UnityEngine.UI;

public class EffectWindow : MonoBehaviour
{
    [SerializeField] Image windowFrame;
    [SerializeField] Image iconFrame;
    [SerializeField] Text effectName;
    [SerializeField] Text effectText;
    [SerializeField] Text effectCount;
    [SerializeField] Text effectGrants;
    [SerializeField] Image icon;


    public void InfoSet(StatusEffect effect)
    {
        effectName.text = $"状態:{effect.Base.DisplayName}";

        effectText.text = effect.Base.Description;
        icon.sprite = effect.Base.Icon;
        effectCount.text = $"残り:{effect.CountTurn}ターン";
        effectGrants.text = $"付与回数:{effect.CountGrant}回";

    }


    public void WindowDestroy()
    {
        Destroy(this.gameObject);
    }
}
