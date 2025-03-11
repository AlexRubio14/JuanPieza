using UnityEngine;

public class GenerateBoardingBoats : MonoBehaviour
{
    private int raftCount;
    private RaftController currentRaft;

    private void Start()
    {
        if (NodeManager.instance.questData.questObjective == QuestData.QuestObjective.BOARDING) 
        {
            raftCount = 3;
            currentRaft = RaftManager.Instance.CreateRaftEventsHardCoded();
        }
    }

    private void Update()
    {
        if (PirateBoardingManager.Instance.piratesBoarding.Count >= 0) //Si hay piratas abordando return;
            return;

        if (currentRaft == null)
            return;

        if (currentRaft.GetPirates().Count == 0)
            return;


            raftCount--;
            if(raftCount <= 0)
            {
                Camera.main.GetComponent<ArriveIslandCamera>().enabled = true;
                Camera.main.GetComponent<ArriveIslandCamera>().SetIsRepositing();
            }
            else
                currentRaft = RaftManager.Instance.CreateRaftEventsHardCoded();
        }
}
