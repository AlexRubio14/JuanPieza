using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    [SerializeField] protected string objectName;
    [SerializeField] protected float weight;
    [SerializeField] protected SelectedVisual selectedVisual;
    [SerializeField] protected bool isBeingUsed;

    [SerializeField]
    public Rigidbody rb;
    protected Collider objectCollider;

    public enum ObjectType { WEAPON, TOOL, DECORATION, RESOURCE };

    protected ObjectType objectType;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isBeingUsed = false;
    }

    public abstract void Interact(ObjectHolder _objectHolder);

    public abstract void UseItem(ObjectHolder _objectHolder);

    public float GetWeight()
    {
        return weight;
    }

    public SelectedVisual GetSelectedVisual()
    {
        return selectedVisual;
    }

    public bool GetIsBeingUsed()
    {
        return isBeingUsed;
    }

    public void SetIsBeingUsed(bool _value)
    {
        isBeingUsed = _value;
    }
}
