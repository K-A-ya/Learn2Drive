using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelector : MonoBehaviour
{
    public void SelectMap(string mapSceneName)
    {
        PlayerPrefs.SetString("SelectedMap", mapSceneName); // Save selected map name
        PlayerPrefs.Save();
        SceneManager.LoadScene(mapSceneName); // Load the selected map scene
    }
}
