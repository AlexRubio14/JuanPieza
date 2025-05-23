using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager instance;

    public struct PlayerData
    {
        public PlayerInput playerInput;
        public SinglePlayerController singlePlayer;
        public GameInput gameInput;
        public RumbleController rumbleController;
    }

    [field: SerializeField]
    public List<PlayerData> players {  get; private set; }
    public List<PlayerController> ingamePlayers {  get; private set; }

    [Space, SerializeField]
    private Material[] characterMat;

    [field: Space, SerializeField]
    public Sprite repairSprite {  get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        players = new List<PlayerData>();
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
        foreach (PlayerData item in players)
        {
            if (item.singlePlayer.currentColor == _material)
                return true;
        }

        return false;
    }
}
