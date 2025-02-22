using UnityEngine;
using UnityEngine.UI;

public class ObjectsTooltip : MonoBehaviour
{
    public enum ObjectState { None, Empty, Loaded, Cooldown, Broken, Repairing }
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
                canvas.gameObject.SetActive(true);
                image.sprite = emptySprite;
                image.color = Color.green;
                Debug.Log("Empty");
                break;
            case ObjectState.Loaded:
                canvas.gameObject.SetActive(true);
                image.sprite = loadedSprite;
                image.color = Color.blue;
                Debug.Log("Loaded");
                break;
            case ObjectState.Cooldown:
                canvas.gameObject.SetActive(true);
                image.sprite = cooldownSprite;
                image.color = Color.red;
                Debug.Log("Cooldown");
                break;
            case ObjectState.Broken:
                canvas.gameObject.SetActive(true);
                image.sprite = brokenSprite;
                image.color = Color.black;
                Debug.Log("Broken");
                break;
            case ObjectState.Repairing:
                canvas.gameObject.SetActive(true);
                image.sprite = repairingSprite;
                image.color = Color.yellow;
                Debug.Log("Repairing");
                break;
            default:
                canvas.gameObject.SetActive(false);
                Debug.Log("Default");
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
