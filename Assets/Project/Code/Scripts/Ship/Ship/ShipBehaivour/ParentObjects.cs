using UnityEngine;

public class ParentObjects : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & mask) != 0)
        {
            if (other.TryGetComponent(out InteractableObject interactableObject))
                if (interactableObject.isBeginUsed)
                    return;
            if (other.TryGetComponent(out Weapon weapon))
                if (weapon.IsPlayerMounted())
                    return;
            other.transform.SetParent(transform);
        }
    }
}
