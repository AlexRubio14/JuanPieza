using UnityEngine;

public class ObjectDetectBullet : DetectBullet
{
    protected override void DetectCollision(Collision collision)
    {
        base.DetectCollision(collision);
        SetInformation();
    }

    protected void SetInformation()
    {
        GetComponent<Repair>().SetbulletInformation(ship, bullet.GetDamage());
    }

}
