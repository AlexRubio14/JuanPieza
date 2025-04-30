using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [Space, SerializeField] 
	private Canvas canvas;

	[Serializable]
	public struct ImageDeviceSprite
	{
		public Image image;
		public Vector3 offset;
		public Sprite[] sprite;
	}
	[Tooltip("En la lista de imagenes en dispositivo el orden ha de ser Keyboard 0 y Gamepad 1"), SerializedDictionary("Action", "Image - Offset - Device - Sprites")]
    public SerializedDictionary<HintController.ActionType, ImageDeviceSprite> actionImages;

	[field: SerializeField]
	public HintController.ActionType useType { get; set; }
    [field: SerializeField]
    public HintController.ActionType interactType { get; set; }

	protected InteractableObject currentObject;

    protected virtual void Awake()
    {
        canvas.worldCamera = Camera.main;
        currentObject = GetComponent<InteractableObject>();
    }
    protected virtual void Update()
    {
        foreach (KeyValuePair<HintController.ActionType, ImageDeviceSprite> actionImage in actionImages)
		{
			if (actionImage.Value.image.gameObject.activeInHierarchy)
				actionImage.Value.image.transform.position = transform.position + actionImage.Value.offset;
		}
    }

    public virtual void EnableHint(HintController.ActionType _action, HintController.DeviceType _device)
	{
        actionImages[_action].image.gameObject.SetActive(true);
        actionImages[_action].image.sprite = actionImages[_action].sprite[(int)_device];
	}
	public void DisableHint(HintController.ActionType _action)
	{
		actionImages[_action].image.gameObject.SetActive(false);
    }
	public virtual void DisableAllHints()
	{
		foreach (KeyValuePair<HintController.ActionType, ImageDeviceSprite> actionImage in actionImages)
			actionImage.Value.image.gameObject.SetActive(false);
	}

    protected virtual void OnDrawGizmosSelected()
    {
		foreach (KeyValuePair<HintController.ActionType, ImageDeviceSprite> actionImage in actionImages) 
		{
			switch (actionImage.Key)
			{
				case HintController.ActionType.GRAB:
					Gizmos.color = Color.red;
					break;
				case HintController.ActionType.USE:
                    Gizmos.color = Color.blue;
                    break;
				case HintController.ActionType.HOLD_USE:
                    Gizmos.color = Color.cyan;
                    break;
				case HintController.ActionType.INTERACT:
					Gizmos.color = Color.green;
					break;
				case HintController.ActionType.HOLD_INTERACT:
					Gizmos.color = Color.yellow;
					break;
				default:
					break;
			}

			Gizmos.DrawWireSphere(transform.position + actionImage.Value.offset, 0.5f);
		}

    }
}
