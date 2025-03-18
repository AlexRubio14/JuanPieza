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
        Vector3 offset = new Vector3(0f,2.5f,0f);
        GameObject instantiatedDecal = Instantiate(petrolDecal, transform.position + offset, Quaternion.identity);
        instantiatedDecal.transform.forward = Vector3.down;
        instantiatedDecal.transform.SetParent(shipImpacted);
    }

}
