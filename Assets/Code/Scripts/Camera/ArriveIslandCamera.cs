using UnityEngine;

public class ArriveIslandCamera : MonoBehaviour
{
    private bool moveCamera;

    private void Update()
    {
        if(moveCamera && ShipsManager.instance.playerShip != null)
        {
            transform.position = new Vector3(0, ShipsManager.instance.playerShip.GetNewY(), ShipsManager.instance.playerShip.gameObject.transform.position.z + ShipsManager.instance.playerShip.GetNewZ());
            ShipsManager.instance.playerShip.GetComponent<ShipCurve>().SetStartMovementToIsland(true);
        }

    }

    public void SetMoveCamera(bool state)
    {
        moveCamera = state;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
