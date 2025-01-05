using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private LayerMask itemsLayerMask;
    [SerializeField] private Transform sphereCastTransform;

    private InteractableObject nearestInteractableObject;

    private InteractableObject interactableObject;

    [SerializeField] private Transform objectPickedPos;

    private bool hasPickedObject = false;

    private RaycastHit[] colliders;

    private void Awake()
    {
        colliders = new RaycastHit[0];
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

            if (tempObject.isBeingUsed)
                continue;

            lastDistance = Vector3.Distance(transform.parent.position, item.collider.transform.position);

            currentinteractableObject = tempObject;

        }

        return currentinteractableObject;
    }
    private void ChangeNearestInteractableObject(InteractableObject _neareastObject)
    {
        if (_neareastObject != nearestInteractableObject)
        {
            if(nearestInteractableObject != null)
                nearestInteractableObject.GetSelectedVisual().Hide();

            if (_neareastObject != null)
            {
                if (_neareastObject.objectToInteract == interactableObject.objectSO)
                    _neareastObject.GetSelectedVisual().Show();
                else
                {
                    //Mostrar el interactable object que necesita

                }
            }
                

            nearestInteractableObject = _neareastObject;
        }
    }

    #region Getters
    public Vector3 GetObjectPickedPosition()
    {
        return objectPickedPos.position;
    }
    public bool GetHasObjectPicked()
    {
        return hasPickedObject;
    }
    public InteractableObject GetHandInteractableObject()
    {
        return interactableObject;
    }
    public InteractableObject GetNearestInteractableObject()
    {
        return nearestInteractableObject;
    }

    #endregion

    #region Setters
    public void SetHasObjectPicked(bool _value)
    {
        hasPickedObject = _value;
    }
    public void SetInteractableObject(InteractableObject _interactableObject)
    {
        interactableObject = _interactableObject;
    }
    #endregion

    public void InstantiateItem(ObjectSO _interactableObject)
    {
        GameObject item = Instantiate(_interactableObject.prefab);
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.SetParent(transform.parent, true);
        item.transform.position = objectPickedPos.position;
        SetInteractableObject(item.GetComponent<InteractableObject>());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sphereCastTransform.position, sphereCastRadius);
    }
}
