using UnityEngine;

public class WaterController : MonoBehaviour
{

    [SerializeField]
    private GameObject waterSplashParticles;

    [SerializeField]
    private AudioClip fallWaterClip;

    private Radio radioButtonClip;
    private AudioClip radioBreakClip;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
            Destroy(collision.gameObject);

        if (collision.collider.CompareTag("Object"))
        {
            if (collision.collider.TryGetComponent(out Radio _radio))
            {
                _radio.RadioFallAtWater();
                radioBreakClip = _radio.breakRadioClip;
                Invoke("BreakRadio", _radio.timeToBreakRadio);
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

    private void BreakRadio()
    {
        if (radioBreakClip)
        {
            Radio.BreakRadio(radioBreakClip);
            StartCoroutine(Radio.PlayMainMusic());
        }

    }
}

