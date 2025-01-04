using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Votation : MonoBehaviour
{
    [Header("Direcction")]
    [SerializeField] private int direcctionValue;

    private List<PlayerController> currentsPlayers;

    private void Start()
    {
        currentsPlayers = new List<PlayerController>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && !other.GetComponent<PlayerController>().votationDone)
        {
            currentsPlayers.Add(other.GetComponent<PlayerController>());
            other.GetComponent<PlayerController>().votationDone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentsPlayers.Remove(other.GetComponent<PlayerController>());
            other.GetComponent<PlayerController>().votationDone = false;
        }
    }

    public List<PlayerController> GetCurrentsPlayer()
    {
        return currentsPlayers;
    }

    public int GetDirecctionValue()
    {
        return direcctionValue;
    }
}
