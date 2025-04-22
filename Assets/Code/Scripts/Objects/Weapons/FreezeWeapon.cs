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
    [SerializeField] private Vector3 initPosition;
    [SerializeField] private float maxScale;
    [SerializeField] private AudioClip breakIce;
    private GameObject iceCubeWeapon;

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
        ScaleIceCube();

        if (freezeCurrentTime > freezeMaxTime)
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

    public void BreakIce()
    {
        if (!freeze)
            return;
        weapon.SetFreeze(false);
        objectState.SetIsBroke(false, true);
        freezeCurrentTime = 0;
        weaponCurrentFreezePercentage = 0;
        iceCubeWeapon.transform.localScale = Vector3.zero;
    }

    private void ScaleIceCube()
    {
        float nextFreezePercentage = weaponCurrentFreezePercentage + increaseFreezePercentage;
        float t = freezeCurrentTime / freezeMaxTime;
        float visualFreezePercentage = Mathf.Lerp(weaponCurrentFreezePercentage, nextFreezePercentage, t);
        float scale = Mathf.Lerp(0, maxScale, visualFreezePercentage / 100f);
        iceCubeWeapon.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void GenerateIceCube()
    {
        iceCubeWeapon = Instantiate(iceCube);
        iceCubeWeapon.transform.SetParent(this.transform, true);
        iceCubeWeapon.transform.localScale = Vector3.zero;
        iceCubeWeapon.transform.localPosition = initPosition;
    }

    public void SetFreeze(bool _freeze)
    { 
        freeze = _freeze;
        if (freeze)
            GenerateIceCube();
    }
}
