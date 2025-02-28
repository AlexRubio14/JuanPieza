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
            Destroy(collision.gameObject);

        //Instanciar particulas
        Vector3 splashPosition = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
        Instantiate(waterSplashParticles, splashPosition, Quaternion.identity);

        AudioManager.instance.Play2dOneShotSound(fallWaterClip, "Objects");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BoardingPirate"))
        {
            if (other.gameObject.TryGetComponent(out controllerPirateBoarding controller))
            {
                controller.ResetPirate();
            }
        }

        //Instanciar particulas
        Vector3 splashPosition = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
        Instantiate(waterSplashParticles, splashPosition, Quaternion.identity);

        AudioManager.instance.Play2dOneShotSound(fallWaterClip, "Objects");
    }
}

