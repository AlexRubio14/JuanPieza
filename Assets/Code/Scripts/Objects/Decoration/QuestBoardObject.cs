using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestBoardObject : InteractableObject
{
    [SerializeField] private GameObject questCanvas;
    [SerializeField]
    private AudioClip openBoardClip;
    public override void Grab(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }
    public override void Release(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }
    public override void Use(ObjectHolder _objectHolder) { }
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

        AudioManager.instance.Play2dOneShotSound(openBoardClip, "Objects", 0.75f, 0.9f, 1.1f);
    }

    public override bool CanGrab(ObjectHolder _objectHolder)
    {
        return false;
    }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return !_objectHolder.GetHandInteractableObject();
    }

    public GameObject GetQuestCanvas()
    {
        return questCanvas;
    }
    public void StopInteracting()
    {
        foreach ((PlayerInput, SinglePlayerController) player in PlayersManager.instance.players)
        {
            player.Item1.SwitchCurrentActionMap("Gameplay");
        }
    }


}
