using UnityEngine;
using UnityEngine.UI;

public class ObjectsTooltip : MonoBehaviour
{
    public enum ObjectType { Weapon, Object }
    public enum ObjectState { None, Interacting, Empty, Loaded, Cooldown, Broken, Repairing }
    
    [Header("Objects Tooltip")]
    [SerializeField] private ObjectType objectType = ObjectType.Object;

    private ObjectState currentState;
    
    [Header("UI")]
    [SerializeField] private GameObject progressBarGO;
    [SerializeField] private float size = 1.5f;
    [SerializeField] private float offset = 3.5f;
    private Canvas canvas;
    private Image image;
    public ProgressBarController progressBar { get; private set; }
    
    [Header("Object Images")]
    [SerializeField] private Sprite interactingSprite;
    [SerializeField] private Sprite brokenSprite;

    [Header("Weapon Images")] 
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite loadedSprite;

    private int playersInteracting = 0;
    void Start()
    {
        CreateCanvas();
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
            case ObjectState.Interacting:
                SetImage(interactingSprite);
                break;
            case ObjectState.Broken:
                SetImage(brokenSprite);
                break;
            case ObjectState.Empty:
                SetImage(emptySprite);
                break;
            case ObjectState.Loaded:
                SetImage(loadedSprite);
                break;
            case ObjectState.Cooldown:
                SetProgressBar(Color.red);
                break;
            case ObjectState.Repairing: 
                SetProgressBar(Color.green);
                break;
            default:
                if (progressBar != null)
                    progressBar.gameObject.SetActive(false);
                
                image.gameObject.SetActive(false);
                break;
        }
    }

    private void Update()
    {
        if (image.gameObject.activeInHierarchy)
        {
            Vector3 tooltipPos = transform.position + new Vector3(0, offset, 0);

            image.transform.position = tooltipPos;
            
            if (progressBar != null)
                progressBar.transform.position = tooltipPos;
        }
    }

    public void AddPlayer()
    {
        playersInteracting++;
    }
    public void RemovePlayer()
    {
        playersInteracting--;
    }

    public int GetTotalPlayers()
    {
        return playersInteracting;
    }
    private void CreateCanvas()
    {
        GameObject canvasGO = new GameObject("Canvas");
        canvasGO.transform.SetParent(transform);
    
        canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.planeDistance = 50;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 101;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject imageGO = new GameObject("HintImage");
        imageGO.transform.SetParent(canvasGO.transform);

        RectTransform rt = imageGO.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(size, size);
        rt.anchoredPosition = Vector2.zero;
        rt.localRotation = new Quaternion(0, 0, 0, 0);
        
        image = imageGO.AddComponent<Image>();
        image.color = Color.white;
        
        imageGO.transform.SetParent(canvasGO.transform);

        if (progressBarGO != null)
            CreateProgressBar(canvasGO.transform);

    }

    private void CreateProgressBar(Transform parent)
    {
        if (progressBarGO != null)
        {
            GameObject progressBarInstance = Instantiate(progressBarGO, parent);
        
            progressBar = progressBarInstance.GetComponent<ProgressBarController>();
        
            RectTransform rt = progressBarInstance.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning(gameObject.name + " no tiene progress bar");
        }
    }

    private void SetProgressBar(Color color)
    {
        if (progressBar != null)
        {
            image.gameObject.SetActive(false);
            progressBar.gameObject.SetActive(true);
            
            Transform child = progressBar.transform.GetChild(0);
            Image childImage = child.GetComponent<Image>();
            childImage.color = color;
        }
    }

    private void SetImage(Sprite sprite)
    {
        image.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(false);
        
        image.sprite = sprite;
    }
}
