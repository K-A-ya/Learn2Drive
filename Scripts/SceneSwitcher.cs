using UnityEngine;
using UnityEngine.SceneManagement;

public class STart : MonoBehaviour
{
    public void Log()
    {
        SceneManager.LoadScene("Login_Menu");
    }

    public void Reg()
    {
        SceneManager.LoadScene("Register_Menu");
    }
}
