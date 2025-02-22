using System;
using System.Collections.Generic;
using UnityEngine;

public class RaftManager : MonoBehaviour
{
    public static RaftManager Instance;

    [SerializeField] public Queue<Action> raftEventQueue = new Queue<Action>();

    public bool isProcessingEvent { get; private set; } = false;

    private void Awake()
    {
        if(Instance != null && Instance == this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            raftEventQueue.Enqueue(() => CreateRaftEvent());
        }

        ProcessRaftEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateRaftEvent()
    {
        RaftController raftController = managerPirateBoarding.Instance.GetUnusedRaft();

        raftController.SetUpRaft();

    }

    public void ProcessRaftEvent()
    {
        if (raftEventQueue.Count == 0)
            return;

        if(isProcessingEvent) 
            return;

        Action nextEvent = raftEventQueue.Dequeue();
        nextEvent?.Invoke();
    }

    //TODO: this method should be called everytime a battle is finished
    public void ResetQueue()
    {
        raftEventQueue.Clear();
    }
}
