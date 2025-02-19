using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsTooltip : MonoBehaviour
{
    public enum ObjectState { Empty, Loaded, Cooldown, Broken, Repairing }
    private ObjectState currentState;
    private Camera camera;
    
    [Header("UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image image;
    [SerializeField] private float offset;

    [Header("Weapon Images")] 
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite loadedSprite;
    [SerializeField] private Sprite cooldownSprite;
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private Sprite repairingSprite;

    void Start()
    {
        canvas.worldCamera = Camera.main;

        UpdateStateDisplay();
    }

    public void SetState(ObjectState newState)
    {
        currentState = newState;
        UpdateStateDisplay();
    }

    private void UpdateStateDisplay()
    {
        switch (currentState)
        {
            case ObjectState.Empty:
                image.sprite = emptySprite;
                break;
            case ObjectState.Loaded:
                image.sprite = loadedSprite;
                break;
            case ObjectState.Cooldown:
                image.sprite = cooldownSprite;
                break;
            case ObjectState.Broken:
                image.sprite = brokenSprite;
                break;
            case ObjectState.Repairing:
                image.sprite = repairingSprite;
                break;
        }
    }

    private void Update()
    {
        if (image.gameObject.activeInHierarchy)
        {
            Vector3 tooltipPos = transform.position + new Vector3(0, offset, 0);
            image.transform.position = tooltipPos;

        }
    }
}
