using UnityEngine;

public class Quest9Script : MonoBehaviour
{
    [SerializeField] private HintActionData pushHint;
    private HintController.Hint[] data;

    private void Start()
    {
        data = new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.USE, pushHint.HintId),
                new HintController.Hint(HintController.ActionType.CANT_INTERACT, "")
            };
    }

    private void Update()
    {
        foreach (PlayerController controller in PlayersManager.instance.ingamePlayers)
        {
            controller.hintController.UpdateActionType(data);
        }
    }
}
