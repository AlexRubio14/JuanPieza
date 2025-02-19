using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ArriveIslandCamera : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Quaternion targetRotation;
    private bool moveCamera;


    private void Start()
    {
        if(MapManager.Instance.GetCurrentLevel().nodeType != NodeData.NodeType.BATTLE)
            transform.position = new Vector3(0, ShipsManager.instance.playerShip.GetNewY(), ShipsManager.instance.playerShip.gameObject.transform.position.z + ShipsManager.instance.playerShip.GetNewZ());
    }

    private void FixedUpdate()
    {
        if(moveCamera && ShipsManager.instance.playerShip != null)
        {
            if(!ShipsManager.instance.playerShip.GetComponent<ShipCurve>().GetStartMovementToIsland())
                ShipsManager.instance.playerShip.GetComponent<ShipCurve>().SetStartMovementToIsland(true);
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, ShipsManager.instance.playerShip.GetComponent<ShipCurve>().GetStep());
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, ShipsManager.instance.playerShip.GetComponent<ShipCurve>().GetStep());
    }

    public void SetMoveCamera(bool state)
    {
        moveCamera = state;
        if(state)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
