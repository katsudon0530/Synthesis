using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    [SerializeField] Image Frame;
    [SerializeField] Image nameFrame;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image icon;
    [SerializeField] Color npColor;
    [SerializeField] TMP_Text descriotionText;
    [SerializeField] GameObject SynthesisPanel;
    [SerializeField] GameObject effectUp;
    [SerializeField] GameObject effectDown;
    GameObject windowGenerator;
    CardWindow ThisWindow;

    private RectTransform _rect;
    public CardBase Base { get; private set; }
    public string InstanceId { get; private set; } = System.Guid.NewGuid().ToString();
    public bool PlayCondition { get; set; }

    public UnityAction<Card> OnClickCard;

    private Vector2 originalSize;
    private Vector2 originalPosition;


    private void Start()
    {
        windowGenerator = GameObject.FindWithTag("WindowGenerator");
        _rect = GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
        if (_rect == null)
            Debug.LogError("Canvas に RectTransform が必要です！");

        originalSize = _rect.sizeDelta;
        originalPosition = _rect.anchoredPosition;
    }
    //カード内容の定義
    public void Set(CardBase cardBase)
    {
        Base = cardBase;
        nameText.text = cardBase.DisplayName;
        icon.sprite = cardBase.Icon;
        descriotionText.text = cardBase.Description;
        nameFrame.color = cardBase.Color;
        Frame.color = cardBase.Color;
        SynthesisPanel.SetActive(false);
        PlayCondition = true;
        effectReSet();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverOn();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HoverOff();
    }

    //カードクリック時のリアクション先の参照
    public void OnClick()
    {
        OnClickCard?.Invoke(this);
    }


    void Update()
    {
        if (!PlayCondition)
        {
            Frame.color = npColor;
            nameFrame.color = npColor;
        }
        else
        {
            Frame.color = Base.Color;
            nameFrame.color = Base.Color;
        }
    }

    //エフェクト表示を消す
    public void effectReSet()
    {
        effectUp.SetActive(false);
        effectDown.SetActive(false);
    }

    //どちらかのエフェクトを表示
    public void effectSet(float magnification)
    {
        if (magnification == 1f)
        {
            effectReSet();
        }
        else if (magnification < 1f)
        {
            effectDown.gameObject.SetActive(true);
        }
        else if (magnification > 1f)
        {
            effectUp.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// カード選択中のホバー
    /// </summary>
    public void HoverOn()
    {
        _rect.anchoredPosition += new Vector2(0, 0.1f);
        _rect.sizeDelta *= 1.1f;
        GetComponentInChildren<Canvas>().sortingLayerName = "overLay";
        ThisWindow = windowGenerator.GetComponent<WindowGenerator>().SpawnWindow(Base);
    }

    /// <summary>
    /// カード選択中のホバー
    /// </summary>
    public void HoverOff()
    {
        _rect.anchoredPosition = originalPosition;
        _rect.sizeDelta = originalSize;
        GetComponentInChildren<Canvas>().sortingLayerName = "Default";
        if (ThisWindow != null)
            Destroy(ThisWindow.gameObject);
    }

    //カードのハイドパネルを非表示にする
    public void Open()
    {
        SynthesisPanel.SetActive(false);
    }

    public void Close()
    {
        SynthesisPanel.SetActive(true);
    }
}
