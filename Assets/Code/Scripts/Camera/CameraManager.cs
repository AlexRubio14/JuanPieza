using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private CameraController simpleCamera;
    private VotationCamera sailCamera;
    private ArriveIslandCamera arriveCamera;

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

    private void Start()
    {
        simpleCamera = Camera.main.GetComponent<CameraController>();
        sailCamera = Camera.main.GetComponent<VotationCamera>();
        arriveCamera = Camera.main.GetComponent<ArriveIslandCamera>();
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

    public void SetArriveCamera(bool state)
    {
        arriveCamera.enabled = state;
        arriveCamera.SetMoveCamera(state);
    }
}
