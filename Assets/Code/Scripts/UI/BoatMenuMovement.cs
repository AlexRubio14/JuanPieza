using UnityEngine;

public class BoatMenuMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float resetDistance = 20f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        if (Vector3.Distance(startPosition, transform.position) >= resetDistance)
        {
            transform.position = startPosition;
        }
    }
}
