using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private LayerMask itemsLayerMask;
    [SerializeField] Transform sphereCastTransform;

    (InteractableObject, Collider) interactableObject = (null, null);

    [SerializeField] private Transform objectPickedPos;

    private bool hasPickedObject = false;

    public RaycastHit[] colliders;

    private void Awake()
    {
        colliders = new RaycastHit[0];
    }

    // Update is called once per frame
    void Update()
    {
        (InteractableObject, Collider) neareastObject = CheckItemsInRange();
        ChangeInteractableObject(neareastObject);
    }

    // Crea un sphereRaycast, te pilla el interactableObject mas cercano que no este siendo utilizado y te devuelve su script y su collider a la vez.
    // Si no hay ningun item devuelve (null, null);
    public (InteractableObject, Collider) CheckItemsInRange()
    {
        colliders = Physics.SphereCastAll(sphereCastTransform.position, sphereCastRadius, transform.forward, sphereCastRadius, itemsLayerMask);

        (InteractableObject, Collider) currentinteractableObject = (null, null);

        float lastDistance = 1000f;

        foreach (RaycastHit item in colliders) 
        {
            if(Vector3.Distance(transform.parent.position, item.collider.transform.position) >= lastDistance)
            {
                continue;
            }

            InteractableObject tempObject = item.collider.GetComponent<InteractableObject>();

            if (tempObject.GetIsBeingUsed())
                continue;

            lastDistance = Vector3.Distance(transform.parent.position, item.collider.transform.position);

            currentinteractableObject = (tempObject, item.collider);

        }

        return currentinteractableObject;
    }

    private void ChangeInteractableObject((InteractableObject, Collider) neareastObject)
    {
        if (hasPickedObject)
            return;

        if (neareastObject != interactableObject)
        {
            if(interactableObject.Item1 != null)
            {
                interactableObject.Item1.GetSelectedVisual().Hide();
            }

            if(neareastObject.Item1 != null)
            {
                neareastObject.Item1.GetSelectedVisual().Show();
            }

            interactableObject = neareastObject;
        }
    }

    public InteractableObject GetInteractableObject()
    {
        return interactableObject.Item1;
    }
    public Vector3 GetObjectPickedPosition()
    {
        return objectPickedPos.position;
    }

    public bool GetHasObjectPicked()
    {
        return hasPickedObject;
    }

    public void SetHasObjectPicked(bool _value)
    {
        hasPickedObject = _value;
    }

    public void InstantiateItem(InteractableObject _interactableObject, Collider _interactableObjectCollider)
    {
        GameObject item = Instantiate(_interactableObject.gameObject);
        item.transform.SetParent(transform.parent, true);
        item.transform.localPosition = objectPickedPos.position;
        SetInteractableObject(_interactableObject, _interactableObjectCollider);
    }

    public void SetInteractableObject(InteractableObject _interactableObject, Collider _interactableObjectCollider)
    {
        interactableObject.Item1 = _interactableObject;
        interactableObject.Item2 = _interactableObjectCollider;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sphereCastTransform.position, sphereCastRadius);
    }
}
