using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager instance;
    public enum FishingState { IDLE, WAITING_NPC, CAN_HOOK_NPC, HOOKED_NPC, WAITING_OBJECT, CAN_HOOK_OBJECT, HOOKED_OBJECT, HOOKED_PLAYER }
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

    [Space, Header("Hooked Entities"), SerializeField]
    private float timeToHook;
    [SerializeField]
    private RumbleController.RumblePressets canHookRumble;
    [SerializeField]
    private float parabolaSpeed;
    [SerializeField]
    private float parabolaHeight;
    
    [Space, Header("Object Fishing"), SerializeField]
    private Vector2 minMaxTimeToFishing;
    

    [field: Space, Header("Humanoid Rescue"), SerializeField]
    public float defaultYPos { get; private set; }
    [field: SerializeField]
    public float deathZPos { get; private set; }
    [SerializeField]
    private float humanoidRespawnOffset;
    [SerializeField]
    private float playerRescueDistance;

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
                    case FishingState.WAITING_NPC:
                        WaitingHumanoid(i);
                        CheckNearPlayers(i);
                        break;
                    case FishingState.HOOKED_NPC:
                        HookedHumanoid(i);
                        break;
                    case FishingState.HOOKED_PLAYER:
                        HookedHumanoid(i);
                        break;
                    case FishingState.WAITING_OBJECT:
                        WaitingObject(i);
                        CheckNearPlayers(i);
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
    private void WaitingHumanoid(int _id)
    {
        //Aqui o el player deberia estar nadando o ha perdido su oportunidad
        if (fishingData[_id].rescueNPC != null && !fishingData[_id].rescueNPC.isSwimming) // Si un NPC no se esta moviendo
        {
            //Empezar el evento para que el player recoja la caña y salve al player
            CanHook(_id, FishingState.CAN_HOOK_NPC);
        }

    }
    private void HookedHumanoid(int _id)
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
            fishingData[_id].fishingRod.hook.hookCanvas.gameObject.SetActive(false);
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
            fishingData[_id].fishedObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            StopFishing(fishingData[_id].fishingRod);
            
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
        GameObject interactableCanvas = fishingData[_dataId].fishingRod.hook.hookCanvas.gameObject;
        interactableCanvas.transform.forward = Camera.main.transform.forward;
        interactableCanvas.transform.localPosition = new Vector3(0.2f, 2, 0.5f);
        interactableCanvas.SetActive(true);


        int playerId = fishingData[_dataId].fishingRod.player.playerInput.playerReference;
        PlayersManager.instance.players[playerId].rumbleController.AddRumble(canHookRumble);
    }

    public void FishingRodUsed(FishingRod _fishingRod)
    {
        int id = GetFisingRodId(_fishingRod);

        if (id == -1)
            return;

        RescueNPC rescueNPC = GetNearestAvaliableRescueNPC(_fishingRod);
        if (rescueNPC)
        {
            //Setear en el player muerto el target
            rescueNPC.SetHookPosition(fishingData[id].fishingRod.hook.gameObject);
            rescueNPC.isSwimming = true;
            FishingData newData = fishingData[id];
            newData.currentPlayer = null;
            newData.fishingState = FishingState.WAITING_NPC;
            newData.rescueNPC = rescueNPC;
            fishingData[id] = newData;
            return;
        }
            
        for (int i = 0; i < rescueNPCs.Count; i++)
        {
            if (!rescueNPCs[i].rescued &&
            !rescueNPCs[i].isSwimming &&
            rescueNPCs[i].hookToFollow == null)
            {   
                return;
            }
        }

        //Toca pescar objetos
        GenerateWaitObjectValues(id);
    }
    public IEnumerator HookGrabbed(FishingRod _fishingRod)
    {
        yield return new WaitForEndOfFrame();

        int id = GetFisingRodId(_fishingRod);
        if (id != -1)
        {
            switch (fishingData[id].fishingState)
            {
                case FishingState.WAITING_NPC:
                    GrabWhileWaitingHumanoid(id, _fishingRod);
                    break;
                case FishingState.CAN_HOOK_NPC:
                    GrabHumanoid(id, _fishingRod);
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
    }
    private void StopFishing(FishingRod _fishingRod)
    {
        _fishingRod.player.stateMachine.ChangeState(_fishingRod.player.stateMachine.idleState);
        _fishingRod.isFishing = false;
        _fishingRod.hook.hookCanvas.gameObject.SetActive(false);
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
            rescueNPCs[i].hookToFollow == null)
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
    private void GrabWhileWaitingHumanoid(int _id, FishingRod _fishingRod)
    {
        if (fishingData[_id].rescueNPC != null)
            fishingData[_id].rescueNPC.HookRemoved();

        StopFishing(_fishingRod);
    }
    private void GrabHumanoid(int _id, FishingRod _fishingRod)
    {
        //Revivir player (No cambiarle el estado de muerto hasta que llegue al barco)
        FishingData newData = fishingData[_id];
        newData.fishingState = FishingState.HOOKED_NPC;
        newData.parabolaStartPos = _fishingRod.hook.transform.position;
        newData.parabolaEndPos = _fishingRod.player.transform.position + -_fishingRod.player.transform.forward * humanoidRespawnOffset;
        fishingData[_id] = newData;


        if (fishingData[_id].rescueNPC != null && fishingData[_id].currentPlayer == null)
            fishingData[_id].rescueNPC.transform.forward = (
                new Vector3(newData.parabolaEndPos.y, fishingData[_id].rescueNPC.transform.position.y, newData.parabolaEndPos.z) -
                fishingData[_id].rescueNPC.transform.position
                ).normalized;
        AudioManager.instance.Play2dOneShotSound(fishingData[_id].fishingRod.grabWaterHookClip, "Objects", 0.7f, 0.85f, 1.15f);
    }
    private void GrabObject(int _id, FishingRod _fishingRod)
    {

        //Generar objeto random de la pool
        ObjectSO itemData = fishingObjectPool.GetRandomItem();
        //Instanciarlo
        GameObject newItem = Instantiate(itemData.prefab, _fishingRod.hook.transform.position, Quaternion.identity);

        //Si esta 
        if (newItem.TryGetComponent(out FreezeWeapon freezeWeapon) && NodeManager.instance.questData.questClimete == QuestData.QuestClimete.SNOW)
            freezeWeapon.SetFreeze(true);

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

        float itemOffset = 1;
        if (newItem.TryGetComponent(out Weapon _weapon))
            itemOffset = 2.3f;
        Vector3 targetPos = _fishingRod.player.transform.position - _fishingRod.player.transform.forward * itemOffset + Vector3.up;

        newData.parabolaEndPos = targetPos;
        fishingData[_id] = newData;

        AudioManager.instance.Play2dOneShotSound(fishingData[_id].fishingRod.grabWaterHookClip, "Objects", 0.2f, 0.85f, 1.15f);
    }

    #endregion

    private void CheckNearPlayers(int _id)
    {
        if (!fishingData[_id].fishingRod.hook)
            return;
        foreach (DeathState item in deadPlayers)
        {
            if (!item.isSwimming)
                continue;

            float distance = Vector3.Distance(fishingData[_id].fishingRod.hook.transform.position, item.transform.position);
            if (distance <= playerRescueDistance)
            {
                item.isSwimming = false;

                if (fishingData[_id].rescueNPC != null)
                    fishingData[_id].rescueNPC.HookRemoved();


                FishingData newData = fishingData[_id];
                newData.fishingState = FishingState.HOOKED_PLAYER;
                newData.currentPlayer = item;
                newData.rescueNPC = null;
                newData.parabolaProcess = 0;
                newData.parabolaStartPos = fishingData[_id].fishingRod.hook.transform.position;
                newData.parabolaEndPos = 
                    fishingData[_id].fishingRod.player.transform.position + -fishingData[_id].fishingRod.player.transform.forward * humanoidRespawnOffset;
                fishingData[_id] = newData;

                fishingData[_id].fishingRod.GrabHook();
                return;
            }

        }
    }

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