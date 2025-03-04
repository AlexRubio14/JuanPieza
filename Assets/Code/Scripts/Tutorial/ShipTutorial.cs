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
    Action showHAction;
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
    [SerializeField]
    private Transform sea;

    [Space, Header("Cannon"), SerializeField]
    private Cannon cannon;
    [SerializeField]
    private List<ObjectSO> cannonObjects;
    [SerializeField]
    private ObjectSO bulletObject;
    [SerializeField]
    private DialogueData cannonBulletDialogue;
    private InteractableObject bulletBox;
    private InteractableObject hammerBox;
    [SerializeField]
    private ObjectSO hammerObject;
    private bool cannonFixed;
    [SerializeField]
    private Transform rock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        foreach (GameObject obj in arrows)
            obj.SetActive(false);

        repairingShip = false;
        fixedShip = false;
        StartCoroutine(StartTutorial(defaultLayer));
        
        IEnumerator StartTutorial(LayerMask _layer)
        {
            yield return new WaitForEndOfFrame();

            foreach (InteractableObject item in ShipsManager.instance.playerShip.GetInventory())
            {
                StartCoroutine(ChangeItemLayer(item, _layer));
            }

            cannon.GetObjectState().SetIsBroke(true);
            cannon.OnBreakObject();

            DialogueController.instance.StartDialogue(starterData);

            fishingRodBox = ShipsManager.instance.playerShip.GetObjectBoxByObject(fishingRodSO);
            hammerBox = ShipsManager.instance.playerShip.GetObjectBoxByObject(cannon.GetRepairItem());
            bulletBox = ShipsManager.instance.playerShip.GetObjectBoxByObject(cannon.objectToInteract);
        }

        showFAction += ShowFishing; 
        hideArrowsAction += HideArrow; 
        enableFAction += EnableFishing;
        disableFAction += DisableFishing;
        showWAction += ShowWood;
        enableWAction += EnableWoodObjects;
        disableWAction += DisableWoodObjects;
        showHAction += ShowHammer;
        showCAction += ShowCannonBulletBox;
        enableCAction += EnableCannon;
        disableCAction += DisableCannon;

        DialogueController.instance.AddAction("D.T.HA", hideArrowsAction);
        DialogueController.instance.AddAction("D.T.SF", showFAction);
        DialogueController.instance.AddAction("D.T.EF", enableFAction);
        DialogueController.instance.AddAction("D.T.DF", disableFAction);
        DialogueController.instance.AddAction("D.T.SW", showWAction);
        DialogueController.instance.AddAction("D.T.EW", enableWAction);
        DialogueController.instance.AddAction("D.T.DW", disableWAction);
        DialogueController.instance.AddAction("D.T.SH", showHAction);
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

        FishMentorTutorial();
        HoleRepairTutorial();
        CannonFixTutorial();
        ShootCannonTutorial();

    }

    private void FishMentorTutorial()
    {
        if (npc.rescued)
            return;
        bool haveFishingRod = false;

        foreach (PlayerController item in PlayersManager.instance.ingamePlayers)
        {
            InteractableObject handObject = item.objectHolder.GetHandInteractableObject();
            if (handObject && handObject.objectSO == fishingRodSO)
                haveFishingRod = true;
        }

        if (haveFishingRod)
            arrows[0].transform.position = npc.transform.position + Vector3.up * arrowOffset;
        else
            ShowFishing();
    }
    private void HoleRepairTutorial()
    {
        if (fixedShip)
            return;

        if (HolesRepaired()) //Si no hay agujeros
        {
            fixedShip = true;
            //Empezar dialogo del ca�on
            DialogueController.instance.StartDialogue(finishRepairDialogue);
            repairingShip = false;
        }

        if (!repairingShip || holes == null)
            return;


        bool haveWood = false;

        foreach (PlayerController item in PlayersManager.instance.ingamePlayers)
        {
            InteractableObject handObject = item.objectHolder.GetHandInteractableObject();
            if (handObject && handObject.objectSO == woodObject)
                haveWood = true;
        }

        //Si tiene madera en la mano
        if (haveWood)
        {
            HideArrow();
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
        else //Si no tiene madera mostrar flecha en 
        {
            HideArrow();

            bool haveFishingRod = false;

            foreach (PlayerController item in PlayersManager.instance.ingamePlayers)
            {
                InteractableObject handObject = item.objectHolder.GetHandInteractableObject();
                if (handObject && handObject.objectSO == fishingRodSO)
                    haveFishingRod = true;
            }

            arrows[0].SetActive(true);

            if (haveFishingRod)
                arrows[0].transform.position = sea.transform.position + Vector3.up * arrowOffset;
            else
                ShowFishing();
        }
    }
    private void CannonFixTutorial()
    {
        if (!HolesRepaired() || cannonFixed)
            return;

        bool haveHammer = false;

        foreach (PlayerController item in PlayersManager.instance.ingamePlayers)
        {
            InteractableObject handObject = item.objectHolder.GetHandInteractableObject();
            if (handObject && handObject.objectSO == hammerObject)
                haveHammer = true;
        }

        arrows[0].SetActive(true);

        if (haveHammer)
            arrows[0].transform.position = cannon.transform.position + Vector3.up * arrowOffset;
        else
            ShowHammer();

        if (holes == null || holes.Length <= 0 || cannonFixed || cannon.GetObjectState().GetIsBroken())
            return;
        cannonFixed = true;
        DialogueController.instance.StartDialogue(cannonBulletDialogue);
    }
    private void ShootCannonTutorial()
    {
        if (!cannonFixed)
            return;

        arrows[0].SetActive(true);
        if (cannon.isPlayerMounted())//Mostrar flecha a la roca
        {
            if(rock)
                arrows[0].transform.position = rock.transform.position + Vector3.up * arrowOffset;
            else
            {
                //ACABAR TUTORIAL
                arrows[0].SetActive(false);
            }
        }
        else //Mostrar flecha al cañon
        {
            bool haveBullet = false;

            foreach (PlayerController item in PlayersManager.instance.ingamePlayers)
            {
                InteractableObject handObject = item.objectHolder.GetHandInteractableObject();
                if (handObject && handObject.objectSO == cannon.objectToInteract)
                    haveBullet = true;
            }

            if (cannon.hasAmmo || haveBullet)
                arrows[0].transform.position = cannon.transform.position + Vector3.up * arrowOffset;
            else
                ShowCannonBulletBox();
        }
    }


    private bool HolesRepaired()
    {
        if (holes == null || holes.Length <= 0)
            return false;

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

    private void ShowHammer()
    {
        arrows[0].SetActive(true);
        arrows[0].transform.position = hammerBox.transform.position + Vector3.up * arrowOffset;
    }
    private void ShowCannonBulletBox()
    {
        arrows[0].SetActive(true);
        arrows[0].transform.position = bulletBox.transform.position + Vector3.up * arrowOffset;
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
