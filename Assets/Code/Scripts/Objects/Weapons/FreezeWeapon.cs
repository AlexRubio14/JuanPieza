using UnityEngine;

public class FreezeWeapon : MonoBehaviour
{
    [Header("Freeze")]
    [Range(1, 100)]
    [SerializeField] private float weaponFreezePercentage;
    [SerializeField] private float increaseFreezePercentage;
    [SerializeField] private float freezeMaxTime;
    private float weaponCurrentFreezePercentage;
    private float freezeCurrentTime;
    private bool freeze;
    private Weapon weapon;
    private ObjectState objectState;

    [Header("Ice")]
    [SerializeField] private GameObject iceCube;
    [SerializeField] private AudioClip breakIce;

    private void Start()
    {
        weapon = GetComponent<Weapon>();
        objectState = GetComponent<ObjectState>();
    }

    private void Update()
    {
        Freeze();
    }

    private void Freeze()
    {
        if (!freeze || weaponCurrentFreezePercentage == 100)
            return;

        freezeCurrentTime += Time.deltaTime;
        if(freezeCurrentTime > freezeMaxTime)
        {
            weaponCurrentFreezePercentage += increaseFreezePercentage;
            freezeCurrentTime = 0;
        }

        if(weaponCurrentFreezePercentage >= weaponFreezePercentage && !weapon.GetFreeze())
        {
            weapon.SetFreeze(true);
            objectState.SetIsBroke(true, true);
        }
    }

    public void SetFreeze(bool _freeze)
    { 
        freeze = _freeze; 
    }
}
