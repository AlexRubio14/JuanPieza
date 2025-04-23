public class ItemHint : Hint
{
    protected int playersLooking;
    protected bool isEnabled;
    protected override void Update()
    {
        base.Update();
        if(playersLooking == 0 && isEnabled)
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
    public void AddPlayer()
    {
        playersLooking++;
    }
    public void RemovePlayer() 
    {
        playersLooking--;
    }
}
