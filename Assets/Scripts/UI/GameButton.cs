using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    [SerializeField] GameMaster master;
    [SerializeField] GameUI gameUI;

    [SerializeField] GameObject ExitButton;


    public void OnTitleButton()
    {
        SceneManager.LoadScene("Title");
    }

    public void EnemyButton(int enemyID)
    {
        master.enemyNum = enemyID;
        master.Setup();
    }

    public void ProceedReturnButton(string ProceedReturn)
    {
        gameUI.RuleTextAZ(ProceedReturn);
    }


    public void ResetButton()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

   
    public void OnObjectButton(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void OffObjectButton(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void UndoButton(GameObject button)
    {
        Reaction riaction = button.GetComponent<Reaction>();
        riaction.ButtonReSet();
    }
}