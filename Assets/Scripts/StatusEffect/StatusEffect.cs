using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusEffect : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image Frame;
    GameObject windowGenerator;
    EffectWindow thisWindow;
    int countTurn;
    int countGrant;

    public StatusEffectBase Base { get; private set; }
    public int CountTurn { get => countTurn; set => countTurn = value; }
    public int CountGrant { get => countGrant; set => countGrant = value; }


    //エフェクトの情報をセット
    public void SetEffect(StatusEffectBase effectBase)
    {
        Base = effectBase;
        icon.sprite = effectBase.Icon;
        countTurn = effectBase.EffectCount;
        CountGrant = 1;
        windowGenerator = GameObject.FindWithTag("WindowGenerator");
    }

    //エフェクトの効果期限
    public void EffectCountDown(List<StatusEffect> effects)
    {
        countTurn = countTurn - 1;
        if (countTurn <= 0)
        {
            Destroy(this.gameObject);
            effects.Remove(this);
        }
    }

    //カードにマウスカーソルが入った時の反応
    public void PointerEnter()
    {
        thisWindow = windowGenerator.GetComponent<EffectWindowGenerator>().SpawnWindow(this);
    }

    //カードからマウスカーソルが出た時のリアクション
    public void PointerExit()
    {

        Destroy(thisWindow.gameObject);
    }

}
