using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    [SerializeField] protected string objectName;
    [SerializeField] private float weight;
    [SerializeField] SelectedVisual selectedVisual;
    [SerializeField] private bool isBeingUsed;

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

    protected abstract void Interact();
    //protected abstract void UseItem();

    public float GetWeight()
    {
        return weight;
    }

    public SelectedVisual GetSelectedVisual()
    {
        return selectedVisual;
    }
}
