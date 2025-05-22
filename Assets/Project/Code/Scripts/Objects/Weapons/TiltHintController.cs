using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class TiltHintController : MonoBehaviour
{
    [SerializeField]
    private GameObject tiltObject;
    [SerializeField]
    private Vector3 uiOffset;

    [Space, SerializeField]
    private SpriteRenderer tiltUI;

    [SerializeField, SerializedDictionary("Device", "Sprites")]
    private SerializedDictionary<HintController.DeviceType, Sprite> tiltSprites;

    private Transform followPosition;

    // Update is called once per frame
    void Update()
    {
        tiltObject.transform.position = followPosition.position + uiOffset;
        transform.forward = Camera.main.transform.forward;
    }

    public void ChangeDeviceType(bool _isGamepad, Transform _followPos)
    {
        HintController.DeviceType device = _isGamepad ? HintController.DeviceType.GAMEPAD : HintController.DeviceType.KEYBOARD;

        tiltUI.sprite = tiltSprites[device];
        followPosition = _followPos;
    }
}
