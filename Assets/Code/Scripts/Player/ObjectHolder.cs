using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private LayerMask itemsLayerMask;
    [SerializeField] private Transform sphereCastTransform;
    
    private InteractableObject nearestInteractableObject;

    private InteractableObject interactableObject;

    [SerializeField] private Transform objectPickedPos;
    private HintController hintController;

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

            if (tempObject.isBeingUsed)
                continue;

            lastDistance = Vector3.Distance(transform.parent.position, item.collider.transform.position);

            currentinteractableObject = tempObject;

        }

        return currentinteractableObject;
    }

    #region Getters
    public Vector3 GetObjectPickedPosition()
    {
        return objectPickedPos.position;
    }
    public bool GetHasObjectPicked()
    {
        return hasPickedObject;
    }
    public InteractableObject GetHandInteractableObject()
    {
        return interactableObject;
    }
    public InteractableObject GetNearestInteractableObject()
    {
        return nearestInteractableObject;
    }

    #endregion

    #region Setters
    public void ChangeObjectInHand(InteractableObject _interactableObject, bool _setParent = true)
    {
        if (interactableObject)
            RemoveItemFromHand();

        hasPickedObject = true;
        interactableObject = _interactableObject;
        if (!_interactableObject)
            return;
        
        _interactableObject.rb.isKinematic = true;

        if (!_setParent)
            return;
        _interactableObject.transform.position = GetObjectPickedPosition();
        _interactableObject.transform.rotation = transform.rotation;

        _interactableObject.transform.SetParent(transform.parent);
        _interactableObject.SetIsBeingUsed(true);

    }
    public InteractableObject RemoveItemFromHand()
    {

        hasPickedObject = false;
        interactableObject.transform.SetParent(null);
        interactableObject.rb.isKinematic = false;

        InteractableObject currentIO = interactableObject;
        interactableObject = null;

        if (currentIO)
            currentIO.SetIsBeingUsed(false);

        return currentIO;


    }
    public void ChangeNearestInteractableObject(InteractableObject _nearestObject)
    {
        if (_nearestObject != nearestInteractableObject)
        {
            if (nearestInteractableObject)
                nearestInteractableObject.GetSelectedVisual().Hide();

            if (!_nearestObject)
            {
                nearestInteractableObject = _nearestObject;
                hintController.UpdateActionType(HintController.ActionType.NONE);
                return;
            }

            if (_nearestObject.CanInteract(this))
                _nearestObject.GetSelectedVisual().Show();
            else
            {
                //Mostrar el interactable object que necesita

            }

            nearestInteractableObject = _nearestObject;
            
           
            hintController.UpdateActionType(nearestInteractableObject.ShowNeededInputHint(this));
        }
    }

    #endregion

    public void InstantiateItemInHand(ObjectSO _interactableObject)
    {
        InteractableObject item = Instantiate(_interactableObject.prefab).GetComponent<InteractableObject>();
        item.SetIsBeingUsed(true);
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.SetParent(transform.parent, true);
        item.transform.position = objectPickedPos.position;
        ChangeObjectInHand(item);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sphereCastTransform.position, sphereCastRadius);
    }
}
