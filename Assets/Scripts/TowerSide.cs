using System;
using UnityEngine;

public class TowerSide : MonoBehaviour
{
    private bool _isFull = false;

    private string _towerSideTag;
    
    public bool IsFull
    {
        get => _isFull;
        set
        {
            _isFull = value;
            gameObject.tag = value ? "TowerSideFull" : "TowerSide";
        }
    }

    private Tower _currentTower;

    public Tower CurrentTower => _currentTower;

    private void OnMouseDown() 
    {
        if (Manager.Instance.CurrentState != GameState.Play)
            return;
        if (Time.timeScale == 0.0f)
            return;
        if (gameObject.CompareTag("TowerSide"))
        {
            UIManager.Instance.BuyTowerClick();
        }
        else if(gameObject.CompareTag("TowerSideFull"))
        {
            UIManager.Instance.MenuTowerClick(_currentTower, this);
        }
        TowerManager.Instance.GetTowerSide(this);
    }

    public void GetCurrentTower(Tower tower)
    {
        _currentTower = tower;
    }

    public void RemoveTower()
    {
        _isFull = false;
    }

    private void Update()
    {
        
    }
}