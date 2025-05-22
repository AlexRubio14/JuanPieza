using UnityEngine;

public class Geyser : MonoBehaviour
{
    [SerializeField] private Vector3 geyserForce;
    [SerializeField] private ParticleSystem geyserParticles;
    [SerializeField] private LayerMask affectedLayers;

    [SerializeField] private float raycastRadius;
    [SerializeField] private float raycastDistance;
    private Vector3 raycastSize;

    private ParticleSystem currentParticles;
    private bool isRunning = false;

    private void OnEnable()
    {
        currentParticles = Instantiate(geyserParticles, transform);
        currentParticles.Play();
        isRunning = true;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        //if (currentParticles.IsAlive(false))
        //{
        //    Destroy(currentParticles.gameObject);
        //    isRunning = false;
        //    gameObject.SetActive(false);
        //}

        raycastSize = new Vector3(transform.position.x, transform.position.y + raycastDistance, transform.position.z);
        Collider[] colliders = Physics.OverlapCapsule(transform.position, raycastSize, raycastRadius, affectedLayers);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.attachedRigidbody;
            if (rb != null)
            {
                rb.AddForce(geyserForce * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.cyan;

    //    Vector3 point1 = transform.position;
    //    Vector3 point2 = raycastSize;

    //    DrawVerticalCapsule(point1, point2, raycastRadius);
    //}

    //private void DrawVerticalCapsule(Vector3 p1, Vector3 p2, float radius)
    //{
    //    // Dibuja la línea central
    //    Gizmos.DrawLine(p1 + Vector3.left * radius, p2 + Vector3.left * radius);
    //    Gizmos.DrawLine(p1 + Vector3.right * radius, p2 + Vector3.right * radius);
    //    Gizmos.DrawLine(p1 + Vector3.forward * radius, p2 + Vector3.forward * radius);
    //    Gizmos.DrawLine(p1 + Vector3.back * radius, p2 + Vector3.back * radius);

    //    // Dibuja las esferas en los extremos
    //    Gizmos.DrawWireSphere(p1, radius);
    //    Gizmos.DrawWireSphere(p2, radius);
    //}


}
