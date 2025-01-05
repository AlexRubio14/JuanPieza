using System.Collections.Generic;
using UnityEngine;

public class WoodShelf : Box
{
    [Header("Decorations")]
    [SerializeField] private List<GameObject> decorations;
    private int currentDecorationInShelf;

    private void Start()
    {
        currentDecorationInShelf = 4;
        itemsInBox = 4;
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
}
