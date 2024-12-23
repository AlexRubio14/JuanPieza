using UnityEngine;

public abstract class Object : iInteractable
{

    [SerializeField] protected string objectName;
    [SerializeField] protected float weight { get; private set; }
    SelectedVisual selectedVisual;

    public enum ObjectType { WEAPON, TOOL, DECORATION, RESOURCE };
    [SerializeField] protected ObjectType objectType;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
