using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClimateManager : MonoBehaviour
{
    public static ClimateManager instance;

    [Header("General")]
    [SerializeField] private List<Material> skyboxs;
    private QuestData.QuestClimete currentClimate;

    [Header("Storm")]
    [SerializeField] private float sunIntensity;
    [SerializeField] private float ligthningSpawnTime;
    [SerializeField] private float ligthningStrikeTime;
    [SerializeField] private float lightningOrbMaxScale;
    [SerializeField] private float holeRadius;
    [SerializeField] private float cameraShakeDuration;
    [SerializeField] private float cameraShakeMagnitude;
    [SerializeField] private GameObject lightningOrb;
    [SerializeField] private GameObject rain;
    [SerializeField] private LayerMask hitLayer;
    private GameObject affectedObject;
    private GameObject _lightningOrb;
    private float currentTime = 0;
    private bool preparingStrike = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        rain.SetActive(false);

        switch (NodeManager.instance.questData.questClimete)
        {
            case QuestData.QuestClimete.CLEAR:
                RenderSettings.skybox = skyboxs[0];
                break;
            case QuestData.QuestClimete.SNOW:
                RenderSettings.skybox = skyboxs[1];
                break;
            case QuestData.QuestClimete.STORM:
                RenderSettings.skybox = skyboxs[2];
                PreparedStorm();
                break;
        }
    }

    private void PreparedStorm()
    {
        rain.SetActive(true);
        RenderSettings.sun.intensity = sunIntensity;
        RenderSettings.sun.color = Color.blue;
    }

    private void Update()
    {
        switch (currentClimate)
        {
            case QuestData.QuestClimete.SNOW:
                break;
            case QuestData.QuestClimete.STORM:
                Lightning();
                break;
            default:
                break;
        }
    }

    #region storm 
    private void Lightning()
    {
        if (preparingStrike)
            PreparedStrike();
        else
            WaitSpawnLightning();
    }

    private void PreparedStrike()
    {
        currentTime += Time.deltaTime;
        float progress = Mathf.Clamp01(currentTime / ligthningStrikeTime);
        float scaleValue = Mathf.Lerp(0f, lightningOrbMaxScale, progress);
        _lightningOrb.transform.localScale = Vector3.one * scaleValue;

        if (currentTime >= ligthningStrikeTime)
        {
            currentTime = 0;
            Strike();
            preparingStrike = false;
        }
    }

    private void Strike()
    {
        affectedObject.GetComponent<InteractableObject>().DestroyLightning();
        //Instanciar rayo
        RaycastHit[] hits = Physics.SphereCastAll(affectedObject.transform.position, holeRadius, Vector3.forward, 0, hitLayer);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out PlayerController player))
            {
                Debug.Log("stunned");
            }
        }

        Camera.main.GetComponent<CameraShaker>().TriggerShake(cameraShakeDuration, cameraShakeMagnitude);
    }

    private void WaitSpawnLightning()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= ligthningSpawnTime)
        {
            currentTime = 0;
            ChooseAffectedItem();
            preparingStrike = true;
        }

    }

    private void ChooseAffectedItem()
    {
        List<InteractableObject> inventory = ShipsManager.instance.playerShip.GetInventory();
        if (inventory.Count == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, inventory.Count);
        affectedObject = inventory[randomIndex].gameObject;

        GameObject newlightningOrb = Instantiate(lightningOrb, Vector3.zero, Quaternion.identity);
        newlightningOrb.transform.SetParent(affectedObject.transform, true);
        newlightningOrb.transform.localScale = Vector3.zero;
        newlightningOrb.transform.localPosition = new Vector3(0,1,0);
        _lightningOrb = newlightningOrb;
        affectedObject.GetComponent<InteractableObject>().SetLightning(newlightningOrb);
    }
    #endregion


    public void SetCurrentClimate()
    {
        currentClimate = NodeManager.instance.questData.questClimete;
    }
}
