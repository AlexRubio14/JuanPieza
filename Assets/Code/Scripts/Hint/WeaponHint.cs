using UnityEngine;
using UnityEngine.UI;

public class WeaponHint : RepairItemHint
{
    [Space, Header("Weapon Hint"), SerializeField]
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

    protected override void Start()
    {
        base.Start();
        currentWeapon = currentObject as Weapon;
    }
    private void FixedUpdate()
    {
        movementImage.gameObject.SetActive(currentWeapon.GetMountedPlayerId() != -1);

        if (currentWeapon.GetMountedPlayerId() != -1)
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
