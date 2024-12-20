using UnityEngine;

public class DetectBullet : MonoBehaviour
{
    [Header("ShipInformation")]
    [SerializeField] private ShipHealth shipHealth;

    [Header("Hole")]
    [SerializeField] private GameObject hole;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            shipHealth.SetCurrentHealth(-other.gameObject.GetComponent<Bullet>().GetDamage());
            CreateHole(other.gameObject.transform);
            Destroy(other.gameObject);
        }
    }

    private void CreateHole(Transform _tranform)
    {
        GameObject _hole = Instantiate(hole);
        _hole.transform.position = new Vector3(_tranform.position.x, transform.position.y, _tranform.position.z);   
        _hole.transform.SetParent(this.transform, true);
    }
}
