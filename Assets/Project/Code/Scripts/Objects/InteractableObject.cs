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
    public bool isBeginUsed { get; protected set; }
    [field: SerializeField]
    protected bool canGrab;
    [field: SerializeField]
    public bool canUse {  get; protected set; }
    [SerializeField]
    public Rigidbody rb;
    [field: SerializeField]
    public Transform grabPivot {  get; protected set; }
    protected Collider objectCollider;

    public bool hasToBeInTheShip = true;

    public ItemHint hint {  get; protected set; }
    
    protected virtual void Awake()
    {
        //tooltip = GetComponent<ObjectsTooltip>();
        
        rb = GetComponent<Rigidbody>();
        isBeginUsed = false;
        hint = GetComponent<ItemHint>();
    }


    public abstract void Grab(ObjectHolder _objectHolder);
    public abstract void Release(ObjectHolder _objectHolder);
    public abstract void Interact(ObjectHolder _objectHolder);
    public virtual void StopInteract(ObjectHolder _objectHolder) { }

    public abstract void Use(ObjectHolder _objectHolder);
    public virtual void StopUse(ObjectHolder _objectHolder) { }
    public SelectedVisual GetSelectedVisual()
    {
        return selectedVisual;
    }
    public void SetIsBeingUsed(bool _value)
    {
        isBeginUsed = _value;
    }

    public virtual bool CanGrab(ObjectHolder _objectHolder)
    {
        return !_objectHolder.GetHandInteractableObject() && canGrab;
    }
    public abstract bool CanInteract(ObjectHolder _objectHolder);
    protected virtual void OnEnable()
    {
        StartCoroutine(AddObjectToShip());
    }
    protected IEnumerator AddObjectToShip()
    {
        yield return new WaitForEndOfFrame();
        if (ShipsManager.instance && hasToBeInTheShip && ShipsManager.instance.playerShip)
            ShipsManager.instance.playerShip.AddInteractuableObject(this);
    }

    protected virtual void OnDestroy()
    {
        if (ShipsManager.instance && hasToBeInTheShip && ShipsManager.instance.playerShip)
            ShipsManager.instance.playerShip.RemoveInteractuableObject(this);
    }

}
