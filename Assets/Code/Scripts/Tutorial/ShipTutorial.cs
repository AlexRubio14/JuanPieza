using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTutorial : MonoBehaviour
{
    [Header("Dialogue"), SerializeField]
    private DialogueData starterData;

    Action hideArrowsAction;
    Action showFAction;
    Action enableFAction;
    Action disableFAction;
    Action showWAction;
    Action enableWAction;
    Action disableWAction;
    Action showCAction;
    Action enableCAction;
    Action disableCAction;


    [Space, Header("Show Arrow"), SerializeField]
    private GameObject[] arrows;
    [SerializeField]
    private float arrowOffset;

    [Space, Header("Layers"), SerializeField]
    private int interactableLayer;
    [SerializeField]
    private int defaultLayer;

    [Space, Header("Fishing"), SerializeField]
    private List<ObjectSO> fishingObjects;
    [SerializeField]
    private ObjectPool fishingObjectPool;
    [SerializeField]
    private ObjectSO fishingRodSO;
    private InteractableObject fishingRodBox;
    [SerializeField]
    private TutorialSwimNPC npc;

    [Space, Header("Wood"), SerializeField]
    private ObjectSO woodObject;
    [SerializeField]
    private List<ObjectSO> woodObjects;
    private bool fixedShip;
    private RepairHole[] holes;
    [SerializeField]
    private DialogueData finishRepairDialogue;
    private bool repairingShip;

    [Space, Header("Cannon"), SerializeField]
    private Cannon cannon;
    [SerializeField]
    private List<ObjectSO> cannonObjects;
    [SerializeField]
    private ObjectSO bulletObject;
    [SerializeField]
    private DialogueData cannonBulletDialogue;
    private InteractableObject hammerBox;
    private bool cannonFixed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        foreach (GameObject obj in arrows)
            obj.SetActive(false);

        repairingShip = false;

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

            fishingRodBox = ShipsManager.instance.playerShip.GetObjectBoxByObject(fishingRodSO);
            hammerBox = ShipsManager.instance.playerShip.GetObjectBoxByObject(cannon.GetRepairItem());
        }

        showFAction += ShowFishing; 
        hideArrowsAction += HideArrow; 
        enableFAction += EnableFishing;
        disableFAction += DisableFishing;
        showWAction += ShowWood;
        enableWAction += EnableWoodObjects;
        disableWAction += DisableWoodObjects;
        showCAction += ShowCannon;
        enableCAction += EnableCannon;
        disableCAction += DisableCannon;

        DialogueController.instance.AddAction("D.T.HA", hideArrowsAction);
        DialogueController.instance.AddAction("D.T.SF", showFAction);
        DialogueController.instance.AddAction("D.T.EF", enableFAction);
        DialogueController.instance.AddAction("D.T.DF", disableFAction);
        DialogueController.instance.AddAction("D.T.SW", showWAction);
        DialogueController.instance.AddAction("D.T.EW", enableWAction);
        DialogueController.instance.AddAction("D.T.DW", disableWAction);
        DialogueController.instance.AddAction("D.T.SC", showCAction);
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
        foreach (GameObject obj in arrows)
            obj.transform.forward = -Camera.main.transform.forward;

        if (repairingShip)
        {

            for (int i = 0; i < holes.Length; i++)
            {
                if (holes[i] != null)
                {
                    arrows[i].SetActive(true);
                    arrows[i].transform.position = holes[i].transform.position + Vector3.up * arrowOffset;
                }
                else
                    arrows[i].SetActive(false);
            }
        }
        if (!fixedShip && HolesRepaired())//No hay agujeros y 
        {
            fixedShip = true;
            //Empezar dialogo del cañon
            DialogueController.instance.StartDialogue(finishRepairDialogue);
            repairingShip = false;
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

    private void HideArrow()
    {
        foreach (GameObject obj in arrows)
            obj.SetActive(false);
    }

    private void ShowFishing()
    {
        arrows[0].SetActive(true);
        arrows[0].transform.position = fishingRodBox.transform.position + Vector3.up * arrowOffset;
        arrows[1].SetActive(true);
        arrows[1].transform.position = npc.transform.position + Vector3.up * arrowOffset;
    }
    private void EnableFishing() 
    {
        foreach (ObjectSO item in fishingObjects)
            SetItemTypeLayer(item, interactableLayer);
    }
    private void DisableFishing()
    {
        foreach (ObjectSO item in fishingObjects)
            SetItemTypeLayer(item, defaultLayer);
    }

    private void ShowWood()
    {
        repairingShip = true;
    }
    private void EnableWoodObjects()
    {
        foreach (ObjectSO item in woodObjects)
            SetItemTypeLayer(item, interactableLayer);

        fishingObjectPool.AddItemToItemPool(woodObject);
        fishingObjectPool.AddItemToPriorityList(woodObject);

    }
    private void DisableWoodObjects()
    {
        foreach (ObjectSO item in woodObjects)
            SetItemTypeLayer(item, interactableLayer);

        fishingObjectPool.RemoveItemFromPool(woodObject);
        fishingObjectPool.RemoveItemFromPriorityList(woodObject);
    }

    private void ShowCannon()
    {
        arrows[0].SetActive(true);
        arrows[0].transform.position = cannon.transform.position + Vector3.up * arrowOffset;
        arrows[1].SetActive(true);
        arrows[1].transform.position = hammerBox.transform.position + Vector3.up * arrowOffset;
    }
    private void EnableCannon()
    {
        foreach (ObjectSO item in cannonObjects)
            SetItemTypeLayer(item, interactableLayer);

        fishingObjectPool.AddItemToItemPool(bulletObject);
        fishingObjectPool.AddItemToPriorityList(bulletObject);
    }
    private void DisableCannon()
    {
        foreach (ObjectSO item in cannonObjects)
            SetItemTypeLayer(item, defaultLayer);

        fishingObjectPool.RemoveItemFromPool(bulletObject);
        fishingObjectPool.RemoveItemFromPriorityList(bulletObject);
    }

}
