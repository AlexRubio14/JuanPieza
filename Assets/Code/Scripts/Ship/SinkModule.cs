using UnityEngine;

public class SinkModule : MonoBehaviour
{
    public bool isMoving { get; private set; }
    private bool goDown;

    private Vector3 startPos;
    private Vector3 endPos;

    private float elapsedTime;
    private float duration;

    private void Awake()
    {
        isMoving = false;
        goDown = true;
        
        startPos = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
        endPos = new Vector3(transform.localPosition.x, -2f, transform.localPosition.z);

        elapsedTime = 0f;
        duration = 2f;
    }

    private void Update()
    {
        if (isMoving)
        {
            if(goDown)
            {
                if (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;

                    float t = Mathf.Clamp01(elapsedTime / duration);

                    transform.localPosition = Vector3.Lerp(startPos, endPos, t);
                }
                isMoving = false;
                goDown = false;
                elapsedTime = 0f;
            }
            else
            {
                if (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;

                    float t = Mathf.Clamp01(elapsedTime / duration);

                    transform.position = Vector3.Lerp(endPos, startPos, t);
                }
                isMoving = false;
                goDown = true;
                elapsedTime = 0f;
            }
            
        }
        
    }

    public void Move()
    {
        isMoving = true;
    }
}
