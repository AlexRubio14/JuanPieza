using UnityEngine;

public class Geyser : MonoBehaviour
{
    public enum GeyserPhase { EMITTING_PARTICLES, ASCENDING, HOLDING_PEAK, DESCENDING };

    //particulas
    [SerializeField] private float particleDuration;
    [SerializeField] private float geyserDuration;
    private float timer;

    [SerializeField] private Vector3 geyserForce;

    private GeyserPhase currentPhase;

    private void OnEnable()
    {
        timer = 0f;
        currentPhase = GeyserPhase.EMITTING_PARTICLES;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        switch (currentPhase)
        {
            case GeyserPhase.EMITTING_PARTICLES:
                break;
            case GeyserPhase.ASCENDING:
                //subir el transform del geyser
                break;
            case GeyserPhase.HOLDING_PEAK:
                break;
            case GeyserPhase.DESCENDING:
                //bajar el transform del geyser
                break;
            default:
                break;
        }
    }

    public void ChangePhase(GeyserPhase phase)
    {
        switch (currentPhase)
        {
            case GeyserPhase.EMITTING_PARTICLES:
                DeactivateParticles();
                break;
            case GeyserPhase.ASCENDING:
                break;
            case GeyserPhase.HOLDING_PEAK:
                break;
            case GeyserPhase.DESCENDING:
                gameObject.SetActive(false);
                break;
            default:
                break;
        }

        switch (phase)
        {
            case GeyserPhase.EMITTING_PARTICLES:
                ActivateParticles();
                break;
            case GeyserPhase.ASCENDING:
                ActivateGeyser();
                break;
            case GeyserPhase.HOLDING_PEAK:
                break;
            case GeyserPhase.DESCENDING:
                break;
            default:
                break;
        }

        currentPhase = phase;
    }

    private void ActivateParticles()
    {

    }

    private void DeactivateParticles()
    {

    }

    private void ActivateGeyser()
    {

    }


}
