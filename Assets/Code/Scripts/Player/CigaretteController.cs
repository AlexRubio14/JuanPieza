using UnityEngine;
using System.Collections.Generic;
public class CigaretteController : MonoBehaviour
{
    [SerializeField] private List<GameObject> cigarrettes;

    private void Start()
    {
        foreach (GameObject cigarrete in cigarrettes)
            cigarrete.SetActive(false);

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
