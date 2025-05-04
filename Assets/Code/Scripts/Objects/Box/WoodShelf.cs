using System.Collections.Generic;
using UnityEngine;

public class WoodShelf : Box
{
    [Header("Decorations")]
    [SerializeField] private List<GameObject> decorations;
    private int currentDecorationInShelf;

    [Header("Force")]
    [SerializeField] private float forceMultiplier;

    [Header("Tutorial")]
    [SerializeField] private ShowMessageRepair message;

    protected override void Start()
    {
        AddItemInBox(false, 4);
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!CanInteract(_objectHolder) || state.GetIsBroken())
            return;

        Tool handObject = _objectHolder.GetHandInteractableObject() as Tool;
        handObject.addToolAtDestroy = false;
        AddItemInBox(true);
        InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
        Destroy(currentObject.gameObject);
    }

    public override void AddItemInBox(bool _makeSound, int cuantity = 1)
    {
        if (itemsInBox + cuantity > 4)
            return;

        decorations[itemsInBox].SetActive(true);
        base.AddItemInBox(_makeSound, cuantity);
    }
    public override void RemoveItemInBox()
    {
        decorations[itemsInBox - 1].SetActive(false);

        base.RemoveItemInBox();

        if(message)
            message.TakeHarmer();
    }
    public override void OnBreakObject()
    {
        if (itemsInBox == 0)
            return;

        if(itemsInBox >= 2)
            ThrowItems(2);
        else if(itemsInBox == 1)
            ThrowItems(1);
    }

    private void ThrowItems(int cuantity)
    {
        for(int i = 0; i<cuantity; i++) 
        {
            GameObject _objectSO = Instantiate(itemDropped.prefab, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
            float randomX = Random.value > 0.5f ? 1f : -1f; 
            float randomZ = Random.value > 0.5f ? 1f : -1f; 

            Vector3 force = new Vector3(randomX, 1f, randomZ) * forceMultiplier;

            _objectSO.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            RemoveItemInBox();
        }
    }
}
