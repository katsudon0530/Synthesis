using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Image Frame;
    [SerializeField] Image nameFrame;
    [SerializeField] Text nameText;
    [SerializeField] Image icon;
    [SerializeField] Color npColor;
    [SerializeField] Text descriotionText;
    [SerializeField] GameObject hidePanel;
    [SerializeField] GameObject SynthesisPanel;
    [SerializeField] GameObject effectUp;
    [SerializeField] GameObject effectDown;
    GameObject windowGenerator;
    CardWindow ThisWindow;


    public CardBase Base { get; private set; }

    public UnityAction<Card> OnClickCard;

    public Vector3 originalSize;
    public Vector3 originalPosition;

    private void Start()
    {
        windowGenerator = GameObject.FindWithTag("WindowGenerator");
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
        Base.PlayCondition = true;

    }

    //カードクリック時のリアクション先の参照
    public void OnClick()
    {
        OnClickCard?.Invoke(this);
    }
    //カードにをクリックした後の位置補正
    public void PosReset()
    {
        transform.position += Vector3.up * 0.1f;
    }
    //カードにマウスカーソルが入った時の反応
    public void PointerEnter()
    {
        originalSize = transform.localScale;
        originalPosition = transform.position;
        transform.position += Vector3.up * 0.1f;
        transform.localScale = originalSize * 1.1f;
        GetComponentInChildren<Canvas>().sortingLayerName = "overLay";
        ThisWindow = windowGenerator.GetComponent<WindowGenerator>().SpawnWindow(Base);
    }


    //カードからマウスカーソルが出た時のリアクション
    public void PointerExit()
    {
        transform.position -= Vector3.up * 0.1f;
        transform.localScale = originalSize;
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

    void Update()
    {
        if (!Base.PlayCondition)
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
        if (magnification < 1f)
        {
            effectDown.gameObject.SetActive(true);
        }
        else if (magnification > 1f)
        {
            effectUp.gameObject.SetActive(true);
        }
    }
}
