using UnityEngine;

public abstract class EnemyObject : MonoBehaviour
{
    [SerializeField]
    public EnemieManager enemieManager;
    [SerializeField]
    private Transform resourcePosition;

    public bool isBroken;

    [SerializeField]
    private GameObject fixedWeapon;
    [SerializeField]
    private GameObject brokenWeapon;

    public abstract void UseObject();

    public void BreakWeapon()
    {
        fixedWeapon.SetActive(false);
        brokenWeapon.SetActive(true);

        isBroken = true;
    }

    public void FixWeapon()
    {
        fixedWeapon.SetActive(true);
        brokenWeapon.SetActive(false);


        isBroken = false;
    }
}
