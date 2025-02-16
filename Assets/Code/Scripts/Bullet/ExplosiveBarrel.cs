using UnityEngine;

public class ExplosiveBarrel : Bullet
{
    [SerializeField] private Vector2 explosionForce;

    [SerializeField] private LayerMask pushLayers;

    private void OnDestroy()
    {
        Explode();
    }
    private void Explode()
    {
        Debug.Log("EXPLODED");
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRadius, transform.forward,
            1f, pushLayers);  

        foreach (RaycastHit hit in hits)
        {
            Debug.Log("HITED");
            if (hit.collider && gameObject != hit.collider.gameObject)
            {
                Debug.Log(hit.collider.gameObject.name);
                Vector3 hitDirection = hit.collider.transform.position - transform.position;
                hit.collider.GetComponent<EnemyController>().Knockback(explosionForce, hitDirection.normalized);
            }
        }
    }
}
