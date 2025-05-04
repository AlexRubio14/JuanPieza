using UnityEngine;

public class GenerateBoardingBoats : MonoBehaviour
{
    private int raftCount;
    private RaftController currentRaft;
    private bool boardingHasStarted = false;
    private int pirates;
    
    private void Start()
    {
        if (NodeManager.instance.questData.questObjective == QuestData.QuestObjective.BOARDING)
        {
            raftCount = 3;
            pirates = 1;
            Invoke("StartBoarding", 6f);
        }
    }

    private void StartBoarding()
    {
        currentRaft = RaftManager.Instance.CreateRaftEventsHardCoded(pirates);
        boardingHasStarted = true;
    }

    private void Update()
    {
        if (!boardingHasStarted)
            return;

        if (!currentRaft.eventHasFinished)
            return;

        if (PirateBoardingManager.Instance.piratesBoarding.Count <= 0)
        {
            raftCount--;
            if (raftCount <= 0)
            {
                Camera.main.GetComponent<ArriveIslandCamera>().enabled = true;
                Camera.main.GetComponent<ArriveIslandCamera>().SetIsRepositing();
                boardingHasStarted = false;
            }
            else
            {
                if(raftCount == 2)
                    pirates = 2; 
                else
                    pirates = 4;

                currentRaft = RaftManager.Instance.CreateRaftEventsHardCoded(pirates);
                currentRaft.eventHasFinished = false;
            }
        }
    }


}
