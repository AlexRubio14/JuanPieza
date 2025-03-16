public class ColorChanger : InteractableObject
{
    public override void Interact(ObjectHolder _objectHolder)
    {
        int playerId = _objectHolder.GetComponentInParent<PlayerController>().playerInput.playerReference;

        int colorId = PlayersManager.instance.GetNextMaterial(PlayersManager.instance.players[playerId].singlePlayer.currentColor);
        PlayersManager.instance.players[playerId].singlePlayer.ChangePlayerColor(colorId);

        _objectHolder.GetComponentInParent<IngamePlayerColorController>().SetPlayerColor(colorId) ;

    }
    public override void Use(ObjectHolder _objectHolder) { }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (CanInteract(_objectHolder))
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "change_color"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.NONE, "")
            };
    }

}
