using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // load Car Selection scene
    public void OnPlayClicked()
    {
        SceneManager.LoadScene(5);
    }

    // load Test Menu scene
    public void OnTestClicked()
    {
        SceneManager.LoadScene(4);
    }

    // load Learn Menu scene
    public void OnLearnClicked()
    {
        SceneManager.LoadScene(7);
    }
}
