using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class managerPirateBoarding : MonoBehaviour
{
    public static managerPirateBoarding Instance { get; private set; }

    [SerializeField] private List<RaftController> rafts;
    [SerializeField] private GameObject boardingEnemy;
    [SerializeField] private int piratesToSpawn;
    [SerializeField] private Transform piratesHolder;

    [SerializeField] public List<controllerPirateBoarding> piratesPool { get; private set; } = new List<controllerPirateBoarding>();

    private List<Transform> raftsStartPos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiatePirates();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.H)) {
            rafts[0].SetUpRaft();
        }
    }

    private void CalculateRaftSpawnPoints()
    {

    }

    public void InstantiatePirates()
    {
        for (int i = 0; i < piratesToSpawn; i++)
        {
            GameObject pirate = Instantiate(boardingEnemy, piratesHolder.position, boardingEnemy.transform.rotation);

            if (pirate.TryGetComponent(out controllerPirateBoarding controller))
            {
                piratesPool.Add(controller);
                Debug.Log("Pirates Added");
                Debug.Log(i);
                controller.gameObject.transform.SetParent(piratesHolder, true);
            }
        }
    }
}
