using UnityEngine;
using UnityEngine.UI;

public class RepairItemHint : ItemHint
{

    [Space,Header("Repair Hint"), SerializeField]
    protected Image itemImage;
    [SerializeField]
    private Vector3 itemImageOffset;

    [field: Space, SerializeField] 
    public ProgressBarController progressBar {  get; private set; }
    [SerializeField] 
    protected Vector3 progressBarOffset;

    protected override void Update()
    {
        base.Update();

        if (itemImage.gameObject.activeInHierarchy)
            itemImage.transform.position = transform.position + itemImageOffset;

        if (progressBar.gameObject.activeInHierarchy)
        {
            progressBar.transform.position = transform.position + progressBarOffset;
            if (!CanSomePlayerRepair())
                progressBar.gameObject.SetActive(false);
        }
    }

    public override void DisableAllHints()
    {
        base.DisableAllHints();
        if (itemImage)
            itemImage.gameObject.SetActive(false);
    }

    public void ShowRepairSprite()
    {
        isEnabled = true;
        itemImage.sprite = PlayersManager.instance.repairSprite;
        itemImage.gameObject.SetActive(true);

    }

    private bool CanSomePlayerRepair()
    {
        foreach (int player in playersLooking)
        {
            if ((currentObject as Repair).CanRepair(PlayersManager.instance.ingamePlayers[player].objectHolder))
                return true;
        }

        return false;
    }
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + progressBarOffset, 0.5f);
    }


}
