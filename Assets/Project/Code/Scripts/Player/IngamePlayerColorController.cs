using System.Collections;
using UnityEngine;

public class IngamePlayerColorController : MonoBehaviour
{

    private PlayerController playerController;
    private SkinnedMeshRenderer[] renderers;
    private MeshRenderer hatRenderer;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        hatRenderer = GetComponentInChildren<MeshRenderer>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitEndOfFrame());
        IEnumerator WaitEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            SetPlayerColor(PlayersManager.instance.players[playerController.playerInput.playerReference].singlePlayer.currentColor);
        }
    }

    public void SetPlayerColor(int _color)
    {
        Material currentMat = PlayersManager.instance.GetMaterial(_color);

        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.material = currentMat;
        }

        hatRenderer.material = currentMat;
    }
}
