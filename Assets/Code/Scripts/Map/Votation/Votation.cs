using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Votation : MonoBehaviour
{
    [Header("Direcction")]
    [SerializeField] private int direcctionValue;

    private List<(PlayerController, int)> currentsPlayers;
    private static Dictionary<PlayerController, Votation> globalPlayersInVotation = new Dictionary<PlayerController, Votation>();

    private void Start()
    {
        currentsPlayers = new List<(PlayerController, int)>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController _player = other.GetComponent<PlayerController>();

            if (globalPlayersInVotation.TryGetValue(_player, out var currentArea) && currentArea != this)
            {
                return;
            }

            if (currentsPlayers.Exists(p => p.Item2 == _player.playerInput.playerReference))
                return;

            currentsPlayers.Add((_player, _player.playerInput.playerReference));
            globalPlayersInVotation[_player] = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController _player = other.GetComponent<PlayerController>();
            currentsPlayers.Remove((_player, _player.playerInput.playerReference));

            if (globalPlayersInVotation.TryGetValue(_player, out var currentArea) && currentArea == this)
            {
                globalPlayersInVotation.Remove(_player);
            }
        }
    }

    public void CleanPlayerList()
    {
        currentsPlayers.Clear();
    }

    public List<(PlayerController, int)> GetCurrentsPlayer()
    {
        return currentsPlayers;
    }

    public int GetDirecctionValue()
    {
        return direcctionValue;
    }
}
