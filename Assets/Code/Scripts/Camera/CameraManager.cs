using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private CameraController simpleCamera;
    private VotationCamera sailCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        SetScripts();
    }

    public void SetSailCamera(bool state)
    {
        sailCamera.enabled = state;
        sailCamera.SetMoveCamera(state);
    }

    public void SetSimpleCamera(bool state)
    {
        simpleCamera.enabled = state;
    }

    private void SetScripts()
    {
        if (simpleCamera == null)
        {
            simpleCamera = Camera.main.GetComponent<CameraController>();
            sailCamera = Camera.main.GetComponent<VotationCamera>();
        }

    }
}
