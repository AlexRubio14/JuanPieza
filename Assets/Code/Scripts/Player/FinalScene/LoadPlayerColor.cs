using UnityEngine;

public class LoadPlayerColor : MonoBehaviour
{
    [SerializeField]
    private int playerId;
    
    void Start()
    {
        if (PlayersManager.instance.players.Count <= playerId)
            return;

        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        Material playerMaterial = PlayersManager.instance.GetMaterial(PlayersManager.instance.players[playerId].singlePlayer.currentColor);
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.material = playerMaterial;
        }

        MeshRenderer hatRenderer = GetComponentInChildren<MeshRenderer>();
        if (hatRenderer)
            hatRenderer.material = playerMaterial;
    }
}
