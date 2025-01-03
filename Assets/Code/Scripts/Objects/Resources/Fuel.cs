using UnityEngine;

public class Fuel : Resource
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        //Comprobar si el objeto de delante es un arma de combustible
        
        //Llamar a la funcion de llenar deposito

        Debug.Log("Uso combustible");
    }
}
