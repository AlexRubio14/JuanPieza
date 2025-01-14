using System.Collections;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [field: Header("Interactable Object"), SerializeField] 
    public ObjectSO objectSO {  get; protected set; }
    [field: SerializeField] 
    public ObjectSO objectToInteract {  get; protected set; }
    [SerializeField] 
    protected SelectedVisual selectedVisual;
    public bool isBeingUsed { get; protected set; }

    [SerializeField]
    public Rigidbody rb;
    [field: SerializeField]
    public Transform grabPivot {  get; protected set; }
    protected Collider objectCollider;

    public bool hasToBeInTheShip = true;
    
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isBeingUsed = false;
    }
    protected virtual void Start()
    {
        StartCoroutine(AddObjectToShip());
    }

    public abstract void Interact(ObjectHolder _objectHolder);
    public virtual void StopInteract(ObjectHolder _objectHolder) { }

    public abstract void UseItem(ObjectHolder _objectHolder);
    public SelectedVisual GetSelectedVisual()
    {
        return selectedVisual;
    }

    public void SetIsBeingUsed(bool _value)
    {
        isBeingUsed = _value;
    }

    public virtual bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        
        return 
            handObject && objectToInteract == handObject.objectSO || 
            !_objectHolder.GetHandInteractableObject() && !objectToInteract;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(AddObjectToShip());
    }
    protected virtual void OnDestroy()
    {
        if (ShipsManager.instance && hasToBeInTheShip && ShipsManager.instance.playerShip)
            ShipsManager.instance.playerShip.RemoveInteractuableObject(this);
    }
    public virtual HintController.ActionType ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (handObject && objectToInteract == handObject.objectSO || !_objectHolder.GetHandInteractableObject() && !objectToInteract)
        {
            return HintController.ActionType.INTERACT;
        }
 
        return HintController.ActionType.NONE;
    }

    protected IEnumerator AddObjectToShip()
    {
        yield return new WaitForEndOfFrame();
        if (ShipsManager.instance && hasToBeInTheShip && ShipsManager.instance.playerShip)
            ShipsManager.instance.playerShip.AddInteractuableObject(this);
    }

}
