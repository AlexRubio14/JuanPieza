using UnityEngine;

public class DetectPlayerInIce : MonoBehaviour
{
    public LayerMask playerLayer;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.SetOnIce(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.SetOnIce(false);
        }
    }
}
