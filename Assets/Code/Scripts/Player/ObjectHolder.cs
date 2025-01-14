using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private LayerMask itemsLayerMask;
    [SerializeField] private Transform sphereCastTransform;

    [field: Space, Header("Audio"), SerializeField]
    public AudioClip pickUpClip;


    private InteractableObject nearestInteractableObject;

    private InteractableObject interactableObject;

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

        _interactableObject.transform.SetParent(transform.parent);
        
        _interactableObject.transform.position = objectPickedPos[(int)_interactableObject.objectSO.objectSize].position;
        _interactableObject.transform.position += _interactableObject.grabPivot.transform.localPosition;

        _interactableObject.transform.rotation = transform.rotation;
        _interactableObject.transform.forward = _interactableObject.grabPivot.transform.forward;


        foreach (Collider item in _interactableObject.GetComponents<Collider>())
            item.enabled = false;

        _interactableObject.SetIsBeingUsed(true);
    }
    public InteractableObject RemoveItemFromHand()
    {
        InteractableObject currentIO = interactableObject;

        hasPickedObject = false;
        interactableObject = null;

        if (currentIO)
        {
            currentIO.transform.SetParent(ShipsManager.instance.playerShip.transform);
            currentIO.SetIsBeingUsed(false);
            currentIO.rb.isKinematic = false;
            foreach (Collider item in currentIO.GetComponents<Collider>())
                item.enabled = true;


        }

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
                if (interactableObject)
                    hintController.UpdateActionType(interactableObject.ShowNeededInputHint(this));
                else
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
            
           if(!interactableObject || interactableObject  && interactableObject.objectToInteract == _nearestObject)
                hintController.UpdateActionType(nearestInteractableObject.ShowNeededInputHint(this));
        }
    }

    #endregion

    public InteractableObject InstantiateItemInHand(ObjectSO _interactableObject)
    {
        InteractableObject item = Instantiate(_interactableObject.prefab).GetComponent<InteractableObject>();
        item.SetIsBeingUsed(true);
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.SetParent(transform.parent, true);
        item.transform.position = objectPickedPos[(int)_interactableObject.objectSize].position;
        ChangeObjectInHand(item);
        AudioManager.instance.Play2dOneShotSound(pickUpClip, "Objects");
        return item;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sphereCastTransform.position, sphereCastRadius);
    }
}
