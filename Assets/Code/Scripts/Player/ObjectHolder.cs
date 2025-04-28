using System.Collections;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private LayerMask itemsLayerMask;
    [SerializeField] private Transform sphereCastTransform;

    [field: Space, Header("Audio"), SerializeField]
    public AudioClip pickUpClip;


    private InteractableObject nearestInteractableObject;

    private InteractableObject handObject;

    [SerializeField] private Transform[] objectPickedPos;

    private bool hasPickedObject = false;

    private RaycastHit[] colliders;

    public PlayerController playerController {  get; private set; }

    private void Awake()
    {
        colliders = new RaycastHit[0];
        playerController = GetComponentInParent<PlayerController>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InteractableObject neareastObject = CheckItemsInRange();
        ChangeNearestInteractableObject(neareastObject);
    }

    // Crea un sphereRaycast, te pilla el interactableObject mas cercano que no este siendo utilizado y te devuelve su script y su collider a la vez.
    public InteractableObject CheckItemsInRange()
    {
        colliders = Physics.SphereCastAll(sphereCastTransform.position, sphereCastRadius, transform.forward, sphereCastRadius, itemsLayerMask);

        InteractableObject currentinteractableObject = null;

        float lastDistance = 1000f;

        foreach (RaycastHit item in colliders) 
        {
            if(Vector3.Distance(transform.parent.position, item.collider.transform.position) >= lastDistance)
            {
                continue;
            }

            InteractableObject tempObject = item.collider.GetComponent<InteractableObject>();

            if (!tempObject || 
                (tempObject.isBeginUsed && (tempObject is not Weapon)) ||
                !tempObject.CanGrab(this) && !tempObject.CanInteract(this) &&
                (tempObject is not Repair || !(tempObject as Repair).GetObjectState().GetIsBroken()) &&
                (tempObject is not Weapon || !(tempObject as Weapon).GetFreeze())
                )
                continue;

            lastDistance = Vector3.Distance(transform.parent.position, item.collider.transform.position);

            currentinteractableObject = tempObject;

        }

        return currentinteractableObject;
    }

    #region Getters
    public bool GetHasObjectPicked()
    {
        return hasPickedObject;
    }
    public InteractableObject GetHandInteractableObject()
    {
        return handObject;
    }
    public InteractableObject GetNearestInteractableObject()
    {
        return nearestInteractableObject;
    }

    #endregion

    #region Setters
    public void ChangeObjectInHand(InteractableObject _interactableObject, bool _setParent = true)
    {
        if (handObject)
            RemoveItemFromHand();

        hasPickedObject = true;
        handObject = _interactableObject;

        if (!_interactableObject)
            return;
        
        _interactableObject.rb.isKinematic = true;

        if (!_setParent)
            return;

        
        _interactableObject.transform.SetParent(objectPickedPos[(int)_interactableObject.objectSO.objectSize]);

        _interactableObject.transform.localPosition = -_interactableObject.grabPivot.transform.localPosition;


        _interactableObject.transform.localRotation = Quaternion.identity;
        _interactableObject.transform.localRotation = _interactableObject.grabPivot.transform.localRotation;

        foreach (Collider item in _interactableObject.GetComponents<Collider>())
            item.enabled = false;

        _interactableObject.SetIsBeingUsed(true);
    }
    public InteractableObject RemoveItemFromHand()
    {
        InteractableObject currentIO = handObject;

        hasPickedObject = false;
        handObject = null;

        if (currentIO)
        {
            Transform parent = ShipsManager.instance ? ShipsManager.instance.playerShip.transform : null;
            currentIO.transform.SetParent(parent);
            currentIO.SetIsBeingUsed(false);
            currentIO.rb.isKinematic = false;
            foreach (Collider item in currentIO.GetComponents<Collider>())
                item.enabled = true;
        }

        return currentIO;
    }
    public void ChangeNearestInteractableObject(InteractableObject _nearestObject)
    {
        if (handObject && nearestInteractableObject && _nearestObject == handObject)
        {
            nearestInteractableObject.hint.RemovePlayer(playerController.playerInput.playerReference);
            nearestInteractableObject = null;
            return;
        }

        if(nearestInteractableObject == _nearestObject)
            return;

        if (nearestInteractableObject)
        {
            nearestInteractableObject.hint.RemovePlayer(playerController.playerInput.playerReference);
            nearestInteractableObject.GetSelectedVisual().Hide();
        }

        if (!_nearestObject)
        {
            nearestInteractableObject = _nearestObject;
            return;
        }


        if (_nearestObject.CanGrab(this) || _nearestObject.CanInteract(this))
            _nearestObject.GetSelectedVisual().Show();
        
        if (_nearestObject is Repair)
        {
            Repair repair = _nearestObject as Repair;
            if (repair.GetObjectState().GetIsBroken())
                _nearestObject.GetSelectedVisual().Show();
        }

        _nearestObject.hint.AddPlayer(playerController.playerInput.playerReference);
        nearestInteractableObject = _nearestObject;
    }

    #endregion

    public InteractableObject InstantiateItemInHand(ObjectSO _interactableObject)
    {
        InteractableObject item = Instantiate(_interactableObject.prefab).GetComponent<InteractableObject>();
        item.SetIsBeingUsed(true);
        item.rb.isKinematic = true;
        StartCoroutine(SetItemParent(item));
        AudioManager.instance.Play2dOneShotSound(pickUpClip, "Objects");

        IEnumerator SetItemParent(InteractableObject interactableObject)
        {
            yield return new WaitForEndOfFrame();

            ChangeObjectInHand(interactableObject);
        }

        return item;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sphereCastTransform.position, sphereCastRadius);
    }
}
