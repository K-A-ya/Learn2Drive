using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public Transform carParent; // Parent GameObject containing all car models

    private void Start()
    {
        int selectedCar = PlayerPrefs.GetInt("SelectedCar", 0); // Load selected car

        for (int i = 0; i < carParent.childCount; i++)
        {
            carParent.GetChild(i).gameObject.SetActive(i == selectedCar);
        }
    }
}
