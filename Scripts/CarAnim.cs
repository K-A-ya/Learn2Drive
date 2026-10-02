using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnim : MonoBehaviour
{
    // idk if i have to use htis?
    [SerializeField] private Vector3 finalPosition;
    private Vector3 intialPosition;

    private void Awake()
    {
        intialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, 0.1f);

    }

    private void OnDisable()
    {
        transform.position = intialPosition;
    }
}
