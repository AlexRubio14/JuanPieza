using UnityEngine;
using UnityEngine.UI;

public class WeaponHint : ItemHint
{
    [SerializeField]
    private Image movementImage;
    [SerializeField]
    private Sprite movementSprite;
    [SerializeField]
    private Sprite rotationSprite;
    [SerializeField]
    private Vector3 offset;

    private Weapon currentWeapon;

    private void Awake()
    {
        currentWeapon = GetComponent<Weapon>();
    }

    private void FixedUpdate()
    {
        movementImage.gameObject.SetActive(currentWeapon.isBeingUsed);
        if (currentWeapon.isBeingUsed)
        {
            if (currentWeapon.isRotating)
                movementImage.sprite = rotationSprite;
            else
                movementImage.sprite = movementSprite;

            movementImage.transform.position = transform.position + offset;
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, 0.5f);
    }
}
