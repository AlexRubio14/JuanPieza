using UnityEngine;

public class CreateHole : DetectBullet
{
    [Header("Hole")]
    [SerializeField] private GameObject hole;
    protected override void DetectCollision(Collision collision)
    {
        base.DetectCollision(collision);
        GenerateHole(collision.contacts[0].point);
    }

    protected void GenerateHole(Vector3 position)
    {
        GameObject _hole = Instantiate(hole);
        _hole.transform.position = position;
        _hole.GetComponent<Hole>().SetShipInformation(ship);
        _hole.GetComponentInChildren<Repair>().SetbulletInformation(ship, bullet.GetDamage());
        _hole.transform.SetParent(transform, true);
    }
}
