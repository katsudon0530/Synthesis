using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

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

    private GraphicRaycaster _raycaster;
    public CardBase Base { get; private set; }

    public UnityAction<Card> OnClickCard;

    public Vector3 originalSize;
    public Vector3 originalPosition;

    /// <summary>
    ///　キーがボタンオブジェクト内にあるかどうかのフラグ
    /// </summary>
    private bool _onCursor;

    /// <summary>
    ///　キーが押されているかどうかのフラグ
    /// </summary>
    private bool _onPress = false;

    private bool OnCursor
    {
        get => _onCursor;
        set
        {
            if (_onCursor != value) // カーソルがボタン上にあるか判定しているboolの値が切り替わったとき
            {
                if (value && _onPress)
                {
                    _onCursor = false; //ボタンを押しながら入ると変更しない
                }
                else
                {
                    _onCursor = value; // 変更を適用
                }

                if (_onCursor)
                {
                    PointerEnter();
                }
                else if (!_onCursor)
                {
                    PointerExit();
                }
            
            }
        }
    }

    private void Start()
    {
        windowGenerator = GameObject.FindWithTag("WindowGenerator");
        _raycaster = GetComponentInChildren<Canvas>().GetComponent<GraphicRaycaster>();
        if (_raycaster == null)
            Debug.LogError("Canvas に GraphicRaycaster が必要です！");
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



    void Update()
    {
        OnMouseCursor();
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


    private void OnMouseCursor()
    {
        PointerEventData pointerData
            = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(pointerData, results);

        OnCursor = CheckCursor(results);
    }
    /// <summary>
    ///　マウスカーソルがボタン上にあるかどうかを判定する
    /// </summary>
    private bool CheckCursor(List<RaycastResult> results)
    {

        return results.Count > 0 && results[0].gameObject.name == "Panel";
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
