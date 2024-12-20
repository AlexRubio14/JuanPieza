using UnityEngine;

public class DetectBullet : MonoBehaviour
{
    [Header("ShipInformation")]
    [SerializeField] private ShipHealth shipHealth;

    [Header("Hole")]
    [SerializeField] private GameObject hole;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            shipHealth.SetCurrentHealth(-collision.gameObject.GetComponent<Bullet>().GetDamage());
            CreateHole(collision.contacts[0].point);
            Destroy(collision.gameObject);
        }
    }

    private void CreateHole(Vector3 position)
    {
        GameObject _hole = Instantiate(hole);
        _hole.transform.position = position;
        _hole.transform.SetParent(this.transform, true);
    }
}
