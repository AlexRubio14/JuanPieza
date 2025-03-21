using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class VotationUI : MonoBehaviour
{
    [Header("Information")]
    [SerializeField] private TextMeshProUGUI destination;
    [SerializeField] private TextMeshProUGUI players;

    public void SetDestinationText(string destinationText)
    {
        destination.text = destinationText;
    }

    public void SetPlayers(List<(PlayerController, int)> _players) 
    {
        string playerNames = "";
        foreach (var player in _players)
        {
            playerNames += "J" + (player.Item2+1) + "  "; 
        }
        players.text = playerNames;
    }
}
