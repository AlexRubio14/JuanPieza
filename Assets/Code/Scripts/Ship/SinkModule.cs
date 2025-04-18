using UnityEngine;

public class SinkModule : MonoBehaviour
{
    public bool isMoving { get; private set; }
    private bool goDown;

    private Vector3 startPos;
    private Vector3 endPos;

    private float elapsedTime;
    private float duration = 1.5f; // Puedes ajustar este valor para hacerlo más rápido o más lento

    private void Awake()
    {
        isMoving = false;
        goDown = true;

        startPos = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
        endPos = new Vector3(transform.localPosition.x, -1f, transform.localPosition.z);

        elapsedTime = 0f;
    }

    private void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            if (goDown)
            {
                transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(endPos, startPos, t);
            }

            if (t >= 1f)
            {
                isMoving = false;
                elapsedTime = 0f;

                if (goDown)
                {
                    goDown = false;
                    Invoke("Move", 2f);
                }
                else
                {
                    goDown = true;
                }
            }
        }
    }

    public void Move()
    {
        isMoving = true;
        elapsedTime = 0f;
    }
}
