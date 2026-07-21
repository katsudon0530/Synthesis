using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


namespace UI
{
    public class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _image;

        /// <summary>
        ///　クリックされたときに発火するAction
        /// </summary>
        [Header("クリックされたときに発火するメソッドをここにアサインしてください")] 
        [SerializeField]
        private UnityEvent _onClick;
        public UnityEvent Onclick { get => _onClick; set => _onClick = value; }


        [Header("ボタンの有効化")]
        [SerializeField]
        private bool _interactable;

        [FormerlySerializedAs("colorChangeTime")]
        [Header("色が切り替わるための遷移時間")]
        [SerializeField]
        private float _colorChangeTime = 0.5f;

        [Header("ボタンに表示されるテキスト")]
        [TextArea(3, 10)]
        [SerializeField]
        private string _text;
        private TMP_Text _displayText;

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

        public bool Interactable
        {
            get => _interactable;
            set
            {
                if (_interactable == value)
                    return;

                _interactable = value;
                _onPress = false;

                if(_colorCoroutine != null)
                    StopCoroutine(_colorCoroutine);
                _image.color = _interactable ? DefaultColor : DisableColor;
                _displayText.color = _interactable ? DefaultTextColor : DisableTextColor;
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
            _image.color = DefaultColor;
            _displayText = transform.GetComponentInChildren<TMP_Text>();
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

        private void StartChangeColor(Color imageColor, Color textColor)
        {
            if (_colorCoroutine != null)
            {
                StopCoroutine(_colorCoroutine);
            }

            _colorCoroutine = StartCoroutine(ChangeColor(imageColor, textColor));

        }
        /// <summary>
        ///　カーソルが入ってきた時の処理
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            _onCursor = true;

            if(_interactable && !_onPress)
                StartChangeColor(OnCursorColor, OnCursorTextColor);

        }
        /// <summary>
        ///　カーソルが出た時の処理
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            _onCursor = false;

            if (_interactable)
                StartChangeColor(DefaultColor, DefaultTextColor);

        }

        /// <summary>
        ///　クリックされたときの処理
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            if(!_interactable)
                return;

            _onPress = false;

            if (_onCursor)
            {
                StartChangeColor(OnCursorColor, OnCursorTextColor);
                ActionInvoke();
            }
            else
            {
                StartChangeColor(DefaultColor, DefaultTextColor);
            }
        }

        /// <summary>
        ///　クリック中の処理
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_interactable)
                return;

            _onPress = true;
            StopCoroutine(_colorCoroutine);
            _image.color = (OnClickedColor);
            _displayText.color = (OnClickedTextColor);

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

        private void OnDisable()
        {
            _onCursor = false;
            _onPress = false;

            if (_colorCoroutine != null)
            {
                StopCoroutine(_colorCoroutine);
                _colorCoroutine = null;
            }
        }

        private void OnEnable()
        {
            if (_image == null || _displayText == null)
                return;

            _image.color = _interactable ? DefaultColor : DisableColor;
            _displayText.color = _interactable ? DefaultTextColor : DisableTextColor;
        }
    }
}