using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestBoardObject : InteractableObject
{
    [SerializeField] private GameObject questCanvas;
    public override void Interact(ObjectHolder _objectHolder)
    {

        StartCoroutine(WaitEndOfFrame());
        IEnumerator WaitEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            questCanvas.SetActive(true);

            foreach ((PlayerInput, SinglePlayerController) player in PlayersManager.instance.players)
            {
                player.Item1.SwitchCurrentActionMap("MapMenu");
            }
        }
    }

    public void StopInteracting()
    {
        foreach ((PlayerInput, SinglePlayerController) player in PlayersManager.instance.players)
        {
            player.Item1.SwitchCurrentActionMap("Gameplay");
        }
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if(_objectHolder.GetHandInteractableObject() != null)
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.NONE, "")
            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.INTERACT, "open_map"),
            new HintController.Hint(HintController.ActionType.CANT_USE, "")
        };
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        
    }

    public GameObject GetQuestCanvas()
    {
        return questCanvas;
    }
}
