using UnityEngine;

public class Fuel : Resource
{
    public override void Use(ObjectHolder _objectHolder)
    {
        //Comprobar si el objeto de delante es un arma de combustible
        
        //Llamar a la funcion de llenar deposito

        Debug.Log("Uso combustible");
    }
}
