using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractNPC : InteractableObject
{
    [SerializeField] private Image backGround;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string message;
    [SerializeField] private float maxTimeSpawnMessage;
    [SerializeField] private float maxTimeShowMessage;

    protected bool canInteract;

    [Space, SerializeField]
    protected AudioClip[] silabsNPC;
    [SerializeField]
    protected float speakingDuration;
    [SerializeField]
    protected float timeToSaySilab;
    [SerializeField]
    protected Vector2 pitch;
    protected List<AudioClip> lastSilabs;
    protected float currentSilabTime;
    protected float currentTime;
    protected int lastSilabsIndex;

    protected virtual void Start()
    {
        text.gameObject.SetActive(false);
        backGround.fillAmount = 0;
        canInteract = true;
        lastSilabs = new List<AudioClip>
        {
            null,
            null,
            null
        };
    }

    private void Update()
    {
        if (!canInteract)
        {
            currentSilabTime += Time.deltaTime;
            currentTime += Time.deltaTime;

            if (currentTime <= speakingDuration && currentSilabTime >= timeToSaySilab)
            {
                currentSilabTime -= timeToSaySilab;
                AudioClip silab = GetValidSilab();
                lastSilabs[lastSilabsIndex] = silab;
                lastSilabsIndex = (lastSilabsIndex + 1) % lastSilabs.Count;
                AudioManager.instance.Play2dOneShotSound(silab, "NPC", 1, pitch.x, pitch.y);                
            }
        }
    }

    protected virtual IEnumerator FillAndShowMessage()
    {
        canInteract = false;
        currentSilabTime = 0;
        currentTime = 0;

        float timer = 0f;
        while (timer < maxTimeSpawnMessage)
        {
            timer += Time.deltaTime;
            backGround.fillAmount = Mathf.Clamp01(timer / maxTimeSpawnMessage);
            yield return null;
        }

        text.text = message;
        text.gameObject.SetActive(true);

        yield return new WaitForSeconds(maxTimeShowMessage);

        text.gameObject.SetActive(false);

        timer = 0f;
        while (timer < maxTimeSpawnMessage)
        {
            timer += Time.deltaTime;
            backGround.fillAmount = Mathf.Clamp01(1 - (timer / maxTimeSpawnMessage));
            yield return null;
        }

        canInteract = true;
    }
    protected AudioClip GetValidSilab()
    {
        int silabId = Random.Range(0, silabsNPC.Length);
        AudioClip silab = silabsNPC[silabId];

        if (lastSilabs.Contains(silab))
        {
            silab = GetValidSilab();
        }

        return silab;
    }

    public override void Grab(ObjectHolder _objectHolder)
    {

    }

    public override void Release(ObjectHolder _objectHolder)
    {

    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        StartCoroutine(FillAndShowMessage());
    }

    public override void Use(ObjectHolder _objectHolder)
    {

    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return canInteract;
    }
}
