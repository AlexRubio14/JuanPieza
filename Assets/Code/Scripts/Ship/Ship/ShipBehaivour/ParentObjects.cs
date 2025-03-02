using UnityEngine;

public class ParentObjects : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & mask) != 0)
        {
            if(other.TryGetComponent<Weapon>(out Weapon weapon)) 
                if(weapon.isPlayerMounted())
                    return;
            other.transform.parent = transform;
        }
    }
}
