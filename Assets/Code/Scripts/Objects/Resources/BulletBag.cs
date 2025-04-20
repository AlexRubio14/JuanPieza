using UnityEngine;

public class BulletBag : Resource, ICatapultAmmo
{
    [Space, Header("Bullet Bag"), SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float bulletsToSpawn;
    [SerializeField]
    private float bulletSpawnOffset;


    public override void Release(ObjectHolder _objectHolder)
    {
        if ((this as ICatapultAmmo).LoadItemInCatapult(_objectHolder, this))
            return;

        base.Release(_objectHolder);
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        _objectHolder.RemoveItemFromHand();
        PlayerController controller = _objectHolder.GetComponentInParent<PlayerController>();
        controller.animator.SetBool("Pick", false);

        //Spawnear X balas
        float angleIncrement = 360 / bulletsToSpawn;
        float currentAngle = 0;

        for (int i = 0; i < bulletsToSpawn; i++)
        {
            Quaternion spawnRotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 spawnDir = spawnRotation * controller.transform.forward;
            Vector3 spawnPos = controller.transform.position + spawnDir * bulletSpawnOffset;

            Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

            currentAngle += angleIncrement;
        }

        Destroy(gameObject);
    }
    public override void Use(ObjectHolder _objectHolder) { }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return _objectHolder.GetHandInteractableObject() == this;
    }
    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (_objectHolder.GetHandInteractableObject() == this)
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                new HintController.Hint(HintController.ActionType.USE, "open")
            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, bulletSpawnOffset);
    }
}
