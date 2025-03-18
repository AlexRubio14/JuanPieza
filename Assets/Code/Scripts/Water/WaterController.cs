using UnityEngine;

public class WaterController : MonoBehaviour
{

    [SerializeField]
    private GameObject waterSplashParticles;

    [SerializeField]
    private AudioClip fallWaterClip;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Object"))
        {
            if (collision.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject))
            {
                if (interactableObject.objectSO.objectType == ObjectSO.ObjectType.WEAPON)
                    ShipsManager.instance.playerShip.GenerateCannon();
            }
            Destroy(collision.gameObject);
        }


        

        //Instanciar particulas
        Vector3 splashPosition = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
        Instantiate(waterSplashParticles, splashPosition, Quaternion.identity);

        if (collision.collider.CompareTag("BoardingPirate"))
        {
            if (collision.gameObject.TryGetComponent(out PirateBoardingController controller))
            {
                controller.ResetPirate();
            }
        }

        AudioManager.instance.Play2dOneShotSound(fallWaterClip, "Objects");
    }
}

