using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


namespace UI
{
    public class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Image _image;

        /// <summary>
        ///　クリックされたときに発火するAction
        /// </summary>
        [Header("クリックされたときに発火するメソッドをここにアサインしてください")] 
        [SerializeField]
        private UnityEvent _onClick;
        public UnityEvent Onclick { get => _onClick; set => _onClick = value; }

        private GraphicRaycaster _raycaster;

        [Header("ボタンの有効化")]
        [SerializeField]
        public bool _interactable;

        [FormerlySerializedAs("colorChangeTime")]
        [Header("色が切り替わるための遷移時間")]
        [SerializeField]
        private float _colorChangeTime = 0.5f;

        [Header("ボタンに表示されるテキスト")]
        [TextArea(3, 10)]
        [SerializeField]
        private string _text;
        private Text _displayText;

        #region ImageColorSettings

        [Header("ボタン画像の色設定")]

        [Header("デフォルトの色")] public Color DefaultColor;

        [Header("カーソルがボタン上にある時の色")] public　Color OnCursorColor;

        [Header("クリック時の色")] public　Color OnClickedColor;

        [Header("非有効化時の色")] public Color DisableColor;

        #endregion

        [Space(20)]

        #region TextColorSettings

        [Header("テキストの色設定")]

        [Header("デフォルトの色")] public Color DefaultTextColor;

        [Header("カーソルがボタン上にある時の色")] public Color OnCursorTextColor;

        [Header("クリック時の色")] public Color OnClickedTextColor;

        [Header("非有効化時の色")] public Color DisableTextColor;

        #endregion

        /// <summary>
        ///　キーがボタンオブジェクト内にあるかどうかのフラグ
        /// </summary>
        private bool _onCursor;

        /// <summary>
        ///　キーが押されているかどうかのフラグ
        /// </summary>
        private bool _onPress;

        /// <summary>
        ///　遷移中のコルーチンがこの変数に都度保存される
        /// </summary>
        private Coroutine _colorCoroutine;

        private bool OnCursor
        {
            get => _onCursor;
            set
            {
                if (_onCursor != value)　// カーソルがボタン上にあるか判定しているboolの値が切り替わったとき
                {
                    if (value && _onPress)
                    {
                        _onCursor = false; //ボタンを押しながら入ると変更しない
                    }
                    else
                    {
                        _onCursor = value;　// 変更を適用
                    }

                    if (_interactable)
                    {
                        if (_colorCoroutine != null)
                        {
                            StopCoroutine(_colorCoroutine);
                            // 色の変更の遷移中であるときに_onCursorが切り替わった時に現在の遷移を止める　
                        }

                        //色の切り替え開始　遷移中のコルーチンを変数に保存(遷移中であるかどうかの判定のため)
                        if (_onCursor)
                        {
                            _colorCoroutine = StartCoroutine(ChangeColor(OnCursorColor, OnCursorTextColor));
                        }
                        else if (!_onCursor)
                        {
                            _colorCoroutine = StartCoroutine(ChangeColor(DefaultColor, DefaultTextColor));
                        }
                    }
                }
            }
        }

        public bool Interactable
        {
            get => _interactable;
            set
            {
                if (_interactable != value)
                {
                    _interactable = value; // 変更を適用

                    //フラグに応じて色を変更する
                    _image.color = (_interactable ? DefaultColor : DisableColor);
                    _displayText.color = (_interactable ? DefaultTextColor : DisableTextColor);
                }
            }
        }

        private void Awake()
        {
            Init();
        }

        /// <summary>
        ///　初期化処理
        /// </summary>
        private void Init()
        {
            _image = GetComponent<Image>();
            _raycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
            if (_raycaster == null)
                Debug.LogError("Canvas に GraphicRaycaster が必要です！");
            _image.color = DefaultColor;
            //_onClick.AddListener(ForDebug);
            _displayText = transform.GetComponentInChildren<Text>();
            if (_displayText == null)
            {
                Debug.LogWarning("Textコンポーネントが見つかりません");
            }
            else
            {
                _displayText.text = _text;
                _displayText.raycastTarget = false;
            }
        }

        void ForDebug()
        {
            Debug.Log($"{gameObject.name}がクリックされました");
        }

        private void Update()
        {
            OnMouseCursor();
        }

        /// <summary>
        ///　クリックされたときの処理
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            _onPress = false;
            if (_interactable && _onCursor)
            {
                if (_colorCoroutine != null)
                {
                    StopCoroutine(_colorCoroutine);
                    // 色の変更の遷移中であるときに_onCursorが切り替わった時に現在の遷移を止める　
                }

                _colorCoroutine = StartCoroutine(ChangeColor(OnCursorColor, OnCursorTextColor));
                ActionInvoke();
            }
        }

        /// <summary>
        ///　クリック中の処理
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_interactable)
            {
                _onPress = true;
                _image.color = (_onCursor ? OnClickedColor : DefaultColor);
                _displayText.color = (_onCursor ? OnClickedTextColor : DefaultTextColor);
            }

        }

        private void ActionInvoke()
        {
            if (_onClick == null)
            {
                Debug.Log("クリック時の処理が一つも登録されていません");
            }
            else
            {
                _onClick.Invoke();
                //　Actionを発火（Actionの中身がnullの場合を考慮）
            }
        }

        /// <summary>
        ///　マウスカーソルがボタン上にあれば真を入れる
        /// </summary>
        private void OnMouseCursor()
        {
            PointerEventData pointerData
                = new PointerEventData(EventSystem.current){ position = Input.mousePosition};
            List<RaycastResult> results = new List<RaycastResult>();
            _raycaster.Raycast(pointerData, results);

            OnCursor = CheckCursor(results);
        }

        /// <summary>
        ///　マウスカーソルがボタン上にあるかどうかを判定する
        /// </summary>
        private bool CheckCursor(List<RaycastResult> results)
        {

            return results.Count > 0 && results[0].gameObject == gameObject;
        }

        /// <summary>
        ///　色を徐々に変化させる
        /// </summary>
        IEnumerator ChangeColor(Color targetImageColor, Color targetTextColor)
        {
            Color startColor1 = _image.color;　//Colorの初期値を保存
            Color startColor2 = _displayText.color;　//Colorの初期値を保存
            float elapsedTime = 0;　// 現在の遷移時間

            while (elapsedTime < _colorChangeTime)　// 決められた遷移時間になるまで繰り返す
            {
                elapsedTime += Time.deltaTime;　// 遷移時間を計算していく 

                //色を徐々にtargetColorに切り替える（Lerpの第三引数は0~1の間で遷移するように）
                _image.color =
                    Color.Lerp(startColor1, targetImageColor,
                        elapsedTime / _colorChangeTime); 
                _displayText.color =
                    Color.Lerp(startColor2, targetTextColor,
                        elapsedTime / _colorChangeTime);
                yield return null;
            }

            _image.color = targetImageColor;
            _displayText.color = targetTextColor;

            _colorCoroutine = null;　// 遷移が終わったので中身を空にする
        }
    }
}