using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotationCanvasManager : MonoBehaviour
{
    public static VotationCanvasManager Instance { get; private set; }

    [Header("Votation")]
    [SerializeField] private List<VotationUI> ui;
    [SerializeField] private VotationTimer timer;
    [SerializeField] private TextMeshProUGUI sailTimer;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private Image healthImage;
    [SerializeField] private TextMeshProUGUI weightText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        SetHealthBar();
    }

    public void SetVotationUIState(bool state)
    {
        foreach (var _ui in ui) 
        { 
            _ui.gameObject.SetActive(state);
        }
        timer.gameObject.SetActive(state);
    }

    public void SetInformationDestination(LevelNode node)
    {
        int i = 0;
        foreach (var _ui in ui)
        {
            _ui.SetDestinationText(node._nodeChildren[i]._node.nodeType.ToString());
            i++;
        }
    }

    public void SetInformationPlayers(List<Votation> _votation)
    {
        int i = 0;
        foreach (var _ui in ui)
        {
            _ui.SetPlayers(_votation[i].GetCurrentsPlayer());
            i++;
        }
    }

    public void SetTimerUiInformation(float time, float maxTime)
    {
        timer.SetTimerUi(time, maxTime);
    }

    public void SetSailTimer(bool state)
    {
        sailTimer.gameObject.SetActive(state);
    }

    public void SetSailtText(float time)
    {
        sailTimer.text = time.ToString("00");
    }

    public void SetMoneyText(bool state)
    {
        moneyText.transform.parent.gameObject.SetActive(state);
        moneyText.text = "+" + MapManager.Instance.GetCurrentLevel()._node.levelMoney.ToString();

        if(state)
            StartCoroutine(DesactiveMoneyText());
    }

    private IEnumerator DesactiveMoneyText()
    {
        yield return new WaitForSeconds(1.5f);

        SetMoneyText(false);
    }

    public void SetHealthBar()
    {
        if(ShipsManager.instance.playerShip != null)
            healthImage.fillAmount = ShipsManager.instance.playerShip.GetCurrentHealth() / ShipsManager.instance.playerShip.GetMaxHealth();
    }

    public void SetWeightText(float value, float maxValeu)
    {
        weightText.text = value.ToString() + " / " + maxValeu.ToString();
    }
}
