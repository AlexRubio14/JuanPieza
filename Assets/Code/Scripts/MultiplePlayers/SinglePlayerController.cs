using UnityEngine;

public class SinglePlayerController : MonoBehaviour
{
    /* Este Script tiene que hacer lo siguiente
     * Tiene que ser don't destroy on load
     * Tiene que contener el player input
     * Tiene que contener variables de player como por ejemplo los cosmeticos
     */

    [field: SerializeField]
    public GameObject currentPlayerSelectorObject {  get; private set; }

    public int currentColor;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ChangePlayerColor(int _colorIndex)
    {
        currentColor = _colorIndex;
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        Material playerMaterial = PlayersManager.instance.GetMaterial(_colorIndex);
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.material = playerMaterial;
        }

        MeshRenderer hatRenderer = GetComponentInChildren<MeshRenderer>();
        if(hatRenderer)
            hatRenderer.material = playerMaterial;
    }
}
