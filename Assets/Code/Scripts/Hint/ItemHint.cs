using System.Collections.Generic;


public class ItemHint : Hint
{
    protected List<int> playersLooking;
    protected bool isEnabled;

    protected virtual void Start()
    {
        playersLooking = new List<int>();
    }
    protected override void Update()
    {
        base.Update();

        if(playersLooking.Count == 0 && isEnabled)
        {
            isEnabled = false;
            DisableAllHints();
        }

    }

    public override void EnableHint(HintController.ActionType _action, HintController.DeviceType _device)
    {
        isEnabled = true;
        base.EnableHint(_action, _device);
    }

    public void AddPlayer(int _playerId)
    {
        playersLooking.Add(_playerId);
    }
    public void RemovePlayer(int _playerId) 
    {
        playersLooking.Remove(_playerId);
    }
    public bool SomePlayerCanInteract(InteractableObject _currentObject)
    {
        foreach (int player in playersLooking)
        {
            PlayerController currentPlayer = PlayersManager.instance.ingamePlayers[player];
            if (_currentObject.CanInteract(currentPlayer.objectHolder))
                return true;
        }

        return false;
    }

}
