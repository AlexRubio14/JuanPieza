using UnityEngine;

public class ObjectDetectBullet : DetectBullet
{
    [Header("Sibling")]
    [SerializeField] private GameObject brokenSibling;
    protected override void DetectCollision(Collision collision)
    {
        base.DetectCollision(collision);
        SetInformation();
    }

    protected void SetInformation()
    {
        brokenSibling.SetActive(true);
        brokenSibling.GetComponent<Repair>().SetbulletInformation(ship, bullet.GetDamage());
        gameObject.SetActive(false);
    }

}
