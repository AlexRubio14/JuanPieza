using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    [SerializeField]
    private LayerMask floorMask;
    [SerializeField]
    private AudioClip[] footsteps;
    [SerializeField]
    private AudioClip[] iceFootsteps;

    private List<AudioClip> lastFootsteps;
    private int lastFootstepIndex;
    [SerializeField]
    private bool playFootstep;
    [SerializeField]
    private float footstepVolume;
    [SerializeField]
    private GameObject footstepParticle;
    private PlayerController controller;
    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();

        lastFootsteps = new List<AudioClip>
        {
            null,
            null,
            null
        };
        lastFootstepIndex = 0;
    }

    public void PlayFootstep()
    {
        if (footstepParticle)
        {
            GameObject particles = Instantiate(footstepParticle, transform.position, Quaternion.identity);
            particles.transform.forward = -transform.forward;

        }

        if (!playFootstep || controller && controller.stateMachine.currentState is not MoveState or IdleState or DrunkState or CannonState)
            return;

        Physics.Raycast(transform.parent.position, Vector3.down, out RaycastHit hit, 1.5f, floorMask);
        if (!hit.collider)
            return;

        AudioClip footstep;
        if (!hit.collider.CompareTag("Ice"))
        {
            footstep = GetValidFootstep(footsteps);
            lastFootsteps[lastFootstepIndex] = footstep;
            lastFootstepIndex = (lastFootstepIndex + 1) % lastFootsteps.Count;
        }
        else
        {
            footstep = GetValidFootstep(iceFootsteps);
            lastFootsteps[lastFootstepIndex] = footstep;
            lastFootstepIndex = (lastFootstepIndex + 1) % lastFootsteps.Count;
        }

        AudioManager.instance.Play2dOneShotSound(footstep, "Player", footstepVolume, 0.7f, 1.3f);
    }

    protected AudioClip GetValidFootstep(AudioClip[] _footsteps)
    {
        int footstepId = Random.Range(0, _footsteps.Length);
        AudioClip footstep = _footsteps[footstepId];

        if (lastFootsteps.Contains(footstep))
        {
            footstep = GetValidFootstep(_footsteps);
        }

        return footstep;
    }
}
