using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Text totalMoneyLabel;
    [SerializeField] private Text currentWave;
    [SerializeField] private Text totalEscapeLabel;
    [SerializeField] private Button playBtn;
    [SerializeField] private GameObject buyTowerMenu;

    [SerializeField] private GameObject towerMenu;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Button sellBtn;
    // [SerializeField] private Sprite[] upgradeSprites;
    // [SerializeField] private float padding;
    
    private Text _playBtnLabel;

    private void Awake()
    {
        _playBtnLabel = playBtn.GetComponentInChildren<Text>(); // getting label of play button
    }

    private void Update() 
    {
        if (Manager.Instance.CurrentState != GameState.Menu)
        {
            UpdateLabels();
        }
    }
    public void ShowMenu()
    {
        _playBtnLabel.text = "Play Game";
    }

    public void ChangePlayBtnLabel(string text)
    {
        _playBtnLabel.text = text;
    }

    void UpdateLabels()
    {
        totalMoneyLabel.text = MoneyManager.Instance.TotalMoney.ToString();
        totalEscapeLabel.text = "Escaped " + GameManager.Instance.TotalEscaped + " of 10";
        currentWave.text = "Wave " + GameManager.Instance.CurrentWaveNumber;
    }

    public void PlayButtonSetActive(bool isActive)
    {
        playBtn.gameObject.SetActive(isActive);
    }

    public void HideLabels()
    {
        totalEscapeLabel.gameObject.SetActive(false);
        totalMoneyLabel.gameObject.SetActive(false);
        currentWave.gameObject.SetActive(false);
    }
    
    public void ShowLabels()
    {
        totalEscapeLabel.gameObject.SetActive(true);
        totalMoneyLabel.gameObject.SetActive(true);
        currentWave.gameObject.SetActive(true);
    }

    public void OpenBuyTowerMenu()
    {
        HideLabels();
        Time.timeScale = 0.0f;
        buyTowerMenu.SetActive(true);
    }

    public void CloseBuyTowerMenu()
    {
        ShowLabels();
        buyTowerMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void OpenTowerMenu(int upgradeCost, int sellingCost)
    {
        HideLabels();
        Time.timeScale = 0.0f;
        Text upgradeBtnLabel = upgradeBtn.GetComponentInChildren<Text>();
        upgradeBtnLabel.text = "Upgrade: " + upgradeCost.ToString();
        Text sellBtnLabel = sellBtn.GetComponentInChildren<Text>();
        sellBtnLabel.text = "Sell: " + sellingCost.ToString();
        towerMenu.SetActive(true);
    }

    public void CloseTowerMenu()
    {
        ShowLabels();
        Time.timeScale = 1.0f;
        towerMenu.SetActive(false);
    }

    // public void ShowTowerMenu(int tier, int costOfUpgrade, int costOfSelling, Vector3 position)
    // {
    //     CreateUpgradeBtn(tier, costOfUpgrade, position);
    //     CreateSellBtn(costOfSelling, position);
    // }
    //
    // private void CreateUpgradeBtn(int tier, int costOfUpgrade, Vector3 position)
    // {
    //     var destiny = (position + transform.position).normalized;
    //     var destinyPosition = new Vector3(destiny.x, destiny.y + padding, destiny.z).normalized;
    //     Button newUpgradeBtn = Instantiate(upgradeBtn, transform, false);
    //     
    //     Text newUpgradeBtnLabel = newUpgradeBtn.GetComponentInChildren<Text>();
    //     newUpgradeBtnLabel.text = costOfUpgrade.ToString();
    //     newUpgradeBtn.image.sprite = upgradeSprites[tier];
    //     RectTransform rectT = newUpgradeBtn.GetComponentInChildren<RectTransform>();
    //     rectT.position = destinyPosition;
    // }
    //
    // private void CreateSellBtn(int costOfSelling, Vector3 position)
    // {
    //     var destinyPosition = new Vector3(position.x, position.y + padding, position.z).normalized;
    //     Button newSellBtn = Instantiate(sellBtn, destinyPosition, Quaternion.identity, transform);
    //     
    //     Text newUpgradeBtnLabel = newSellBtn.GetComponentInChildren<Text>();
    //     newUpgradeBtnLabel.text = costOfSelling.ToString();
    // }
    
}