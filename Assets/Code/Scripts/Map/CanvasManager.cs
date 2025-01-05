using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    [Header("Votation")]
    [SerializeField] private List<VotationUI> ui;
    [SerializeField] private VotationTimer timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVotationUIState(bool state)
    {
        foreach (var _ui in ui) 
        { 
            _ui.gameObject.SetActive(state);
        }
        timer.gameObject.SetActive(state);
    }

    public void SetInformationDestination(LevelNode node)
    {
        int i = 0;
        foreach (var _ui in ui)
        {
            _ui.SetDestinationText(node._nodeChildren[i]._node.name);
            i++;
        }
    }

    public void SetInformationPlayers(List<Votation> _votation)
    {
        int i = 0;
        foreach (var _ui in ui)
        {
            _ui.SetPlayers(_votation[i].GetCurrentsPlayer());
            i++;
        }
    }

    public void SetTimerUiInformation(float time, float maxTime)
    {
        timer.SetTimerUi(time, maxTime);
    }
}
