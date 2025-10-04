using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{

   
    public void OnObjectButton(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void OffObjectButton(GameObject obj)
    {
        obj.SetActive(false);
    }
}