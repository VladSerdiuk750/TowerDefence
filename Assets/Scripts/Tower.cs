using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
    [SerializeField] private float reloadingTime; // скорость стрельбы (время между выстрелами)

    private float _reloadingCountdown;

    [SerializeField] private float attackRadius; // радиус атаки

    [SerializeField] private ProjectTile towerProjectTile; // сам вид снаряда

    [SerializeField] private Sprite[] towerSprites;

    [SerializeField] private int towerTier;
    
    private int _sellingCost;

    private int _upgradeCost;

    private Enemy _targetEnemy; // по какому потивнику стреляем (изначально не по кому стрелять)

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private int price;
    public int TowerTier => towerTier;

    public int Price
    {
        get => price;
        private set
        {
            price = value;
            _sellingCost = price / 2;
            _upgradeCost = price * 2;
        }
    }

    public int SellingCost => _sellingCost;

    public int UpgradeCost => _upgradeCost;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = towerSprites[towerTier];
        _sellingCost = price / 2;
        _upgradeCost = price * 2;
    }

    private void Update()
    {
        _reloadingCountdown -= Time.deltaTime;

        if (_targetEnemy is null || _targetEnemy.IsDead)
        {
            _targetEnemy = GetTargetEnemy();
            return;
        }

        if (_reloadingCountdown <= 0)
        {
            Attack();
            _reloadingCountdown = reloadingTime;
        }
        
        // если расстояние до цели больше радиуса башни
        if (Vector2.Distance(transform.localPosition, _targetEnemy.transform.localPosition) > attackRadius)
        {
            _targetEnemy = null; // то мы теряем цель
        }
    }

    private void Attack()
    {
        if (towerProjectTile is null)
            return;

        var newProjectile = Instantiate(towerProjectTile);
        newProjectile.transform.localPosition = transform.localPosition;
        newProjectile.Target = _targetEnemy;
    }

    private Enemy GetTargetEnemy()
    {
        var nearestEnemy = GetNearestEnemy(); // считываем ближайшего противника из другого скрипта
        if (nearestEnemy is null || nearestEnemy.IsDead)
            return null;
        if (Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius)
        {
            return nearestEnemy; // то цель становится ближайшей целью для стрельбы
        }

        return null;
    }

    private Enemy GetNearestEnemy() // находит ближайшего врага
    {
        Enemy nearestEnemy = null; // ближайший враг (изначально никакой)
        float smallestDistance = float.PositiveInfinity; // определяет ближайшего врага (расстояние)
        List<Enemy> enemyList = GetEnemiesInRange();

        if (enemyList is null)
            return null;
        
        foreach (Enemy enemy in enemyList) // считывает всех противников в диапозоне стрельбы
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance) // если расстоние от башни до противников минимальное из всех
            {
                // просчет самого расстояния
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition); 
                nearestEnemy = enemy; // делаем его ближайшим врагом
            }
        }
        return nearestEnemy; // возвращает ближайшего врага
    }

    private List<Enemy> GetEnemiesInRange() // какие противники в зоне поражения
    {
        List<Enemy> enemiesInRange = new List<Enemy>(); // подтягивает из другого листа

        foreach (Enemy enemy in GameManager.Instance.enemyList) // ищет в листе противников
        {
            // если расстояние от башни до противника меньше или равно радиусу стрельбы
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy); // то добавляем врага в лист возможных для выстрела
            }
        }
        return enemiesInRange; // возвращает противников дошедших до зоны поражения
    }
    
    // private void RegisterProjectile(ProjectTile newProjectTile)
    // {
    //     _towerProjectiles.Add(newProjectTile);
    // }
    //
    // private void UnRegisterProjectile(ProjectTile newProjectTile)
    // {
    //     _towerProjectiles.Remove(newProjectTile);
    // }
    
    public void TowerLevelUp()
    {
        towerTier++;
        if(towerSprites.Length - 1 >= towerTier)
        {
            _spriteRenderer.sprite = towerSprites[towerTier];
        }

        Price += Price;
    }

    // public void ClearProjectiles()
    // {
    //     foreach(ProjectTile projectile in _towerProjectiles)
    //     {
    //         Destroy(projectile);
    //     }
    //     _towerProjectiles.Clear();
    // }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}