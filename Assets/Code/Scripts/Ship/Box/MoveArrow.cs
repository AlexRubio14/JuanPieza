using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    [SerializeField] private float high;
    [SerializeField] private float speed;
    private float initY;
    private float t = 0f;
    private bool goingUp = true;

    void Start()
    {
        initY = transform.position.y;
    }

    void Update()
    {
        t += Time.deltaTime * speed * (goingUp ? 1 : -1);
        float newY = Mathf.Lerp(initY, initY + high, t);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (t >= 1f)
        {
            t = 1f;
            goingUp = false;
        }
        else if (t <= 0f)
        {
            t = 0f;
            goingUp = true;
        }
    }
}
