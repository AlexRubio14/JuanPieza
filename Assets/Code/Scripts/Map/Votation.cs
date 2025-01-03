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
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            currentsPlayers++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentsPlayers--;
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
