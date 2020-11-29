using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private HUDController hud;

    private Tower _currentSelectedTower;

    public void ShowMenu()
    {
        hud.ShowMenu();
    }

    public void UpdateMenu(string playBtnLabel)
    {
        hud.PlayButtonSetActive(true);
        hud.ChangePlayBtnLabel(playBtnLabel);
    }
    
    public void OnPlayBtnClick()
    {
        hud.PlayButtonSetActive(false);

        if (Manager.Instance.CurrentState == GameState.Pause || Manager.Instance.CurrentState == GameState.Menu)
        {
            GameManager.Instance.Play();
            Manager.Instance.SetCurrentGameState(GameState.Play);
        }
    }

    public void MenuTowerClick(Tower tower, TowerSide towerSide)
    {
        TowerManager.Instance.GetTowerSide(towerSide);
        hud.HideLabels();
        hud.OpenTowerMenu(tower.UpgradeCost, tower.SellingCost);
        Time.timeScale = 0.0f;
        _currentSelectedTower = tower;
        //hud.ShowTowerMenu(tower.TowerTier, tower.UpgradeCost, tower.SellingCost, towerSide.transform.position);
    }

    public void OnUpgradeTowerClick()
    {
        if (MoneyManager.Instance.IsOperationAffordable(_currentSelectedTower.UpgradeCost))
        {
            TowerManager.Instance.UpgradeTower();
            MoneyManager.Instance.SubtractMoney(_currentSelectedTower.UpgradeCost);
            hud.CloseTowerMenu();
        }
    }

    public void OnSellTowerClick()
    {
        MoneyManager.Instance.AddMoney(_currentSelectedTower.SellingCost);
        TowerManager.Instance.SellTower();
        hud.CloseTowerMenu();
    }
    
    public void BuyTowerClick()
    {
        hud.OpenBuyTowerMenu();
    }

    public void OnTowerButtonClick(Tower tower)
    {
        if (MoneyManager.Instance.IsOperationAffordable(tower.Price))
        {
            TowerManager.Instance.OnTowerButtonClick(tower);
            hud.CloseBuyTowerMenu();
        }
    }
}