using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSO", menuName = "Scriptable Objects/ObjectSO")]
public class ObjectSO : ScriptableObject
{
    public enum ItemRarity { BASIC, RARE, EPIC, LEGENDARY }

    [field: SerializeField]
    public string objectName {  get; private set; }

    [field: SerializeField]
    public GameObject prefab { get; private set; }
    
    [field: SerializeField]
    public int price { get; private set; }
    
    [field: SerializeField]
    public float weight { get; private set; }

    [field: SerializeField]
    public ItemRarity rarity { get; private set; }

    public enum ObjectType { WEAPON, TOOL, DECORATION, RESOURCE, BOX };

    public ObjectType objectType;

    public enum ObjectSize { SMALL, MEDIUM, BIG }
    [field: SerializeField]
    public ObjectSize objectSize { get; private set; }

}
