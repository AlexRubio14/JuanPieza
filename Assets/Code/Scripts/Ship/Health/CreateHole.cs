using UnityEngine;

public class CreateHole : DetectBullet
{
    [Header("Hole")]
    [SerializeField] private GameObject hole;

    [SerializeField]
    private LayerMask objectLayer;
    [SerializeField]
    private float holeRadius = 2;
    protected override void DetectCollision(Collision collision)
    {
        base.DetectCollision(collision);
        GenerateHole(collision.contacts[0].point);
        BreakNearbyObjects(collision.contacts[0].point);
    }

    protected void GenerateHole(Vector3 position)
    {
        GameObject _hole = Instantiate(hole);
        _hole.transform.position = position;
        _hole.GetComponent<Hole>().SetShipInformation(ship);
        _hole.GetComponentInChildren<RepairHole>().SetbulletInformation(ship, bullet.GetDamage());
        _hole.transform.SetParent(transform, true);

        if(ship.onDamageRecieved != null)
            ship.onDamageRecieved(_hole);
    }
    private void BreakNearbyObjects(Vector3 _position)
    {
        RaycastHit[] hits = Physics.SphereCastAll(_position, holeRadius, Vector3.forward, 0, objectLayer);

        foreach (RaycastHit hit in hits) 
        {
            if (hit.collider.TryGetComponent(out EnemyObject _enemyObject))
                _enemyObject.BreakObject();
            else if(hit.collider.TryGetComponent(out Repair _objectToRepair))
                _objectToRepair.GetObjectState().SetIsBroke(true);

        }

    }

}
