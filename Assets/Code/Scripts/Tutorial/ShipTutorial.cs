using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTutorial : MonoBehaviour
{
    [SerializeField]
    private int interactableLayer;
    [SerializeField]
    private int defaultLayer;
    [SerializeField]
    private ObjectPool fishingObjectPool;

    [Space, Header("Fishing"), SerializeField]
    private List<ObjectSO> fishingObjects;

    [Space, Header("Wood"), SerializeField]
    private ObjectSO woodObject;
    [SerializeField]
    private List<ObjectSO> woodObjects;

    [Space, Header("Cannon"), SerializeField]
    private Cannon cannon;
    [SerializeField]
    private List<ObjectSO> cannonObjects;
    [SerializeField]
    private ObjectSO bulletObject;
    
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
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            EnableCannon();
        if (Input.GetKeyDown(KeyCode.P))
            EnableFishing();
        if (Input.GetKeyDown(KeyCode.I))
            EnableWoodObjects();
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

}
