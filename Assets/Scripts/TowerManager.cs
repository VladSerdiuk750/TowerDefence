using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    [SerializeField] private List<Tower> typesOfTowersAvailable;

    // лист для башен
    private List<Tower> _towerList = new List<Tower>();

    private TowerSide _currentTowerSide;

    private List<TowerSide> _fullTowerSides;

    public List<Tower> TypesOfTowersAvailable
    {
        get => typesOfTowersAvailable;
        set => typesOfTowersAvailable = value;
    }

    private void Start()
    {
        _fullTowerSides = new List<TowerSide>();
    }

    public void PlaceTower(Tower tower)
    {
        if (_currentTowerSide != null)
        {
            Tower newTower = Instantiate(tower); // спавним башню привязанную к определенной кнопке 
            newTower.transform.position =
                _currentTowerSide.transform.position; // башня помещается по тем координатам где мы нажали
            RegisterTower(newTower); // регистрируем новую башню
            _currentTowerSide.IsFull = true;
            _currentTowerSide.GetCurrentTower(newTower);
            _currentTowerSide = null;
        }
    }

    public void UpgradeTower()
    {
        if(_currentTowerSide is null)
            return;
        _currentTowerSide.CurrentTower.TowerLevelUp();
    }

    public void SellTower()
    {
        UnRegisterTower(_currentTowerSide.CurrentTower);
        Destroy(_currentTowerSide.CurrentTower.gameObject);
        EmptyingTowerSide(_currentTowerSide);
    }

    public void GetTowerSide(TowerSide towerSide)
    {
        _currentTowerSide = towerSide;
    }

    public void RegisterTower(Tower tower)
    {
        _towerList.Add(tower); // добавляем к уже зарегестрированным башням - новую
    }
    
    public void UnRegisterTower(Tower tower)
    {
        _towerList.Remove(tower); // добавляем к уже зарегестрированным башням - новую
    }

    public void RegisterBuildSite(TowerSide towerSide) // считывает информацию о том где стоит какая башня
    {
       _fullTowerSides.Add(towerSide);
    }

    public void EmptyingTowerSide(TowerSide towerSide)
    {
        _fullTowerSides.Remove(towerSide);
        towerSide.IsFull = false;
    }

    public void DestroyAllTowers()
    {
        foreach (Tower tower in _towerList) // перебираем все башни
        {
            foreach (var towerSide in _fullTowerSides)
            {
                EmptyingTowerSide(towerSide);
            }
            Destroy(tower.gameObject); // удаляем все башни
        }

        _towerList.Clear(); // очищаем список
    }

    public void BuyTower(int price)
    {
        MoneyManager.Instance.SubtractMoney(price); // вычитаем стоимость башни
    }

    public void OnTowerButtonClick(Tower tower)
    {
        BuyTower(tower.Price);
        PlaceTower(tower);
    }
}