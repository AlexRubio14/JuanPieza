using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager instance;

    [field: SerializeField]
    public List<(PlayerInput playerInput, SinglePlayerController singlePlayer)> players {  get; private set; }
    public List<PlayerController> ingamePlayers {  get; private set; }

    [Space, SerializeField]
    private Material[] characterMat;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        players = new List<(PlayerInput, SinglePlayerController)>();
        ingamePlayers = new List<PlayerController>();
    } 

    public Material GetMaterial(int _material)
    {
        return characterMat[_material];
    }
    public int GetNextMaterial(int _currentMaterial)
    {
        int nextMaterial = (_currentMaterial + 1) % characterMat.Length;
        
        
        if (IsMaterialUsed(nextMaterial))
            return GetNextMaterial(nextMaterial);

        return nextMaterial;
    }
    private bool IsMaterialUsed(int _material)
    {
        foreach ((PlayerInput, SinglePlayerController) item in players)
        {
            if (item.Item2.currentColor == _material)
                return true;
        }

        return false;
    }
}
