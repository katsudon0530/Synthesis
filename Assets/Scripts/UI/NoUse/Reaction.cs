using UnityEngine;
using UnityEngine.UI;

public class Reaction : MonoBehaviour
{
    [SerializeField]Color colorOn;
    [SerializeField] bool Chage;
    Image imege;

    //カードにマウスカーソルが入った時のリアクション
    public void ButtonPointerEnter()
    {
        transform.localScale = Vector3.one * 1.1f;
        if (Chage == true)
        {
            ChageColor();
        }
    }

    //カードからマウスカーソルが出た時のリアクション
    public void ButtonPointerExit()
    {
        ButtonReSet();
    }

    //ボタンの状態をもとに戻す
    public void ButtonReSet()
    {
        Image imege;
        transform.localScale = Vector3.one;
        if (Chage == true)
        {
            imege = GetComponent<Image>();
            imege.color = Color.white;
        }
    }
    
    //色を変化させる
    public void ChageColor()
    {
        imege = GetComponent<Image>();
        imege.color = colorOn;
    }
}
