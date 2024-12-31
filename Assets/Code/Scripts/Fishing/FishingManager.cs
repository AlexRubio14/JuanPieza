using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static FishingManager;

public class FishingManager : MonoBehaviour
{
    public static FishingManager instance;

    public enum Side { LEFT, RIGHT }
    public enum FishingState { IDLE, WAITING_PLAYER, CAN_HOOK_PLAYER, HOOKED_PLAYER, WAITING_OBJECT, CAN_HOOK_OBJECT, HOOKED_OBJECT }
    private struct FishingData
    {
        public FishingRod fishingRod;
        public FishingState fishingState;
        public float timeToAppearObject;
        public float starterTime;
        public int currentPlayerId;
        public Vector3 lastPlayerPos;
        public float parabolaProcess;
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
        FishingUpdate();
    }

    private void FishingUpdate()
    {

        for (int i = 0; i < fishingData.Count; i++)
        {
            if (fishingData[i].fishingRod.isFishing)
            {
                //comprobar el estado de la pesca
                switch (fishingData[i].fishingState)
                {
                    case FishingState.IDLE:
                        break;
                    case FishingState.WAITING_PLAYER:
                        //Aqui o el player deberia estar nadando o ha perdido su oportunidad
                        //Comprobar si la posicion actual es la misma que la anterior
                        Vector3 actualPlayerPos = deadPlayers[fishingData[i].currentPlayerId].Item1.transform.position;

                        if (actualPlayerPos == fishingData[i].lastPlayerPos) //El player esta quieto
                        {
                            //Empezar el evento para que el player recoja la caña y salve al player
                            CanHook(i, FishingState.CAN_HOOK_PLAYER);
                        }
                        else //El player se esta moviendo
                        {
                            FishingData newData = fishingData[i];
                            newData.lastPlayerPos = actualPlayerPos;
                            fishingData[i] = newData;
                        }

                        break;
                    case FishingState.CAN_HOOK_PLAYER:
                        //Aqui tiene el corto periodo de tiempo para que el player lo resucite
                        //Si no le da el player que esta muerto tendra que continuar con su camino
                        break;
                    case FishingState.WAITING_OBJECT:
                        //Aqui esta esperando el tiempo necesario para que le spawnee un objeto
                        if (Time.time - fishingData[i].starterTime >= fishingData[i].timeToAppearObject)
                        {
                            //Empezar el evento para que el player recoja la caña y consiga un objeto
                            CanHook(i, FishingState.CAN_HOOK_OBJECT);
                        }
                        break;
                    case FishingState.CAN_HOOK_OBJECT:
                        //Aqui tiene el corto periodo de tiempo para que el player recoja el objeto
                        //Si lo deja a pasar volvera al estado de Waiting Object y generaremos las variables de nuevo

                        break;
                    case FishingState.HOOKED_OBJECT:

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
    private void GenerateWaitObjectValues(int _id)
    {
        FishingData newData = fishingData[_id];

        //Generar tiempo de espera random
        newData.timeToAppearObject = Random.Range(minMaxTimeToFishing.x, minMaxTimeToFishing.y);
        newData.starterTime = Time.time;
        newData.fishingState = FishingState.WAITING_OBJECT;
        fishingData[_id] = newData;
    } 
    private void CanHook(int _dataId ,FishingState _nextState)
    {
        FishingData newData = fishingData[_dataId];

        newData.fishingState = _nextState;
        newData.parabolaProcess = 0;

        //Encender el 
    }

    public void FishingRodUsed(FishingRod _fishingRod)
    {
        int id = GetFisingRodId(_fishingRod);

        if (id == -1)
            return;


        //Comprobar en que lado esta
        Side hookSide = GetSideByXPos(_fishingRod.hook.transform.position.x);

        for (int i = 0;i < deadPlayers.Count; i++)
        {
            if (hookSide == deadPlayers[i].Item2 && deadPlayers[i].Item1.hookPosition == Vector3.zero)//Si en el lado que esta hay algun aliado ahogandose y no tiene ningun target
            {
                //Setear en el player muerto el target
                deadPlayers[i].Item1.SetHookPosition(fishingData[id].fishingRod.hook.transform.position);
                FishingData newData = fishingData[id];

                newData.currentPlayerId = i;
                newData.fishingState = FishingState.WAITING_PLAYER;
                fishingData[id] = newData;
                return;
            }
        }

        //Toca pescar objetos
        GenerateWaitObjectValues(id);
    }
    public void HookGrabbed(FishingRod _fishingRod)
    {
        int id = GetFisingRodId(_fishingRod);
        if (id == -1)
            return;

        FishingData newData;
        
        switch (fishingData[id].fishingState)
        {
            case FishingState.WAITING_PLAYER:
                if (deadPlayers.Count > 0 && deadPlayers[fishingData[id].currentPlayerId].Item1.hookPosition != Vector3.zero)
                    deadPlayers[fishingData[id].currentPlayerId].Item1.CalculateDeathPos();
                break;
            case FishingState.CAN_HOOK_PLAYER:
                //Revivir player (No cambiarle el estado de muerto hasta que llegue al barco)
                break;
            case FishingState.CAN_HOOK_OBJECT:
                //Sacar el objeto del agua

                //Generar objeto random de la pool

                //Instanciarlo

                //Colocarlo

                //Cambiar al estado de Recogida
                newData = fishingData[id];
                newData.fishingState = FishingState.HOOKED_OBJECT;
                fishingData[id] = newData;
                break;
            default:
                break;
        }
        

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
        int id = GetFisingRodId(_fishingRod);
        if (id != -1)
            fishingData.Remove(fishingData[id]);
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

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        System.Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}