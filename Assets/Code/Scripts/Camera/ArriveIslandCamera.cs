using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ArriveIslandCamera : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 initPosition;
    private Vector3 startPosition;
    private float t;

    private bool isArriving;
    private float startZ;
    private float endZ;
    private void Start()
    {
        InitValues();
    }

    private void InitValues()
    {
        initPosition = transform.position;

        GetComponent<CameraController>().enabled = false;
        transform.position = ShipsManager.instance.playerShip.cameraInitPosition;
        transform.rotation = Quaternion.identity;

        startZ = transform.position.z;
        endZ = (startZ - ShipsManager.instance.playerShip.startZPosition) / 2;

        isArriving = true;
        DisablePlayers();
    }

    private void Update()
    {
        if (isArriving)
            FollowShip();
        else
            ResetCamPosition();
    }

    private void FollowShip()
    {
        float newZ = Mathf.Lerp(startZ, endZ, ShipsManager.instance.playerShip.t);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
        if (ShipsManager.instance.playerShip.t >= 1)
        {
            startPosition = transform.position;
            isArriving = false;
        }

    }

    private void ResetCamPosition()
    {
        t += Time.deltaTime * speed;

        Vector3 newPosition = Vector3.Lerp(startPosition, initPosition, t);
        transform.position = newPosition;

        transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(55, 0, 0), t);

        if(t >= 1)
        {
            GetComponent<CameraController>().enabled = true;
            ShipsManager.instance.GenerateEnemies();
            ActivePlayers();
            enabled = false;
        }
    }

    private void DisablePlayers()
    {
        foreach ((PlayerInput, SinglePlayerController) item in PlayersManager.instance.players)
            item.Item1.SwitchCurrentActionMap("Camera");
    }

    private void ActivePlayers()
    {
        foreach ((PlayerInput, SinglePlayerController) item in PlayersManager.instance.players)
            item.Item1.SwitchCurrentActionMap("Gameplay");
    }
}
