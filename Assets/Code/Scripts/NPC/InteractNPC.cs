using System.Collections;
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

    protected override void Start()
    {
        text.gameObject.SetActive(false);
        backGround.fillAmount = 0;
        canInteract = true;
    }

    protected virtual IEnumerator FillAndShowMessage()
    {
        canInteract = false;
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

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        return new HintController.Hint[]
          {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
          };
    }
}
