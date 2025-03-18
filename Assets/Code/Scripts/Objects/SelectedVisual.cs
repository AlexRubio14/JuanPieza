using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour
{
    [SerializeField] private Outline[] outlineArray;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        foreach (Outline outline in outlineArray)
        {

            outline.enabled = true;
        }
    }

    public void Hide()
    {
        foreach (Outline outline in outlineArray)
        {
            outline.enabled = false;
        }
    }
}
