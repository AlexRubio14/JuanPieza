using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateManager : MonoBehaviour
{
    public static ClimateManager instance;

    [Header("General")]
    [SerializeField] private List<Material> skyboxs;
    [SerializeField] private List<AudioClip> ambientAudio;
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
    [SerializeField] private GameObject thunder;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] protected AudioClip strikeAudioClip;
    [SerializeField] protected AudioClip lightningAudioClip;
    private AudioSource lightningChargeAS;
    private GameObject affectedObject;
    private GameObject _lightningOrb;
    private float currentTime = 0;
    private bool preparingStrike = false;

    [Header("Snow")]
    [SerializeField] private GameObject snow;

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
        snow.SetActive(false);

        switch (NodeManager.instance.questData.questClimete)
        {
            case QuestData.QuestClimete.CLEAR:
                RenderSettings.skybox = skyboxs[0];
                break;
            case QuestData.QuestClimete.SNOW:
                RenderSettings.skybox = skyboxs[1];
                PrepareSnow();
                AudioManager.instance.Play2dLoop(ambientAudio[0], "Objects");
                break;
            case QuestData.QuestClimete.STORM:
                RenderSettings.skybox = skyboxs[2];
                AudioManager.instance.Play2dLoop(ambientAudio[1], "Objects");
                PrepareStorm();
                break;
        }
    }

    private void Update()
    {
        switch (currentClimate)
        {
            case QuestData.QuestClimete.SNOW:
                FreezeWeapon();
                currentClimate = QuestData.QuestClimete.CLEAR;
                break;
            case QuestData.QuestClimete.STORM:
                Lightning();
                break;
            default:
                break;
        }
    }

    #region storm 
    private void PrepareStorm()
    {
        rain.SetActive(true);
        RenderSettings.sun.intensity = sunIntensity;
        RenderSettings.sun.color = Color.blue;
    }
    private void Lightning()
    {
        if (preparingStrike)
            PreparedStrike();
        else
            WaitSpawnLightning();
    }

    private void PreparedStrike()
    {
        if(_lightningOrb == null)
        {
            preparingStrike = false;
            currentTime = 0;
            AudioManager.instance.StopLoopSound(lightningChargeAS);
            lightningChargeAS = null;
            return;
        }

        currentTime += Time.deltaTime;
        float progress = Mathf.Clamp01(currentTime / ligthningStrikeTime);
        float scaleValue = Mathf.Lerp(0f, lightningOrbMaxScale, progress);
        _lightningOrb.transform.localScale = Vector3.one * scaleValue;
        lightningChargeAS.volume = progress;

        if (currentTime >= ligthningStrikeTime)
        {
            currentTime = 0;
            Strike();
            preparingStrike = false;
            AudioManager.instance.StopLoopSound(lightningChargeAS);
            lightningChargeAS = null;
        }
    }

    private void Strike()
    {
        Destroy(_lightningOrb.gameObject);
        GameObject strike = Instantiate(thunder, affectedObject.transform.position, Quaternion.identity);
        StartCoroutine(DestroyStrike(strike, 2f));
        AudioManager.instance.Play2dOneShotSound(strikeAudioClip, "Objects");
        RaycastHit[] hits = Physics.SphereCastAll(affectedObject.transform.position, holeRadius, Vector3.forward, 0, hitLayer);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out PlayerController player))
            {
                player.stateMachine.ChangeState(player.stateMachine.stunedState);
            }
        }

        Camera.main.GetComponent<CameraShaker>().TriggerShake(cameraShakeDuration, cameraShakeMagnitude);
    }

    IEnumerator DestroyStrike(GameObject strike, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(strike);
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
        newlightningOrb.transform.localPosition = new Vector3(0,0.5f,0);
        _lightningOrb = newlightningOrb;
        lightningChargeAS = AudioManager.instance.Play2dLoop(lightningAudioClip, "Objects", 0f);
    }
    #endregion

    #region Snow
    private void PrepareSnow()
    {
        snow.SetActive(true);
    }

    public void FreezeWeapon()
    {
        List<InteractableObject> inventory = ShipsManager.instance.playerShip.GetInventory();
        foreach (InteractableObject item in inventory)
        {
            if(item.TryGetComponent(out FreezeWeapon freezeWeapon))
            {
                freezeWeapon.SetFreeze(true);
            }
        }

    }
    #endregion

    public void SetCurrentClimate()
    {
        currentClimate = NodeManager.instance.questData.questClimete;
    }
}
