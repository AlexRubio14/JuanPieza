using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    public float minScale;
    public float maxScale;
    public float speed;

    void Update()
    {
        float scale = Mathf.PingPong(Time.time * speed, maxScale - minScale) + minScale;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
