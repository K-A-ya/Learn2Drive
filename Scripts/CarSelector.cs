using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarSelector : MonoBehaviour
{
    // note: make sure that methods work accros scenes
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button selectButton; // Button to confirm car selection

    private int currentCar;

    private void Awake()
    {
        int savedCar = PlayerPrefs.GetInt("SelectedCar", 0); // Load previous selection
        SelectCar(savedCar);
    }

    private void SelectCar(int _index)
    {
        previousButton.interactable = (_index != 0);
        nextButton.interactable = (_index != transform.childCount - 1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }

        currentCar = _index;
    }

    public void ChangeCar(int _change)
    {
        currentCar += _change;
        SelectCar(currentCar);
    }

    public void ConfirmCarSelection()
    {
        PlayerPrefs.SetInt("SelectedCar", currentCar); // Save selected car
        PlayerPrefs.Save();
        SceneManager.LoadScene("Map Selection"); // Load the Map Selection Scene
    }
}
