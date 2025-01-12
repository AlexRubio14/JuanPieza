using UnityEngine;

public class Fish : Resource
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //hola
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        Debug.Log("Uso pez");
    }
}

