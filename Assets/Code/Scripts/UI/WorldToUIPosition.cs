using System;
using UnityEngine;

public class WorldToUIPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0);
    [SerializeField] private RectTransform uiElement;

    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
        

        if (uiElement == null)
        {
            enabled = false;
        }

        if (target == null)
        {
            enabled = false; 
        }
    }

    private void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position + offset);

        uiElement.position = screenPosition;
    }
}
