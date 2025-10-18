using UI;
using UnityEngine;

public class GameStateChangeButton : MonoBehaviour
{
    [SerializeField] private GameState nextState; 
    [SerializeField] private ButtonUI button;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<ButtonUI>();

        button.Onclick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        GameManager.ChangeState(nextState);
    }
}