using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager instance;

    public enum Side { LEFT, RIGHT }
    public enum FishingState { IDLE, WAITING_PLAYER, HOOKED_PLAYER, WAITING_OBJECT, HOOKED_OBJECT}
    private struct FishingData
    {
        public FishingRod fishingRod;
        public FishingState fishingState;
        public float timeToAppearObject;
        public float timePassed;
        public int currentPlayerId;
    }
    private List<FishingData> fishingData;
    private ObjectPool fishingObjectPool;

    private List<(DeathState, Side)> deadPlayers;
    
    [SerializeField]
    private float midXPos;
    [SerializeField]
    private Vector2 minMaxTimeToFishing;

    [field: SerializeField]
    public float defaultYPos { get; private set; }
    [field: SerializeField]
    public float deathZPos { get; private set; }

    private void Awake()
    {
        if (instance)
            Destroy(instance);

        instance = this;

        fishingObjectPool = GetComponent<ObjectPool>();
        fishingData = new List<FishingData>();
        deadPlayers = new List<(DeathState, Side)> ();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FishingData item in fishingData)
        {
            if (item.fishingRod.isFishing)
            {
                //comprobar el estado de la pesca
                switch (item.fishingState)
                {
                    case FishingState.IDLE:
                        break;
                    case FishingState.WAITING_PLAYER:
                        //Aqui o el player deberia estar nadando o ha perdido su oportunidad
                        break;
                    case FishingState.HOOKED_PLAYER:
                        //Aqui tiene el corto periodo de tiempo para que el player lo resucite
                        //Si no le da el player que esta muerto tendra que continuar con su camino
                        break;
                    case FishingState.WAITING_OBJECT:
                        //Aqui esta esperando el tiempo necesario para que le spawnee un objeto
                        break;
                    case FishingState.HOOKED_OBJECT:
                        //Aqui tiene el corto periodo de tiempo para que el player recoja el objeto
                        //Si lo deja a pasar volvera al estado de Waiting Object y reiniciaremos el tiempo que ha pasado a 0
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private Side GetSideByXPos(float _currentX)
    {
        if(_currentX <= midXPos)
            return Side.RIGHT;

        return Side.LEFT;
    }

    #region Fishing Rods
    private int GetFisingRodId(FishingRod _fishingRod)
    {
        int fishingRodId = -1;
        for (int i = 0; i < fishingData.Count; i++)
        {
            if (fishingData[i].fishingRod == _fishingRod)
            {
                fishingRodId = i;
                break;
            }
        }

        return fishingRodId;
    }

    public void FishingRodUsed(FishingRod _fishingRod)
    {
        int id = GetFisingRodId(_fishingRod);

        if (id == -1)
            return;

        FishingData newData = fishingData[id];

        //Comprobar en que lado esta
        Side hookSide = GetSideByXPos(_fishingRod.hook.transform.position.x);

        for (int i = 0;i < deadPlayers.Count; i++)
        {
            if (hookSide == deadPlayers[i].Item2 && deadPlayers[i].Item1.hookPosition == Vector3.zero)//Si en el lado que esta hay algun aliado ahogandose y no tiene ningun target
            {
                //Setear en el player muerto el target
                deadPlayers[i].Item1.SetHookPosition(fishingData[id].fishingRod.hook.transform.position);
                

                newData.currentPlayerId = i;
                newData.fishingState = FishingState.WAITING_PLAYER;
                fishingData[id] = newData;
                return;
            }
        }

        //Toca pescar objetos
        //Generar tiempo de espera random
        float randTime = Random.Range(minMaxTimeToFishing.x, minMaxTimeToFishing.y);
        newData.timeToAppearObject = randTime;
        newData.timePassed = 0;
        newData.fishingState = FishingState.WAITING_OBJECT;
        fishingData[id] = newData;
    }
    public void HookGrabbed(FishingRod _fishingRod)
    {
        int id = GetFisingRodId(_fishingRod);
        if (id == -1)
            return;

        if (deadPlayers[fishingData[id].currentPlayerId].Item1.hookPosition != Vector3.zero)
            deadPlayers[fishingData[id].currentPlayerId].Item1.CalculateDeathPos();

    }


    public void AddFishingRod(FishingRod _fishingRod)
    {
        FishingData data = new FishingData();
        data.fishingRod = _fishingRod;
        data.fishingState = FishingState.IDLE;
        fishingData.Add(data);
    }
    public void RemoveFishingRod(FishingRod _fishingRod)
    {
        fishingData.Remove(fishingData[GetFisingRodId(_fishingRod)]);
    }
    #endregion

    public void AddDeadPlayer(DeathState _deadPlayer)
    {
        deadPlayers.Add((_deadPlayer, GetSideByXPos(_deadPlayer.transform.position.x)));
    }
    public void RemoveDeadPlayer(DeathState _deadPlayer)
    {
        deadPlayers.Remove((_deadPlayer, GetSideByXPos(_deadPlayer.transform.position.x)));
    }

}
