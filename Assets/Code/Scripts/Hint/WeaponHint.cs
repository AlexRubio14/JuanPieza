using UnityEngine;
using UnityEngine.UI;

public class WeaponHint : ItemHint
{
    [SerializeField]
    private GameObject movementImage;
    [SerializeField] 
    private Image movementHintImage;
    [SerializeField]
    private Image rotationHintImage;
    [SerializeField]
    private Sprite[] movementSprite;
    [SerializeField]
    private Sprite[] rotationSprite;
    [SerializeField]
    private Vector3 offset;

    private Weapon currentWeapon;

    private void Awake()
    {
        currentWeapon = GetComponent<Weapon>();
    }


    private void FixedUpdate()
    {
        movementImage.gameObject.SetActive(currentWeapon.isBeginUsed);
        if (currentWeapon.isBeginUsed)
        {
            PlayerController player = PlayersManager.instance.ingamePlayers[currentWeapon.GetMountedPlayerId()];
            HintController.DeviceType device = player.GetComponent<HintController>().deviceType;
            movementHintImage.sprite = movementSprite[(int)device];
            rotationHintImage.sprite = rotationSprite[(int)device];
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
