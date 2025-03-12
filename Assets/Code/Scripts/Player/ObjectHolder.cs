using System.Collections;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private LayerMask itemsLayerMask;
    [SerializeField] private Transform sphereCastTransform;

    [field: Space, Header("Audio"), SerializeField]
    public AudioClip pickUpClip;


    private InteractableObject nearestInteractableObject;

    private InteractableObject handObject;

    [SerializeField] private Transform[] objectPickedPos;
    public HintController hintController { private set;  get; }

    private bool hasPickedObject = false;

    private RaycastHit[] colliders;

    private void Awake()
    {
        hintController = GetComponentInParent<HintController>();
        colliders = new RaycastHit[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InteractableObject neareastObject = CheckItemsInRange();
        ChangeNearestInteractableObject(neareastObject);

        if(!hintController.showingHints && handObject)
            hintController.UpdateActionType(handObject.ShowNeededInputHint(this));

    }

    // Crea un sphereRaycast, te pilla el interactableObject mas cercano que no este siendo utilizado y te devuelve su script y su collider a la vez.
    public InteractableObject CheckItemsInRange()
    {
        colliders = Physics.SphereCastAll(sphereCastTransform.position, sphereCastRadius, transform.forward, sphereCastRadius, itemsLayerMask);

        InteractableObject currentinteractableObject = null;

        float lastDistance = 1000f;

        foreach (RaycastHit item in colliders) 
        {
            if(Vector3.Distance(transform.parent.position, item.collider.transform.position) >= lastDistance)
            {
                continue;
            }

            InteractableObject tempObject = item.collider.GetComponent<InteractableObject>();

            if (tempObject.isBeingUsed || !tempObject.CanInteract(this) && (tempObject is not Repair || !(tempObject as Repair).GetObjectState().GetIsBroken()))
                continue;

            lastDistance = Vector3.Distance(transform.parent.position, item.collider.transform.position);

            currentinteractableObject = tempObject;

        }

        return currentinteractableObject;
    }

    #region Getters
    public bool GetHasObjectPicked()
    {
        return hasPickedObject;
    }
    public InteractableObject GetHandInteractableObject()
    {
        return handObject;
    }
    public InteractableObject GetNearestInteractableObject()
    {
        return nearestInteractableObject;
    }

    #endregion

    #region Setters
    public void ChangeObjectInHand(InteractableObject _interactableObject, bool _setParent = true)
    {
        if (handObject)
            RemoveItemFromHand();

        hasPickedObject = true;
        handObject = _interactableObject;

        if (!_interactableObject)
            return;
        
        _interactableObject.rb.isKinematic = true;

        if (!_setParent)
            return;

        
        _interactableObject.transform.SetParent(objectPickedPos[(int)_interactableObject.objectSO.objectSize]);

        _interactableObject.transform.localPosition = -_interactableObject.grabPivot.transform.localPosition;


        _interactableObject.transform.localRotation = Quaternion.identity;
        _interactableObject.transform.localRotation = _interactableObject.grabPivot.transform.localRotation;

        foreach (Collider item in _interactableObject.GetComponents<Collider>())
            item.enabled = false;

        _interactableObject.SetIsBeingUsed(true);
    }
    public InteractableObject RemoveItemFromHand()
    {
        InteractableObject currentIO = handObject;

        hasPickedObject = false;
        handObject = null;

        if (currentIO)
        {
            Transform parent = ShipsManager.instance ? ShipsManager.instance.playerShip.transform : null;
            currentIO.transform.SetParent(parent);
            currentIO.SetIsBeingUsed(false);
            currentIO.rb.isKinematic = false;
            foreach (Collider item in currentIO.GetComponents<Collider>())
                item.enabled = true;
        }

        return currentIO;
    }
    public void ChangeNearestInteractableObject(InteractableObject _nearestObject)
    {
        if (handObject && _nearestObject == handObject)
        {
            nearestInteractableObject = null;
            return;
        }

        if(nearestInteractableObject == _nearestObject)
            return;

        if (nearestInteractableObject)
        {
            nearestInteractableObject.GetSelectedVisual().Hide();
            if (nearestInteractableObject.GetTooltip() != null)
            {
                nearestInteractableObject.GetTooltip().RemovePlayer();
                if (nearestInteractableObject.GetTooltip().GetTotalPlayers() <= 0)
                    nearestInteractableObject.GetTooltip().SetState(ObjectsTooltip.ObjectState.None);
            }
        }

        if (!_nearestObject)
        {
            nearestInteractableObject = _nearestObject;
            if (handObject)
                hintController.UpdateActionType(handObject.ShowNeededInputHint(this));
            else
                hintController.UpdateActionType(new HintController.Hint[] { new HintController.Hint(HintController.ActionType.NONE, "") });

            return;
        }

        if (_nearestObject.CanInteract(this))
        {
            _nearestObject.GetSelectedVisual().Show();
            if (_nearestObject.GetTooltip() != null)
            {
                _nearestObject.GetTooltip().AddPlayer();
                if (_nearestObject.GetTooltip().GetTotalPlayers() > 0)
                    _nearestObject.GetTooltip().SetState(ObjectsTooltip.ObjectState.Interacting);
            }
        }
        
        if (_nearestObject is Repair)
        {
            Repair repair = _nearestObject as Repair;
            if (repair.GetObjectState().GetIsBroken())
            {
                if (repair.GetTooltip() != null)
                {
                    if (!repair.CanInteract(this))
                        repair.GetTooltip().AddPlayer();
                    
                    if (repair.GetTooltip().GetTotalPlayers() > 0)
                        repair.GetTooltip().SetState(ObjectsTooltip.ObjectState.Broken);
                }
                _nearestObject.GetSelectedVisual().Show();
            }
        }
        
        nearestInteractableObject = _nearestObject;

        if (nearestInteractableObject.CanInteract(this))
            hintController.UpdateActionType(nearestInteractableObject.ShowNeededInputHint(this));
    }

    #endregion

    public InteractableObject InstantiateItemInHand(ObjectSO _interactableObject)
    {
        InteractableObject item = Instantiate(_interactableObject.prefab).GetComponent<InteractableObject>();
        item.SetIsBeingUsed(true);
        item.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(SetItemParent(item));
        AudioManager.instance.Play2dOneShotSound(pickUpClip, "Objects");

        IEnumerator SetItemParent(InteractableObject interactableObject)
        {
            yield return new WaitForEndOfFrame();

            ChangeObjectInHand(interactableObject);
        }

        return item;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sphereCastTransform.position, sphereCastRadius);
    }
}
