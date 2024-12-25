using UnityEngine;

public class SinglePlayerController : MonoBehaviour
{
    /* Este Script tiene que hacer lo siguiente
     * Tiene que ser don't destroy on load
     * Tiene que contener el player input
     * Tiene que contener variables de player como por ejemplo los cosmeticos
     * Al entrar estar en una escena 
     */

    [field: SerializeField]
    public GameObject currentPlayerSelectorObject {  get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
