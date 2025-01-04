using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    [SerializeField] protected ObjectSO objectSO;
    [SerializeField] protected SelectedVisual selectedVisual;
    protected bool isBeingUsed;

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

    public float GetWeight()
    {
        return objectSO.weight;
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

    public ObjectSO GetScriptableObject()
    {
        return objectSO;
    }
}
