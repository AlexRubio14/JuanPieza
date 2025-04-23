using UnityEngine;

public class Quest9Script : MonoBehaviour
{
    private void Update()
    {
        foreach (PlayerController controller in PlayersManager.instance.ingamePlayers)
        {
            Debug.LogWarning("AQUI HEMOS DE SPAMEAR EL HINT DE LA Q PARA EMPUJAR");
            //controller.hintController.UpdateActionType(data);
        }
    }
}
