using System.Collections.Generic;
using UnityEngine;

public class SinkModuleController : MonoBehaviour
{
    private List<Transform> modules = new List<Transform>();

    [SerializeField] private float interval;
    private float timer = 0f;

    void Start()
    {
        foreach (Transform child in transform)  
        {
            modules.Add(child);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            MoveModule();
            timer = 0f;
        }
    }

    private void MoveModule()
    {
        SinkModule module = modules[GetRandomChild()].GetComponent<SinkModule>();
        if (!module.isMoving)
        {
            module.Move();
        }
        else
        {
            MoveModule();
        }
    }

    private int GetRandomChild()
    {
        return Random.Range(0, transform.childCount);
    }
}
