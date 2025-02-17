using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTutorial : MonoBehaviour
{
    [Header("Dialogue"), SerializeField]
    private DialogueData starterData;
    
    Action enablePMAction;
    Action disablePMAction;
    Action showFAction;
    Action enableFAction;
    Action disableFAction;
    Action enableWAction;
    Action disableWAction;
    Action enableCAction;
    Action disableCAction;



    [Space, Header("Layers"), SerializeField]
    private int interactableLayer;
    [SerializeField]
    private int defaultLayer;

    [Space, Header("Fishing"), SerializeField]
    private List<ObjectSO> fishingObjects;
    [SerializeField]
    private ObjectPool fishingObjectPool;

    [Space, Header("Wood"), SerializeField]
    private ObjectSO woodObject;
    [SerializeField]
    private List<ObjectSO> woodObjects;
    private bool fixedShip;
    private RepairHole[] holes;
    [SerializeField]
    private DialogueData finishRepairDialogue;

    [Space, Header("Cannon"), SerializeField]
    private Cannon cannon;
    [SerializeField]
    private List<ObjectSO> cannonObjects;
    [SerializeField]
    private ObjectSO bulletObject;
    [SerializeField]
    private DialogueData cannonBulletDialogue;
    private bool cannonFixed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartTutorial(defaultLayer));

        IEnumerator StartTutorial(LayerMask _layer)
        {
            yield return new WaitForEndOfFrame();

            foreach (InteractableObject item in ShipsManager.instance.playerShip.GetInventory())
            {
                StartCoroutine(ChangeItemLayer(item, _layer));
            }

            ShippingSail sail = ShipsManager.instance.playerShip.GetComponentInChildren<ShippingSail>();
            sail.enabled = false;
            sail.gameObject.layer = defaultLayer;

            cannon.GetObjectState().SetIsBroke(true);
            cannon.OnBreakObject();

            DialogueController.instance.StartDialogue(starterData);
        }
        enablePMAction += EnablePlayerMovement;
        disablePMAction += DisablePlayerMovement;
        //showFAction +=; 
        enableFAction += EnableFishing;
        disableFAction += DisableFishing;
        enableWAction += EnableWoodObjects;
        disableWAction += DisableWoodObjects;
        enableCAction += EnableCannon;
        disableCAction += DisableCannon;

        DialogueController.instance.AddAction("D.T.EPM", enablePMAction);
        DialogueController.instance.AddAction("D.T.DPM", disablePMAction);
        DialogueController.instance.AddAction("D.T.SF", showFAction);
        DialogueController.instance.AddAction("D.T.EF", enableFAction);
        DialogueController.instance.AddAction("D.T.DF", disableFAction);
        DialogueController.instance.AddAction("D.T.EW", enableWAction);
        DialogueController.instance.AddAction("D.T.DW", disableWAction);
        DialogueController.instance.AddAction("D.T.EC", enableCAction);
        DialogueController.instance.AddAction("D.T.DC", disableCAction);

        StartCoroutine(FindHoles());

        IEnumerator FindHoles()
        {
            yield return new WaitForSeconds(3);

            holes = FindObjectsByType<RepairHole>(FindObjectsSortMode.None);
        }

    }

    private void FixedUpdate()
    {
        if (!fixedShip && HolesRepaired())//No hay agujeros y 
        {
            fixedShip = true;
            //Empezar dialogo del cañon
            DialogueController.instance.StartDialogue(finishRepairDialogue);
        }
        if (!cannonFixed && !cannon.GetObjectState().GetIsBroken())
        {
            cannonFixed = true;
            DialogueController.instance.StartDialogue(cannonBulletDialogue);
        }

    }

    private bool HolesRepaired()
    {
        foreach (RepairHole item in holes)
        {
            if (item != null)
                return false;
        }

        return true;
    }
    private void SetItemTypeLayer(ObjectSO _objectType, int _layer)
    {
        foreach (InteractableObject item in ShipsManager.instance.playerShip.GetInventory())
        {
            if (item.objectSO == _objectType)
                StartCoroutine(ChangeItemLayer(item, _layer));
        }
    }
    private IEnumerator ChangeItemLayer(InteractableObject _item, int _layer)
    {
        yield return new WaitForEndOfFrame();
        _item.gameObject.layer = _layer;
    }

    public void EnableFishing() 
    {
        foreach (ObjectSO item in fishingObjects)
            SetItemTypeLayer(item, interactableLayer);
    }
    public void DisableFishing()
    {
        foreach (ObjectSO item in fishingObjects)
            SetItemTypeLayer(item, defaultLayer);
    }

    public void EnableWoodObjects()
    {
        foreach (ObjectSO item in woodObjects)
            SetItemTypeLayer(item, interactableLayer);

        fishingObjectPool.AddItemToItemPool(woodObject);
        fishingObjectPool.AddItemToPriorityList(woodObject);
    }
    public void DisableWoodObjects()
    {
        foreach (ObjectSO item in woodObjects)
            SetItemTypeLayer(item, interactableLayer);

        fishingObjectPool.RemoveItemFromPool(woodObject);
        fishingObjectPool.RemoveItemFromPriorityList(woodObject);
    }

    public void EnableCannon()
    {
        foreach (ObjectSO item in cannonObjects)
            SetItemTypeLayer(item, interactableLayer);

        fishingObjectPool.AddItemToItemPool(bulletObject);
        fishingObjectPool.AddItemToPriorityList(bulletObject);
    }
    public void DisableCannon()
    {
        foreach (ObjectSO item in cannonObjects)
            SetItemTypeLayer(item, defaultLayer);

        fishingObjectPool.RemoveItemFromPool(bulletObject);
        fishingObjectPool.RemoveItemFromPriorityList(bulletObject);
    }

    public void EnablePlayerMovement()
    {
        //foreach (PlayerController item in players)
        //    item.enabled = true;
        
    }
    public void DisablePlayerMovement()
    {
        //players = new List<PlayerController>();
        //for (int i = 0; i < PlayersManager.instance.ingamePlayers.Count; i++)
        //{
        //    players.Add(PlayersManager.instance.ingamePlayers[i]);
        //    PlayersManager.instance.ingamePlayers[i].enabled = false;
        //}

    }

}
