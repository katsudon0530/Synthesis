using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardWindow : MonoBehaviour
{
    [SerializeField] Image WindowFrame;
    [SerializeField] TMP_Text CardName;
    [SerializeField] TMP_Text CardType;
    [SerializeField] TMP_Text CardText;
    [SerializeField] Image CardImage;


    public void InfoSet(CardBase cardBase)
    {
        WindowFrame.color = cardBase.Color;
        CardName.text = $"カード名:{cardBase.DisplayName}";

        switch (cardBase.SynthesisType)
        {
            case SynthesisType.Normal:
                CardType.text = "基礎カード";
                break;
            case SynthesisType.Plus:
                CardType.text = "2枚合成カード";
                break;
            case SynthesisType.DoublePlus:
                CardType.text = "3枚合成カード";
                break;
        }

        CardText.text = cardBase.Explanation;
        CardImage.sprite = cardBase.Icon;

    }

    public void WindowDestroy()
    {
        Destroy(this.gameObject);
    }
}
