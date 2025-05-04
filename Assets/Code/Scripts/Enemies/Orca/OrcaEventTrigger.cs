using UnityEngine;

public class OrcaEventTrigger : MonoBehaviour
{

    [SerializeField]
    private AudioClip splashClip;
    [SerializeField]
    private GameObject waterSplashParticles;
    public void PlaySplashSound()
    {
        AudioManager.instance.Play2dOneShotSound(splashClip, "Orca", 0.7f, 0.9f, 1.1f);
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0;
        Instantiate(waterSplashParticles, spawnPos, Quaternion.identity);
    }
}
