using UnityEngine;

public class NPCController : MonoBehaviour
{
    private enum NpcAnimation {  SMOKE, MALBORO, MOVE_HEAD_SMOKE, DRINK, DANCE}
    [SerializeField] private NpcAnimation currentAnimation;
    private Animator animator;
    [SerializeField] private AudioClip sound;
    [SerializeField] private float maxValue;
    [SerializeField] private ParticleSystem smokeParticles;
    void Start()
    {
        animator = GetComponent<Animator>();

        switch (currentAnimation)
        {
            case NpcAnimation.SMOKE:
                animator.SetTrigger("Smoke");
                animator.SetBool("SmokeUp", true);
                break;
            case NpcAnimation.MALBORO:
                animator.SetTrigger("Malboro");
                animator.SetBool("TakeMalboro", true);
                break;
            case NpcAnimation.MOVE_HEAD_SMOKE:
                animator.SetTrigger("SmokeSit");
                animator.SetBool("MoveHead", true);
                break;
            case NpcAnimation.DRINK:
                animator.SetTrigger("Drink");
                animator.SetBool("DrinkBear", true);
                break;
            case NpcAnimation.DANCE:
                animator.SetTrigger("Dance");
                animator.SetBool("DanceUp", true);
                break;
        }
    }

    public void AmbientSound()
    {
        int randomValue = (int)Random.Range(1, maxValue + 1);

        if(randomValue == 1)
            AudioManager.instance.Play2dOneShotSound(sound, "Object");

        if (currentAnimation == NpcAnimation.SMOKE)
            smokeParticles.Play();
    }


}
