using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    [SerializeField] protected string objectName;
    [SerializeField] protected float weight;
    [SerializeField] protected SelectedVisual selectedVisual;
    [SerializeField] protected bool isBeingUsed;

    public enum ObjectType { WEAPON, TOOL, DECORATION, RESOURCE };

    protected ObjectType objectType;


    private void Awake()
    {
        isBeingUsed = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Interact(ObjectHolder _objectHolder);

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
