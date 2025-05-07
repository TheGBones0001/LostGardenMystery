using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string nextSceneName = "GameplayScene"; // Nome da cena que virá depois
    public GameObject panelMainMenu;
    public void Jogar()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void Sair()
    {
        Application.Quit();
    }
}
