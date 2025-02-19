using UnityEngine;

public class PetrolDecal : MonoBehaviour
{
    [SerializeField] private float time;
    private float currentTime;

    private void Start()
    {
        currentTime = 0f;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > time) { Destroy(gameObject); }
    }
}
