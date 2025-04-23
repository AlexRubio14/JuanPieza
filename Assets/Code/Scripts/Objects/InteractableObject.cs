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
    [SerializeField] 
    protected ObjectsTooltip tooltip;
    public bool isBeingUsed { get; protected set; }
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

    private GameObject lightning;

    public ItemHint hint {  get; protected set; }
    
    protected virtual void Awake()
    {
        if (TryGetComponent(out ObjectsTooltip _tooltip))
            tooltip = _tooltip;
        
        rb = GetComponent<Rigidbody>();
        isBeingUsed = false;
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

    public ObjectsTooltip GetTooltip()
    {
        return tooltip;
    }
    public void SetIsBeingUsed(bool _value)
    {
        isBeingUsed = _value;
    }

    public virtual bool CanGrab(ObjectHolder _objectHolder)
    {
        return !_objectHolder.GetHandInteractableObject() && canGrab;
    }
    public abstract bool CanInteract(ObjectHolder _objectHolder);

    protected virtual void OnDestroy()
    {
        if (ShipsManager.instance && hasToBeInTheShip && ShipsManager.instance.playerShip)
            ShipsManager.instance.playerShip.RemoveInteractuableObject(this);
    }


    public void SetLightning(GameObject _lightning)
    {
        lightning = _lightning;
    }

    public void DestroyLightning()
    {
        Destroy(lightning);
    }

}
