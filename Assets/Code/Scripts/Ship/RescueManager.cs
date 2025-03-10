using System;
using System.Collections.Generic;
using UnityEngine;

public class RescueManager : MonoBehaviour
{
    public static RescueManager instance;

    [SerializeField] private List<GameObject> npcs;
    [SerializeField] private List<GameObject> spawnNpcsPositions;
    [SerializeField] private float randomOffset;

    private int npcsCount;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    private void Start()
    {
        GenerateNPC();
    }

    private void GenerateNPC()
    {
        for (int i = 0; i < NodeManager.instance.questData.rescueCuantity; i++) 
        {
            GameObject npcPrefab = npcs[UnityEngine.Random.Range(0, npcs.Count)];
            GameObject spawnPoint = spawnNpcsPositions[UnityEngine.Random.Range(0, spawnNpcsPositions.Count)];
            Vector3 _randomOffset = new Vector3(UnityEngine.Random.Range(-randomOffset, randomOffset), 0,UnityEngine.Random.Range(-randomOffset, randomOffset));
            float randomRotationY = UnityEngine.Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0f, randomRotationY, 0f);
            Instantiate(npcPrefab, spawnPoint.transform.position + _randomOffset, randomRotation);
        }
    }

    public void RemoveNpcCount()
    {
        npcsCount--;
        if(npcsCount == 0)
        {
            Camera.main.GetComponent<ArriveIslandCamera>().enabled = true;
            Camera.main.GetComponent<ArriveIslandCamera>().SetIsRepositing();
        }
    }

    public void AddNpcCount()
    {
        npcsCount++;
    }
}
