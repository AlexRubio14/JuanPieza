using UnityEngine;

public class PetrolBarrel : Bullet
{
    [SerializeField] private GameObject petrolDecal;

    private void OnDestroy()
    {
        Explode();
    }

    private void Explode()
    {
        Instantiate(petrolDecal);
    }

}
