using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("JotunheimScene");
    }

    public void Sair()
    {
        Application.Quit();
    }
}