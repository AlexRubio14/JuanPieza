using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AYellowpaper.SerializedCollections;

public class RumbleController : MonoBehaviour
{
    public enum RumbleForce { VERY_SOFT, SOFT, MEDIUM, HARD, VERY_HARD }

    [SerializedDictionary("Hardness", "Force")]
    public SerializedDictionary<RumbleForce, float> rumbleForces;

    [Serializable]
    public struct RumblePressets
    {
        public RumbleForce motorForces;
        public float rumbleDuration;
        private float rumbleTimeLeft;

        public float GetTimeLeft() { return rumbleTimeLeft; }
        public void SetTimeLeft(float _timeLeft)
        {
            rumbleTimeLeft = _timeLeft;
        }
    }

    private List<RumblePressets> rumblePressets;

    private Gamepad currentGamepad = null;
    [SerializeField]
    private RumbleForce debugForce;

    [SerializeField]
    private bool debugRumble = false;

    private void Awake()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();

        if (playerInput.devices[0] is Gamepad)
            currentGamepad = playerInput.devices[0] as Gamepad;

        rumblePressets = new List<RumblePressets>();
    }

    private void FixedUpdate()
    {
        if (debugRumble && currentGamepad != null)
        {
            float currentForce = rumbleForces[debugForce];
            currentGamepad.SetMotorSpeeds(currentForce, currentForce);
        }

        RumbleUpdate();

        
    }
    private void OnDestroy()
    {
        if (currentGamepad != null)
            currentGamepad.SetMotorSpeeds(0, 0);
    }

    private void RumbleUpdate()
    {
        if (currentGamepad == null)
            return;

        if (rumblePressets.Count <= 0)
        {
            currentGamepad.SetMotorSpeeds(0, 0);
            return;
        }

        UpdateRumblePressets();

        float maxRumbleForce = GetMaxRumbleForce();

        currentGamepad.SetMotorSpeeds(maxRumbleForce, maxRumbleForce);
    }
    private void UpdateRumblePressets()
    {
        List<RumblePressets> rumbleToRemove = new List<RumblePressets>();
        for (int i = 0; i < rumblePressets.Count; i++)
        {
            RumblePressets rumble = rumblePressets[i];
            rumble.SetTimeLeft(rumblePressets[i].GetTimeLeft() - Time.fixedDeltaTime);

            if(rumble.GetTimeLeft() <= 0)
                rumbleToRemove.Add(rumblePressets[i]);

            rumblePressets[i] = rumble;
        }

        foreach (RumblePressets item in rumbleToRemove)
            rumblePressets.Remove(item);
    }
    private float GetMaxRumbleForce()
    {
        float maxRumbleForce = 0;
        foreach (RumblePressets item in rumblePressets)
        {
            float currentForce = rumbleForces[item.motorForces];
            currentForce *= item.rumbleDuration / item.GetTimeLeft();

            if (currentForce > maxRumbleForce)
                maxRumbleForce = currentForce;
        }

        return maxRumbleForce;
    }

    public void AddRumble(RumblePressets _rumblePresset)
    {
        if(currentGamepad != null)
        {
            _rumblePresset.SetTimeLeft(_rumblePresset.rumbleDuration);
            rumblePressets.Add(_rumblePresset);
        }
    }


}
