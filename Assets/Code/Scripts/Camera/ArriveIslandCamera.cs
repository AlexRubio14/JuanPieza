using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ArriveIslandCamera : MonoBehaviour
{
    public enum CameraBehaivour { WAIT, ARRIVING, ROTATING, REPOSITING, INVERSE_ROTATING };

    [SerializeField] private float speed;

    public CameraBehaivour behaivour;

    private Vector3 initPosition;
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private float t;

    private float startZ;
    private float endZ;

    private void Start()
    {
        InitValues();
        behaivour = CameraBehaivour.WAIT;
    }

    private void InitValues()
    {
        initPosition = transform.position;

        GetComponent<CameraController>().enabled = false;
        transform.position = ShipsManager.instance.playerShip.cameraInitPosition;
        transform.rotation = Quaternion.identity;

        startZ = transform.position.z;
        endZ = (startZ - ShipsManager.instance.playerShip.startZPosition) / 2;

        DisablePlayers();
    }

    private void Update()
    {
        switch (behaivour)
        {
            case CameraBehaivour.ARRIVING:
                FollowShip();
                break;
            case CameraBehaivour.ROTATING:
                ResetCamPosition();
                break;
            case CameraBehaivour.REPOSITING:
                RepositingCamPosition();
                break;
            case CameraBehaivour.INVERSE_ROTATING:
                EndCameraPosition();
                break;
        }
    }

    private void FollowShip()
    {
        float newZ = Mathf.Lerp(startZ, endZ, ShipsManager.instance.playerShip.t);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
        if (ShipsManager.instance.playerShip.t >= 1)
        {
            startPosition = transform.position;
            behaivour = CameraBehaivour.ROTATING;
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

    private void RepositingCamPosition()
    {
        t += Time.deltaTime * speed;

        Vector3 newPosition = Vector3.Lerp(currentPosition, initPosition, t);
        transform.position = newPosition;

        if (t >= 1)
        {
            t = 0;
            behaivour = CameraBehaivour.INVERSE_ROTATING;
        }
    }

    private void EndCameraPosition()
    {
        t += Time.deltaTime * speed;

        Vector3 newPosition = Vector3.Lerp(initPosition, startPosition, t); 
        transform.position = newPosition;

        transform.rotation = Quaternion.Slerp(Quaternion.Euler(55, 0, 0), Quaternion.identity, t);

        if (t >= 1)
        {
            DisablePlayers();
            ShipsManager.instance.playerShip.SetLeaving(true);
            enabled = false;
        }
    }

    private void DisablePlayers()
    {
        foreach ((PlayerInput, SinglePlayerController) item in PlayersManager.instance.players)
            item.Item1.SwitchCurrentActionMap("Camera");

        ShipsManager.instance.playerShip.gameObject.transform.Find("Script").gameObject.SetActive(false);
    }

    private void ActivePlayers()
    {
        foreach ((PlayerInput, SinglePlayerController) item in PlayersManager.instance.players)
            item.Item1.SwitchCurrentActionMap("Gameplay");

        ShipsManager.instance.playerShip.gameObject.transform.Find("Script").gameObject.SetActive(true);
    }

    public void SetIsArriving()
    {
        behaivour = CameraBehaivour.ARRIVING;
    }

    public void SetIsRepositing()
    {
        t = 0;
        GetComponent<CameraController>().enabled = false;
        currentPosition = transform.position;
        behaivour = CameraBehaivour.REPOSITING;
    }
}
