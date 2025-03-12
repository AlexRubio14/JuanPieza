using System;
using System.Collections.Generic;
using UnityEngine;

public class RaftManager : MonoBehaviour
{
    public static RaftManager Instance;

    [SerializeField] public Queue<Action> raftEventQueue = new Queue<Action>();

    [HideInInspector] public bool isProcessingEvent = false;

    [SerializeField] private GenerateBoardingBoats generateBoardingBoats;

    private void Awake()
    {
        if(Instance != null && Instance == this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void CreateRaftEvents(BoardShip _boardShip)
    {
        RaftController raftController = PirateBoardingManager.Instance.GetUnusedRaft();

        raftEventQueue.Enqueue(() => raftController.SetUpRaft(_boardShip));
        ProcessRaftEvent();
    } 


    public void ProcessRaftEvent()
    {
        if (raftEventQueue.Count == 0)
            return;

        if(isProcessingEvent) 
            return;

        isProcessingEvent = true;

        Action nextEvent = raftEventQueue.Dequeue();
        nextEvent?.Invoke();
    }

    //TODO: this method should be called everytime a battle is finished
    public void ResetQueue()
    {
        raftEventQueue.Clear();
    }

    public RaftController CreateRaftEventsHardCoded()
    {
        RaftController raftController = PirateBoardingManager.Instance.GetUnusedRaft();

        raftEventQueue.Enqueue(() => raftController.SetUpRaftHardCoded());
        ProcessRaftEvent();


        return raftController;
    }
}
