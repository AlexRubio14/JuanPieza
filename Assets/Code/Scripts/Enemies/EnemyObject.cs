using UnityEngine;

public abstract class EnemyObject : MonoBehaviour
{
    [SerializeField]
    public EnemieManager enemieManager;

    public bool isBroken;

    [SerializeField]
    private GameObject fixedWeapon;
    [SerializeField]
    private GameObject brokenWeapon;

    public abstract void UseObject();

    public abstract void OnBreakObject();
    public abstract void OnFixObject();

    public virtual void BreakObject()
    {
        fixedWeapon.SetActive(false);
        brokenWeapon.SetActive(true);

        isBroken = true;

        OnBreakObject();
    }

    public virtual void FixObject()
    {
        fixedWeapon.SetActive(true);
        brokenWeapon.SetActive(false);


        isBroken = false;
        OnFixObject();
    }
}
