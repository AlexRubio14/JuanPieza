using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    [field: SerializeField] public ObjectSO objectSO {  get; protected set; }
    [field: SerializeField] public ObjectSO objectToInteract {  get; protected set; }
    [SerializeField] protected SelectedVisual selectedVisual;
    public bool isBeingUsed { get; protected set; }

    [SerializeField]
    public Rigidbody rb;
    protected Collider objectCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isBeingUsed = false;
    }

    public abstract void Interact(ObjectHolder _objectHolder);

    public abstract void UseItem(ObjectHolder _objectHolder);

    public SelectedVisual GetSelectedVisual()
    {
        return selectedVisual;
    }

    public void SetIsBeingUsed(bool _value)
    {
        isBeingUsed = _value;
    }

}
