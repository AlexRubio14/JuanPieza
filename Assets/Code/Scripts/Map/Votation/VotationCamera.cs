using UnityEngine;

public class VotationCamera : MonoBehaviour
{
    [SerializeField] private float newZ;
    [SerializeField] private float newY;

    [Header("Speed")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private bool moveCamera;
    private bool moveXCamera;

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (moveCamera)
        {
            MoveXCamera();
            RotateCamera();
        }
    }

    private void MoveXCamera()
    {
        if(moveXCamera)
        {
            Vector3 targetPosition = new Vector3(ShipsManager.instance.playerShip.transform.position.x, transform.position.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.5f)
            {
                moveXCamera = false;
            }
        }
    }

    private void RotateCamera()
    {
        if (!moveXCamera)
        {
            Ship _ship = ShipsManager.instance.playerShip;
            Vector3 targetPosition = new Vector3(transform.position.x, _ship.GetNewY(), _ship.GetNewZ());

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                moveCamera = false;
                MapManager.Instance.InitVotations();
            }
        }
    }

    public void SetMoveCamera(bool state)
    {
        moveCamera = state;
        moveXCamera = state;
    }
}
