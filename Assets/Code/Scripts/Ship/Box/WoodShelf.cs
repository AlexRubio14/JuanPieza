using System.Collections.Generic;
using UnityEngine;

public class WoodShelf : Box
{
    [Header("Decorations")]
    [SerializeField] private List<GameObject> decorations;
    private int currentDecorationInShelf;

    [Header("Force")]
    [SerializeField] private float forceMultiplier;

    protected override void Start()
    {
        base.Start();
    }
    public override void AddItemInBox()
    {
        base.AddItemInBox();
        if(currentDecorationInShelf < 4)
        {
            currentDecorationInShelf++;
            decorations[currentDecorationInShelf - 1].SetActive(true);
        }
    }

    public override void RemoveItemInBox()
    {
        base.RemoveItemInBox();

        currentDecorationInShelf--;
        decorations[currentDecorationInShelf].SetActive(false);
    }

    public override void OnBreakObject()
    {
        base.OnBreakObject();
        switch (itemsInBox)
        {
            case 0:
                break;
            case 1:
                ThrowItems(1);
                break;
            default:
                ThrowItems(2);
                break;
        }
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
