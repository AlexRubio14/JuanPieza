using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class HookController : MonoBehaviour
{

    [field: SerializeField]
    public Transform hookStringPos {  get; private set; }
    [field: SerializeField]
    public Canvas hookCanvas { get; private set; }

    public bool onWater {  get; private set; }
    public Rigidbody rb {  get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hookCanvas.worldCamera = Camera.main;
        hookCanvas.gameObject.SetActive(false);
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
