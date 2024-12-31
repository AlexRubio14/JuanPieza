using UnityEngine;

public class HookController : MonoBehaviour
{
    public bool onWater {  get; private set; }
    public Rigidbody rb {  get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        onWater = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Water"))
        {
            onWater = true;
        }
    }
}
