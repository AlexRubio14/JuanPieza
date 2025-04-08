using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager instance;
    public enum FishingState { IDLE, WAITING_PLAYER, CAN_HOOK_PLAYER, HOOKED_PLAYER, WAITING_OBJECT, CAN_HOOK_OBJECT, HOOKED_OBJECT }
    private struct FishingData
    {
        public FishingRod fishingRod;
        public FishingState fishingState;
        public float timeToAppearObject;
        public GameObject fishedObject;

        public DeathState currentPlayer;

        public RescueNPC rescueNPC;

        public float starterTime;
        public float parabolaProcess;
        public Vector3 parabolaStartPos;
        public Vector3 parabolaEndPos;
    }
    private List<FishingData> fishingData;
    private ObjectPool fishingObjectPool;

    private List<DeathState> deadPlayers;

    [Space, SerializeField]
    private Vector2 minMaxTimeToFishing;
    [SerializeField]
    private float playerRespawnOffset;
    [Space, SerializeField]
    private float timeToHook;
    [SerializeField]
    private float parabolaSpeed;
    [SerializeField]
    private float parabolaHeight;

    [field: Space, SerializeField]
    public float defaultYPos { get; private set; }
    [field: SerializeField]
    public float deathZPos { get; private set; }

    [field: Space, Header("Audio"), SerializeField]
    private AudioClip revivePlayerClip;

    [Space, Header("Rescue NPC"), SerializeField]
    private List<RescueNPC> rescueNPCs;
    private void Awake()
    {
        if (instance)
            Destroy(instance);

        instance = this;

        fishingObjectPool = GetComponent<ObjectPool>();
        fishingData = new List<FishingData>();
        deadPlayers = new List<DeathState>();
        rescueNPCs = new List<RescueNPC>();
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
                        WaitingPlayer(i);
                        break;
                    case FishingState.CAN_HOOK_PLAYER:
                        CanHookPlayer(i);
                        break;
                    case FishingState.HOOKED_PLAYER:
                        HookedPlayer(i);
                        break;
                    case FishingState.WAITING_OBJECT:
                        WaitingObject(i);
                        break;
                    case FishingState.CAN_HOOK_OBJECT:
                        CanHookObject(i);
                        break;
                    case FishingState.HOOKED_OBJECT:
                        HookedObject(i);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #region Fishing States
    private void WaitingPlayer(int _id)
    {
        //Aqui o el player deberia estar nadando o ha perdido su oportunidad
        if (fishingData[_id].currentPlayer != null && !fishingData[_id].currentPlayer.isSwimming || //El player esta quieto
            fishingData[_id].rescueNPC != null && !fishingData[_id].rescueNPC.isSwimming // O es el tutorial y el player se esta moviendo
            )
        {
            //Empezar el evento para que el player recoja la caña y salve al player
            CanHook(_id, FishingState.CAN_HOOK_PLAYER);
        }

    }
    private void CanHookPlayer(int _id)
    {
        //Aqui tiene el corto periodo de tiempo para que el player lo resucite
        if (Time.time - fishingData[_id].starterTime >= timeToHook)
        {
            fishingData[_id].fishingRod.player.interactCanvasObject.SetActive(false);
            if (fishingData[_id].currentPlayer != null)
            {
                //Generar las variables del player de nuevo
                //fishingData[_id].currentPlayer.CalculateDeathPos();
            }

        }

    }
    private void HookedPlayer(int _id)
    {
        FishingData parabolaData = fishingData[_id];
        parabolaData.parabolaProcess += Time.deltaTime * parabolaSpeed;
        fishingData[_id] = parabolaData;

        Vector3 parabolaPos = Parabola(fishingData[_id].parabolaStartPos, fishingData[_id].parabolaEndPos, parabolaHeight, fishingData[_id].parabolaProcess);
        if (fishingData[_id].currentPlayer != null)
            fishingData[_id].currentPlayer.transform.position = parabolaPos;
        else if (fishingData[_id].rescueNPC != null)
            fishingData[_id].rescueNPC.transform.position = parabolaPos;


        if (fishingData[_id].parabolaProcess >= 1)//Acabar la pesca
        {
            StopFishing(fishingData[_id].fishingRod);
            if (fishingData[_id].currentPlayer != null)
            {
                //Cambiar el estado del player muerto
                fishingData[_id].currentPlayer.deathStateMachine.ChangeState(fishingData[_id].currentPlayer.deathStateMachine.idleState);
                AudioManager.instance.Play2dOneShotSound(revivePlayerClip, "Objects");
            }
            else if (fishingData[_id].rescueNPC != null)
            {
                fishingData[_id].rescueNPC.NPCRescued();
                AudioManager.instance.Play2dOneShotSound(revivePlayerClip, "Objects");
            }
        }
    }

    private void WaitingObject(int _id)
    {
        //Aqui esta esperando el tiempo necesario para que le spawnee un objeto
        if (Time.time - fishingData[_id].starterTime >= fishingData[_id].timeToAppearObject)
        {
            //Empezar el evento para que el player recoja la caña y consiga un objeto
            CanHook(_id, FishingState.CAN_HOOK_OBJECT);
        }
    }
    private void CanHookObject(int _id)
    {
        //Aqui tiene el corto periodo de tiempo para que el player recoja el objeto
        //Si lo deja a pasar volvera al estado de Waiting Object y generaremos las variables de nuevo
        if (Time.time - fishingData[_id].starterTime >= timeToHook)
        {
            fishingData[_id].fishingRod.player.interactCanvasObject.SetActive(false);
            GenerateWaitObjectValues(_id);
        }
    }
    private void HookedObject(int _id)
    {
        FishingData parabolaData = fishingData[_id];
        parabolaData.parabolaProcess += Time.deltaTime * parabolaSpeed;
        fishingData[_id] = parabolaData;

        fishingData[_id].fishedObject.transform.position =
            Parabola(fishingData[_id].parabolaStartPos, fishingData[_id].parabolaEndPos, parabolaHeight, fishingData[_id].parabolaProcess);



        if (fishingData[_id].parabolaProcess >= 1)//Acabar la pesca
        {
            StopFishing(fishingData[_id].fishingRod);

            //Dejar la caña anclada en el suelo
            fishingData[_id].fishingRod.DropItem(fishingData[_id].fishingRod.player.objectHolder);
            fishingData[_id].fishingRod.transform.position += -fishingData[_id].fishingRod.player.transform.forward * 1f;
            //Poner el objeto en la mano                                                        
            fishingData[_id].fishedObject.GetComponent<InteractableObject>().Interact(fishingData[_id].fishingRod.player.objectHolder);
        }
    }
    #endregion

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

    private void CanHook(int _dataId, FishingState _nextState)
    {
        FishingData newData = fishingData[_dataId];

        newData.fishingState = _nextState;
        newData.parabolaProcess = 0;
        newData.starterTime = Time.time;

        fishingData[_dataId] = newData;
        //Encender el cartel con una exclamacion
        GameObject interactableCanvas = fishingData[_dataId].fishingRod.player.interactCanvasObject;
        interactableCanvas.transform.forward = Camera.main.transform.forward;
        interactableCanvas.transform.localPosition = new Vector3(0.2f, 2, 0.5f);
        interactableCanvas.SetActive(true);
    }

    public void FishingRodUsed(FishingRod _fishingRod)
    {
        int id = GetFisingRodId(_fishingRod);

        if (id == -1)
            return;

        for (int i = 0; i < deadPlayers.Count; i++)
        {
            //if (deadPlayers[i].hookPosition == Vector3.zero)//Si no tiene ningun target
            //{
            //    //Setear en el player muerto el target
            //    deadPlayers[i].SetHookPosition(fishingData[id].fishingRod.hook.transform.position);

            //    FishingData newData = fishingData[id];
            //    newData.currentPlayer = deadPlayers[i];
            //    newData.currentPlayer.isSwimming = true;
            //    newData.fishingState = FishingState.WAITING_PLAYER;
            //    fishingData[id] = newData;
            //    return;
            //}
        }

        RescueNPC rescueNPC = GetNearestAvaliableRescueNPC(_fishingRod);
        if (rescueNPC)
        {
            //Setear en el player muerto el target
            rescueNPC.SetHookPosition(fishingData[id].fishingRod.hook.transform.position);
            rescueNPC.isSwimming = true;
            FishingData newData = fishingData[id];
            newData.currentPlayer = null;
            newData.fishingState = FishingState.WAITING_PLAYER;
            newData.rescueNPC = rescueNPC;
            fishingData[id] = newData;
            return;
        }
            
        for (int i = 0; i < rescueNPCs.Count; i++)
        {
            if (!rescueNPCs[i].rescued &&
            !rescueNPCs[i].isSwimming &&
            rescueNPCs[i].hookPosition == Vector3.zero)
            {
                
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

        switch (fishingData[id].fishingState)
        {
            case FishingState.WAITING_PLAYER:
                GrabWhileWaitingPlayer(id, _fishingRod);
                break;
            case FishingState.CAN_HOOK_PLAYER:
                GrabPlayer(id, _fishingRod);
                break;
            case FishingState.WAITING_OBJECT:
                StopFishing(_fishingRod);
                break;
            case FishingState.CAN_HOOK_OBJECT://Sacar el objeto del agua
                GrabObject(id, _fishingRod);
                break;
            default:
                break;
        }


    }
    private void StopFishing(FishingRod _fishingRod)
    {
        _fishingRod.player.stateMachine.ChangeState(_fishingRod.player.stateMachine.idleState);
        _fishingRod.isFishing = false;
        _fishingRod.player.interactCanvasObject.SetActive(false);
    }

    public void AddFishingRod(FishingRod _fishingRod)
    {
        FishingData data = new FishingData();
        data.fishingRod = _fishingRod;
        data.fishingState = FishingState.IDLE;
        fishingData.Add(data);
    }
    public void ResetFishingRodData(FishingRod _fishingRod)
    {
        int rodId = GetFisingRodId(_fishingRod);
        if (rodId == -1)
            return;

        FishingData newData = fishingData[rodId];
        newData.fishingState = FishingState.IDLE;
        newData.fishedObject = null;
        newData.currentPlayer = null;
        newData.starterTime = 0;
        newData.parabolaProcess = 0;
        newData.parabolaStartPos = Vector3.zero;
        newData.parabolaEndPos = Vector3.zero;

        fishingData[rodId] = newData;

    }
    public void RemoveFishingRod(FishingRod _fishingRod)
    {
        int id = GetFisingRodId(_fishingRod);
        if (id != -1)
            fishingData.Remove(fishingData[id]);
    }

    private RescueNPC GetNearestAvaliableRescueNPC(FishingRod _fishingRod)
    {
        RescueNPC nearestNPC = null;
        float distance = 100;
        for (int i = 0; i < rescueNPCs.Count; i++)
        {
            if(!rescueNPCs[i].rescued &&
            !rescueNPCs[i].isSwimming &&
            rescueNPCs[i].hookPosition == Vector3.zero)
            {
                float currentDitance = Vector3.Distance(rescueNPCs[i].transform.position, _fishingRod.hook.transform.position);
                if (currentDitance < distance)
                {
                    nearestNPC = rescueNPCs[i];
                    distance = currentDitance;
                }
            }
        }

        return nearestNPC;
    }
    #endregion

    #region Grab Fishing Rod
    private void GrabWhileWaitingPlayer(int _id, FishingRod _fishingRod)
    {
        //if (fishingData[_id].currentPlayer != null && fishingData[_id].currentPlayer.hookPosition != Vector3.zero)
        //    fishingData[_id].currentPlayer.CalculateDeathPos();
        //else if (fishingData[_id].rescueNPC != null)
        //    fishingData[_id].rescueNPC.HookRemoved();

        StopFishing(_fishingRod);
    }
    private void GrabPlayer(int _id, FishingRod _fishingRod)
    {
        //Revivir player (No cambiarle el estado de muerto hasta que llegue al barco)
        FishingData newData = fishingData[_id];
        newData.fishingState = FishingState.HOOKED_PLAYER;
        newData.parabolaStartPos = _fishingRod.hook.transform.position;
        newData.parabolaEndPos = _fishingRod.player.transform.position + -_fishingRod.player.transform.forward * playerRespawnOffset;
        fishingData[_id] = newData;


        if (fishingData[_id].rescueNPC != null && fishingData[_id].currentPlayer == null)
            fishingData[_id].rescueNPC.transform.forward = (
                new Vector3(newData.parabolaEndPos.y, fishingData[_id].rescueNPC.transform.position.y, newData.parabolaEndPos.z) -
                fishingData[_id].rescueNPC.transform.position
                ).normalized;
    }
    private void GrabObject(int _id, FishingRod _fishingRod)
    {

        //Generar objeto random de la pool
        ObjectSO itemData = fishingObjectPool.GetRandomItem();
        //Instanciarlo
        GameObject newItem = Instantiate(itemData.prefab, _fishingRod.hook.transform.position, Quaternion.identity);

        InteractableObject currentItem = newItem.GetComponent<InteractableObject>();
        currentItem.hasToBeInTheShip = true;
        ShipsManager.instance.playerShip.AddInteractuableObject(currentItem);
        //Colocarlo
        newItem.transform.forward = _fishingRod.player.transform.forward;
        //Cambiar al estado de Recogida
        FishingData newData = fishingData[_id];
        newData.fishingState = FishingState.HOOKED_OBJECT;
        newData.fishedObject = newItem;
        newData.parabolaStartPos = _fishingRod.hook.transform.position;
        newData.parabolaEndPos = _fishingRod.player.transform.position;
        fishingData[_id] = newData;


    }
    #endregion

    public void AddDeadPlayer(DeathState _deadPlayer)
    {
        deadPlayers.Add(_deadPlayer);
    }
    public void AddTutorialNPC(RescueNPC _rescueNPC)
    {
        rescueNPCs.Add(_rescueNPC);
        RescueManager.instance.AddNpcCount();
    }
    public void RemoveDeadPlayer(DeathState _deadPlayer)
    {
        deadPlayers.Remove(_deadPlayer);
    }

    private static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        System.Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public ObjectPool GetObjectPool()
    {
        return fishingObjectPool;
    }
}