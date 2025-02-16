using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RaftController : MonoBehaviour
{
    [SerializeField] private List<Transform> posToSpawn;

    [SerializeField] private GameObject boardingEnemy;

    [SerializeField] private List<controllerPirateBoarding> pirates;

    public bool isBoarding { get; private set; } = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpRaft()
    {
        if (isBoarding)
            return;

        int enemiesToSpawn = CalculateRandomEnemiesToSpawn();

        InstantiatePirates(enemiesToSpawn);

        isBoarding = true;

        StartBoarding();
    }

    private int CalculateRandomEnemiesToSpawn()
    {
        int enemiesToSpawn = 0;

        switch (MapManager.Instance.GetCurrentHeightLevel())
        {
            case 1:
                enemiesToSpawn = Random.Range(1, 3);
                break;
            case 2:
                enemiesToSpawn = Random.Range(2, 5);
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }

        return enemiesToSpawn;
    }

    private void InstantiatePirates(int _enemiesToSpawn)
    {
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            if (managerPirateBoarding.Instance.piratesPool.Count == 0)
            {
                managerPirateBoarding.Instance.InstantiatePirates();
            }

            controllerPirateBoarding controller = managerPirateBoarding.Instance.piratesPool[i];

            pirates.Add(controller);
            controller.transform.SetParent(transform, false);
            controller.transform.position = posToSpawn[i].position;
        }
    }

    private void StartBoarding()
    {
        Debug.Log("Boardeo");
    }
}
