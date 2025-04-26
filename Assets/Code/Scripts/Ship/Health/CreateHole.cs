using UnityEngine;

public class CreateHole : DetectBullet
{
    [Header("Hole")]
    [SerializeField] private GameObject hole;

    [SerializeField]
    private LayerMask floorLayer;

    [SerializeField]
    private LayerMask hitLayer;
    [SerializeField]
    private float holeRadius;

    protected override void DetectCollision(Collision collision, Bullet _bullet)
    {
        base.DetectCollision(collision, _bullet);
        if(_bullet.createHole)
        {
            Vector3 startRayPoint = collision.contacts[0].point + Vector3.up;
            Physics.Raycast(startRayPoint, Vector3.down, out RaycastHit hit, 1.5f, floorLayer);
            GenerateHole(hit.point, _bullet);
            BreakNearbyObjects(hit.point);
        }
    }

    protected void GenerateHole(Vector3 position, Bullet _bullet)
    {
        GameObject _hole = Instantiate(hole);
        _hole.transform.position = position;
        _hole.GetComponentInChildren<RepairHole>().SetbulletInformation(ship, _bullet.GetDamage());
        _hole.transform.SetParent(transform, true);

        if(ship.onDamageRecieved != null)
            ship.onDamageRecieved(_hole);
    }
    private void BreakNearbyObjects(Vector3 _position)
    {
        RaycastHit[] hits = Physics.SphereCastAll(_position, holeRadius, Vector3.forward, 0, hitLayer);

        foreach (RaycastHit hit in hits) 
        {
            if (hit.collider.TryGetComponent(out EnemyObject _enemyObject))
                _enemyObject.BreakObject();
            else if(hit.collider.TryGetComponent(out Repair _objectToRepair))
            {
                _objectToRepair.GetObjectState().SetIsBroke(true);
                _objectToRepair.OnBreakObject();
            }else if (hit.collider.TryGetComponent(out PlayerController player))
            {
                player.PlayerHitted(_position);
            }

        }

    }

}
