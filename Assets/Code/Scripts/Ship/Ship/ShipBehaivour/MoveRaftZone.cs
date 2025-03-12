using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Zone
{
    public GameObject zone;
    public float startZ { get; set; }
    public float endZ { get; set; }
}

public class MoveRaftZone : MonoBehaviour
{
    public enum MoveRaftState { WAIT, MOVE}

    [Header("Move Varibales")]
    [SerializeField] private List<Zone> leftZone;
    [SerializeField] private List<Zone> rightZone;
    [SerializeField] private float moveValue;
    private float currentMoveValue;
    [SerializeField] private float speed;
    public MoveRaftState currentState;

    [Header("Wait State")]
    [SerializeField] private float maxTime;
    private float currentTime;
    private float t;

    private void Start()
    {
        currentTime = 0;
        currentMoveValue = moveValue;
    }

    private void Update()
    {
        switch (currentState)
        {
            case MoveRaftState.WAIT:
                Wait();
                break;
            case MoveRaftState.MOVE:
                MoveDown();
                break;
        }
    }

    private void Wait()
    {
        currentTime += Time.deltaTime;
        if (currentTime > maxTime)
        {
            currentTime = 0;
            t = 0;
            ChangeState(MoveRaftState.MOVE);
        }
    }

    private void MoveDown()
    {
        Move(leftZone);
        Move(rightZone);

        if(t >= 1)
        {
            t = 0;
            ChangeState(MoveRaftState.WAIT);
        }
    }

    private void Move(List<Zone> zones)
    {
        foreach (Zone zone in zones)
        {
            t += Time.deltaTime * speed;
            float newZ = Mathf.Lerp(zone.startZ, zone.endZ, t);
            zone.zone.transform.position = new Vector3(zone.zone.transform.position.x, zone.zone.transform.position.y, newZ);
        }
    }

    private void ChangeState(MoveRaftState newState)
    {
        switch (currentState)
        {
            case MoveRaftState.WAIT:
                SaveInitZ();
                break;
            case MoveRaftState.MOVE:
                break;
        }

        currentState = newState;

        switch (currentState)
        {
            case MoveRaftState.WAIT:
                break;
            case MoveRaftState.MOVE:
                break;
        }
    }

    private void SaveInitZ()
    {
        if (leftZone[0].zone.transform.position.z > (moveValue - 1))
            currentMoveValue = -currentMoveValue;
        else if (leftZone[0].zone.transform.position.z < (-moveValue + 1))
            currentMoveValue = -currentMoveValue;

        foreach (Zone zone in leftZone)
        {
            zone.startZ = zone.zone.transform.position.z;
            zone.endZ = zone.zone.transform.position.z + currentMoveValue;
        }
        foreach (Zone zone in rightZone)
        {
            zone.startZ = zone.zone.transform.position.z;
            zone.endZ = zone.zone.transform.position.z - currentMoveValue;
        }


    }

}
