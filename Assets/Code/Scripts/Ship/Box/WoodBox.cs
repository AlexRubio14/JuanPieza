using System.Collections.Generic;
using UnityEngine;

public class ItemBox : Box
{
    [Header("Decorations")]
    [SerializeField] private List<GameObject> decorations;
    private int currentDecoration;

    private void Start()
    {
        UpdateDecorations();
    }
    public override void AddItemInBox()
    {
        base.AddItemInBox();
        UpdateDecorations();
    }

    public override void RemoveItemInBox()
    {
        base.RemoveItemInBox();
        UpdateDecorations();
    }

    private void UpdateDecorations()
    {
        for (int i = 0; i < decorations.Count; i++)
            decorations[i].SetActive(false);

        SetCurrentDecorationValue();

        if (currentDecoration == 3)
            return;

        decorations[currentDecoration].SetActive(true);
    }

    private void SetCurrentDecorationValue()
    {
        if(itemsInBox == 0)
            currentDecoration = 3;
        else if (itemsInBox <= 5)
            currentDecoration = 2;
        else if (itemsInBox <= 10)
            currentDecoration = 1;
        else
            currentDecoration = 0;

    }
}

