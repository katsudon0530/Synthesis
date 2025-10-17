using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnStartButtan()
    {
        SceneManager.LoadScene("CustomScene");
    }
}
