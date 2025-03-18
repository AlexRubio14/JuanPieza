using UnityEngine;

public class ColorChanger : InteractableObject
{
    [Space, Header("Color Changer"), SerializeField]
    private GameObject changeColorParticles;
    [SerializeField] private AudioClip changeColorClip;

    public override void Interact(ObjectHolder _objectHolder)
    {
        int playerId = _objectHolder.GetComponentInParent<PlayerController>().playerInput.playerReference;

        int colorId = PlayersManager.instance.GetNextMaterial(PlayersManager.instance.players[playerId].singlePlayer.currentColor);
        PlayersManager.instance.players[playerId].singlePlayer.ChangePlayerColor(colorId);

        _objectHolder.GetComponentInParent<IngamePlayerColorController>().SetPlayerColor(colorId);

        Instantiate(changeColorParticles, _objectHolder.transform.parent.position, Quaternion.identity);

        AudioManager.instance.Play2dOneShotSound(changeColorClip, "Player", 1, 0.85f, 1.15f);

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
