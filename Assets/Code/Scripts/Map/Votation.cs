using UnityEngine;

public class Votation : MonoBehaviour
{
    [Header("Direcction")]
    [SerializeField] protected int direcctionValue;

    protected int currentsPlayers;

    private void Start()
    {
        currentsPlayers = 0;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && !other.GetComponent<PlayerController>().votationDone)
        {
            currentsPlayers++;
            other.GetComponent<PlayerController>().votationDone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentsPlayers--;
            other.GetComponent<PlayerController>().votationDone = false;
        }
    }

    public int GetCurrentsPlayer()
    {
        return currentsPlayers;
    }

    public int GetDirecctionValue()
    {
        return direcctionValue;
    }
}
