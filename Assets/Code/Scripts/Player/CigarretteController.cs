using UnityEngine;
using System.Collections.Generic;
public class CigarretteController : MonoBehaviour
{
    [SerializeField] private List<GameObject> cigarrettes;

    private void Start()
    {
        //
    }

    public void ActivateCigarrette()
    {
        foreach (GameObject cigarrete in cigarrettes) 
        { 
            if(!cigarrete.activeInHierarchy)
            {
                cigarrete.SetActive(true);
                return;
            }
        }
    }
}
