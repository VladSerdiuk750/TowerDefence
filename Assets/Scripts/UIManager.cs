using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private HUDController hud;

    private Tower _currentSelectedTower;

    public void ShowMenu()
    {
        hud.ShowMenu();
    }

    public void UpdateMenu()
    {
        hud.PlayButtonSetActive(true);
        switch (Manager.Instance.currentState)
        {
            case GameState.Menu:
            {
                hud.ShowMenu();
                break;
            }
            case GameState.NextWave:
            {
                hud.ChangePlayBtnLabel("Next wave");
                break;
            }
            case GameState.Win:
            {
                hud.ChangePlayBtnLabel("Win!");
                break;
            }
            case GameState.GameOver:
            {
                hud.ChangePlayBtnLabel("Play again!");
                break;
            }
            case GameState.Play:
                break;
        }
    }
    
    public void OnPlayBtnClick()
    {
        hud.PlayButtonSetActive(false);

        switch (Manager.Instance.currentState)
        {
            case GameState.Menu:
            {
                GameManager.Instance.StartNewGame();
                Manager.Instance.SetCurrentGameState(GameState.Play);
                break;
            }
            case GameState.NextWave:
            {
                GameManager.Instance.NextLevel();
                Manager.Instance.SetCurrentGameState(GameState.Play);
                break;
            }
            case GameState.Win:
            {
                GameManager.Instance.StartNewGame();
                Manager.Instance.SetCurrentGameState(GameState.Play);
                break;
            }
            case GameState.GameOver:
            {
                GameManager.Instance.StartNewGame();
                Manager.Instance.SetCurrentGameState(GameState.Play);
                break;
            }
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