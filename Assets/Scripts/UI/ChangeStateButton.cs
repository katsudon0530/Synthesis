using UI;
using UnityEngine;
using UnityEngine.UI;

public class StateChangeButton : MonoBehaviour
{
    [SerializeField] private TurnState nextState; 
    [SerializeField] private ButtonUI button;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<ButtonUI>();

        button.Onclick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        GameMaster.ChangeState(nextState);
    }
}