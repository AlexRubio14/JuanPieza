using UnityEngine;

public class DetectWeightItem : MonoBehaviour
{
    [Header("ShipInformation")]
    [SerializeField] private Ship shipWeigth;
    private void OnCollisionEnter(Collision collision)
    {
        //Aqui comparar si tiene peso el objeto, si tiene sumarle el peso al barco
    }

    private void OnCollisionExit(Collision collision)
    {
        //Aqui comparar si tiene peso el objeto, si tiene restarle el peso al barco
    }
}
