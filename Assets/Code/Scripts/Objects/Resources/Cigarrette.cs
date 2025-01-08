using UnityEngine;

public class Cigarrette : Resource
{
    [Space, SerializeField] private float smokeTime;
    private float currentTime;
    private bool isSmoking;

    private void Start()
    {
        isSmoking = false;
        currentTime = smokeTime;
    }
    private void Update()
    {
        if (isSmoking)
        {
            //particulillas
            currentTime -= Time.deltaTime;
            if (currentTime <= 0) 
            {
                //cancelar animacion player
            }
        }
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        //playear animacion player
        isSmoking = true;
    }
}
