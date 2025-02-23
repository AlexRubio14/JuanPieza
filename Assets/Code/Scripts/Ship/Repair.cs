using System.Collections.Generic;
using UnityEngine;

public class Repair : InteractableObject
{
    [Space, Header("Repair Item")]
    [SerializeField] protected ObjectSO repairItem;
    [SerializeField] protected ObjectState state;
    [SerializeField] private float repairDuration;
    private float currentRepairTime;
    private List<PlayerController> players = new List<PlayerController>();
    [SerializeField]
    private ParticleSystem repairParticles;
    private AudioSource repairAudioSource;
    private bool clipIsPlaying = false;

    [SerializeField] protected AudioClip repairClip;

    protected override void Start()
    {
        base.Start();
        repairParticles.Stop(true);
    }

    protected virtual void Update()
    {
        RepairObject();
    }

    #region Object Fuctions

    public override void Interact(ObjectHolder _objectHolder)
    {
    }

    public override void Use(ObjectHolder _objectHolder)
    {
    }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return handObject && handObject.objectSO == repairItem;
    }
    #endregion

    #region Repair Functions
    public void AddPlayer(ObjectHolder _objectHolder)
    {
        if (!state.GetIsBroken() || !CanInteract(_objectHolder))
            return;

        PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();
        playerCont.animator.SetBool("Interacting", true);
        playerCont.progressBar.EnableProgressBar(true);
        players.Add(playerCont);
        playerCont.stateMachine.ChangeState(playerCont.stateMachine.repairState);

        if (!clipIsPlaying)
        {
            repairAudioSource = AudioManager.instance.Play2dLoop(repairClip, "Objects");
            clipIsPlaying = true;
        }

    }
    public void RemovePlayer(ObjectHolder _objectHolder)
    {
        PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();
        playerCont.animator.SetBool("Interacting", false);
        playerCont.progressBar.EnableProgressBar(false);
        players.Remove(playerCont);
        playerCont.stateMachine.ChangeState(playerCont.stateMachine.idleState);

        if (players.Count == 0)
        {
            AudioManager.instance.StopLoopSound(repairAudioSource);
            clipIsPlaying = false;
        }
    }
    private void RepairObject()
    {
        if (players.Count > 0)
        {
            if (repairParticles.isStopped)
                repairParticles.Play(true);
            
            currentRepairTime += players.Count * Time.deltaTime;
            if (currentRepairTime >= repairDuration)
            {
                FinishRepairing();
                currentRepairTime = 0;
            }
            tooltip.SetState(ObjectsTooltip.ObjectState.Repairing);
            tooltip.progressBar.SetProgress(currentRepairTime, repairDuration);
            
            //foreach (PlayerController player in players)
                //player.progressBar.SetProgress(currentRepairTime, repairDuration);
        }
        else
        {
            if (repairParticles.isPlaying)
                repairParticles.Stop(true);
            
            if (state.GetIsBroken() && tooltip != null)
                tooltip.SetState(ObjectsTooltip.ObjectState.Broken);
            
            currentRepairTime = 0;
        }
    }
    private void FinishRepairing()
    {
        for (int i = players.Count - 1; i >= 0; i--)
        {
            ObjectHolder objHolder = players[i].objectHolder;
            InteractableObject handObject = objHolder.GetHandInteractableObject();

            StopInteract(objHolder);
            StopUse(objHolder);
            if (handObject)
            {
                handObject.StopInteract(objHolder);
                handObject.StopUse(objHolder);
            }
            RepairEnded(objHolder);
        }
    }
    public bool IsPlayerReparing(PlayerController _playerController)
    {
        if (players.Count <= 0)
            return false;

        return players.Contains(_playerController);
    }
    public ObjectSO GetRepairItem() { return repairItem; }
    public ObjectState GetObjectState() { return state; }
    public virtual void OnBreakObject() { }
    protected virtual void RepairEnded(ObjectHolder _objectHolder)
    {
        _objectHolder.hintController.UpdateActionType(HintController.ActionType.NONE);
    }
    #endregion

}
