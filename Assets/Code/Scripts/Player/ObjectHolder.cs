using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{

    [SerializeField] private List<GameObject> objectsInRangeToInteract;

    private GameObject selectedObject;
    private InteractableObject currentInteractableObject;

    [SerializeField] private Transform playerTransform;




    private void Awake()
    {
        objectsInRangeToInteract = new List<GameObject>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectNeareastItem();
    }

    // Cuando un objeto sale del rango de actuar se limpia de la lista
    private void ClearObject(GameObject _gameObject)
    {
        for (int i = 0; i < objectsInRangeToInteract.Count; i++)
        {
            if (objectsInRangeToInteract[i] == _gameObject)
            {
                objectsInRangeToInteract.RemoveAt(i);
                return;
            }
        }
    }

    // Recorre la lista de objetos que tienes en rango de actuar y selecciones el mas cercano al player
    private void SelectNeareastItem()
    {
        if (objectsInRangeToInteract.Count == 0)
            return;

        float currentdis = 100f;
        Vector3 disFromObjectToPlayer = Vector3.zero;
        int objectIndex = 0;

        for (int i = 0; i < objectsInRangeToInteract.Count; i++)
        {
            disFromObjectToPlayer = objectsInRangeToInteract[i].transform.position - playerTransform.position;
            if(disFromObjectToPlayer.magnitude < currentdis)
            {
                currentdis = disFromObjectToPlayer.magnitude;
                objectIndex = i;
            }
        }

        if(objectsInRangeToInteract[objectIndex] != selectedObject && currentInteractableObject != null)
        {
            currentInteractableObject.GetSelectedVisual().Hide();
        }

        selectedObject = objectsInRangeToInteract[objectIndex];

        currentInteractableObject = selectedObject.GetComponent<InteractableObject>();
        currentInteractableObject.GetSelectedVisual().Show();
    }

    public GameObject GetSelectedObject()
    {
        return selectedObject;
    }

    public InteractableObject GetInteractableObject()
    {
        return currentInteractableObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        objectsInRangeToInteract.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        ClearObject(other.gameObject);
        if(other.gameObject == selectedObject)
        {
            currentInteractableObject.GetSelectedVisual().Hide();
            currentInteractableObject = null;
        }
    }
}
