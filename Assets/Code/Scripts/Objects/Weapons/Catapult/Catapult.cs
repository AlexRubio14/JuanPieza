using AYellowpaper.SerializedCollections;
using NUnit.Framework.Constraints;
using UnityEngine;
using VInspector.Libs;

public class Catapult : Weapon
{
    public enum AmmoType { USELESS, EXPLOSIVE }
    [Space, Header("Catapult"), SerializeField]
    [SerializedDictionary("Action", "Device Sprites")]
    private SerializedDictionary<ObjectSO, GameObject> ammoPrefabs;
    [SerializeField]
    private GameObject weakObjectPrefab;
    [SerializeField]
    private Transform catapultAmmoSpawnPos;
    [SerializeField]
    private float catapultAmmoRotationSpeed;
    [SerializeField]
    private GameObject spoonObject;

    private ObjectSO shootObject;
    private Mesh meshToThrow;

    protected override void Shoot()
    {
        if (!ammoPrefabs.ContainsKey(shootObject))
        {
            hasAmmo = false;
            return;
        }


        GameObject ammoPrefab = ammoPrefabs[shootObject];

        bool weakObject = ammoPrefab == null;

        if (weakObject)
            ammoPrefab = weakObjectPrefab;

        GameObject newBullet = Instantiate(ammoPrefab, catapultAmmoSpawnPos.transform.position, Quaternion.identity);

        if (weakObject)
        {
            //Desactivamos todos los hijos del spoon 
            for (int i = 0; i < spoonObject.transform.childCount; i++)
            {
                Transform currentChild = spoonObject.transform.GetChild(0);
                currentChild.SetParent(newBullet.transform);
                currentChild.transform.localPosition = Vector3.zero;

            }
        }

        Rigidbody bulletRB =  newBullet.GetComponent<Rigidbody>();
        bulletRB.AddForce(catapultAmmoSpawnPos.forward * bulletForce, ForceMode.Impulse);
        bulletRB.AddRelativeTorque(
            new Vector3(
                Random.Range(-catapultAmmoRotationSpeed, catapultAmmoRotationSpeed), 
                Random.Range(-catapultAmmoRotationSpeed, catapultAmmoRotationSpeed), 
                Random.Range(-catapultAmmoRotationSpeed, catapultAmmoRotationSpeed) 
                )
            , ForceMode.VelocityChange);
        newBullet.GetComponent<Bullet>().SetDamage(weaponDamage);
        hasAmmo = false;
        AudioManager.instance.Play2dOneShotSound(weaponShootClip, "Objects", 0.4f, 0.9f, 1.1f);
    }

    public override void Reload(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        //Copiar el mesh del objeto que tienes en la mano y lo ponemos en la bala
        GameObject newMesh = CopyMeshRecursive(handObject.transform, spoonObject.transform);
        newMesh.transform.localPosition = Vector3.zero;
        
        shootObject = handObject.objectSO;

        base.Reload(_objectHolder);
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
        {
            return base.CanInteract(_objectHolder);
        }

        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();

        return !isPlayerMounted() && !handObject /*Montarse*/ || isPlayerMounted() && playerCont.playerInput.playerReference == mountedPlayerId /*Bajarse*/ || !hasAmmo && handObject /*Recargar*/ ;

    }
    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
            return base.ShowNeededInputHint(_objectHolder);

        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();

        if (!hasAmmo)
            tooltip.SetState(ObjectsTooltip.ObjectState.Empty);

        if (!handObject && !isPlayerMounted()) //No tiene nada en la mano y no hay nadie montado
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "mount"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (isPlayerMounted() && !hasAmmo && playerCont.playerInput.playerReference == mountedPlayerId) //Si lo esta utilizando y no esta cargado
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "dismount"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (!hasAmmo && handObject)//Si no esta cargado y tiene la bala en la mano
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "load_bullet"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (hasAmmo && playerCont.playerInput.playerReference == mountedPlayerId) //Si esta cargado y el player esta montado
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "dismount"),
                new HintController.Hint(HintController.ActionType.USE, "shoot")
            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }

    private GameObject CopyMeshRecursive(Transform _original, Transform _parent)
    {
        // Crear nuevo GameObject
        GameObject copy = new GameObject(_original.name);

        // Copiar posición/rotación/escala locales
        copy.transform.localPosition = _original.localPosition;
        copy.transform.localRotation = _original.localRotation;
        copy.transform.localScale = _original.localScale;

        // Asignar padre si hay
        if (_parent != null)
            copy.transform.SetParent(_parent, false);

        // Copiar MeshFilter si existe
        MeshFilter originalFilter = _original.GetComponent<MeshFilter>();
        if (originalFilter != null)
        {
            MeshFilter copyFilter = copy.AddComponent<MeshFilter>();
            copyFilter.sharedMesh = originalFilter.sharedMesh;
        }

        // Copiar MeshRenderer si existe
        MeshRenderer originalRenderer = _original.GetComponent<MeshRenderer>();
        if (originalRenderer != null)
        {
            MeshRenderer copyRenderer = copy.AddComponent<MeshRenderer>();
            copyRenderer.sharedMaterials = originalRenderer.sharedMaterials;
        }

        // Recursivamente copiar hijos
        foreach (Transform child in _original)
        {
            CopyMeshRecursive(child, copy.transform);
        }

        return copy;
    }

}
