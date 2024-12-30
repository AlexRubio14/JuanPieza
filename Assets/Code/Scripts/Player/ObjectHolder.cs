using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{

    [SerializeField] private List<GameObject> objectsInRangeToInteract;
    [SerializeField] private GameObject objectPosition;

    private GameObject selectedObject;
    private InteractableObject currentInteractableObject;
    private GameObject objectPicked;

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

    /// <summary>
    /// Falta Hacer que al recorrer la list tener en cuenta si el objeto esta siendo utilizado para no contarlo
    /// </summary>
    private void SelectNeareastItem()
    {
        if (objectsInRangeToInteract.Count == 0)
            return;

        float currentdis = 100f;
        Vector3 disFromObjectToPlayer = Vector3.zero;
        int objectIndex = -1;

        for (int i = 0; i < objectsInRangeToInteract.Count; i++)
        {
            disFromObjectToPlayer = objectsInRangeToInteract[i].transform.position - transform.position;
            if(disFromObjectToPlayer.magnitude < currentdis)
            {
                currentdis = disFromObjectToPlayer.magnitude;
                objectIndex = i;
            }
        }

        if(objectIndex == -1)
        {
            selectedObject = null;
            currentInteractableObject = null;
            return;
        }

        // Si el Objeto ya no es el mas cercano que deje de brillar el outline
        if(objectsInRangeToInteract[objectIndex] != selectedObject && currentInteractableObject != null)
        {
            currentInteractableObject.GetSelectedVisual().Hide();
        }

        selectedObject = objectsInRangeToInteract[objectIndex];
        currentInteractableObject = selectedObject.GetComponent<InteractableObject>();
        
        //Si el objeto mas cercano esta siendo usado no le ponemos outline
        if(currentInteractableObject.GetIsBeingUsed() == false)
        {
            currentInteractableObject.GetSelectedVisual().Show();
        }
    }

    public void PickObject()
    {
        if (selectedObject == null)
            return;

        objectPicked = selectedObject;
        objectPicked.transform.position = objectPosition.transform.position;
        objectPicked.transform.rotation = objectPosition.transform.rotation;
        objectPicked.transform.SetParent(objectPosition.transform);
    }

    public GameObject GetSelectedObject()
    {
        return selectedObject;
    }

    public InteractableObject GetInteractableObject()
    {
        return currentInteractableObject;
    }

    public GameObject GetObjectPicked()
    {
        return objectPicked;
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
