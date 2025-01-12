using UnityEngine;

public class CreateHole : DetectBullet
{
    [Header("Hole")]
    [SerializeField] private GameObject hole;

    [SerializeField]
    private LayerMask objectLayer;
    [SerializeField]
    private float holeRadius = 2;
    protected override void DetectCollision(Collision collision, Bullet _bullet)
    {
        base.DetectCollision(collision, _bullet);
        GenerateHole(collision.contacts[0].point, _bullet);
        BreakNearbyObjects(collision.contacts[0].point);
    }

    protected void GenerateHole(Vector3 position, Bullet _bullet)
    {
        GameObject _hole = Instantiate(hole);
        _hole.transform.position = position;
        _hole.GetComponent<Hole>().SetShipInformation(ship);
        _hole.GetComponentInChildren<RepairHole>().SetbulletInformation(ship, _bullet.GetDamage());
        _hole.transform.SetParent(transform, true);

        Instantiate(_bullet.hitParticles , position, Quaternion.identity);


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
            {
                _objectToRepair.GetObjectState().SetIsBroke(true);
                _objectToRepair.OnBreakObject();
            }
        }

    }

}
