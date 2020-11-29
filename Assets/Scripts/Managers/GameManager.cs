using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GameManager : Singleton<GameManager>
{
    [Header("Spawning parameters")]
    [SerializeField] private Enemy[] typesOfEnemies;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDelay = 0.75f;
    
    [Header("Level parameters")]
    [SerializeField] private int totalWaves;
    [SerializeField] private int totalEnemies;
    
    private int _currentWaveNumber;
    public int CurrentWaveNumber => _currentWaveNumber;
    private int _currentTypesOfEnemiesToSpawn;
    
    private int _totalEscaped;
    private int _roundEscaped;
    private int _roundKilled;
    public int TotalEscaped => _totalEscaped;
    
    private List<Enemy> _enemyList;
    public List<Enemy> EnemyList => _enemyList;

    private void Start()
    {
        _enemyList = new List<Enemy>();
    }

    /// <summary>
    /// Start every level by spawning enemies
    /// </summary>
    public void Play()
    {
        StartCoroutine(Spawn());
    }

    /// <summary>
    /// Spawning enemies
    /// </summary>
    /// <returns></returns>
    private IEnumerator Spawn()
    {
        for (var i = 0; i < totalEnemies; i++)
        {
            var sprite = typesOfEnemies[Random.Range(0, _currentTypesOfEnemiesToSpawn)];
            Enemy newEnemy = Instantiate(sprite, spawnPoint.position, Quaternion.identity);
            RegisterEnemy(newEnemy);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    
    /// <summary>
    /// Invokes when enemy dies
    /// </summary>
    /// <param name="enemy">Which enemy dies</param>
    public void OnEnemyDie(Enemy enemy)
    {
        UnRegisterEnemy(enemy);
        Destroy(enemy.gameObject);
        _roundKilled += 1;
        MoneyManager.Instance.AddMoney(enemy.RewardAmount);
        if (IsWaveOver())
        {
            OnWaveOver();
        }
    }

    /// <summary>
    /// Invokes when enemy escapes
    /// </summary>
    /// <param name="enemy">Which enemy escapes</param>
    public void OnEnemyEscape(Enemy enemy)
    {
        _roundEscaped += 1;
        _totalEscaped += 1;
        UnRegisterEnemy(enemy);
        Destroy(enemy.gameObject);
        if (IsGameOver())
        {
            OnGameOver();
        }
        if (IsWaveOver())
        {
            if(IsWin())
                OnWin();
            else
                OnWaveOver();
        }
    }

    /// <summary>
    /// Handling states when wave is over
    /// </summary>
    private void OnWaveOver()
    {
        if (IsFinalWave())
        {
            UIManager.Instance.UpdateMenu("Final wave!");
        }
        else
        {
            UIManager.Instance.UpdateMenu("Next Wave");
        }
        _currentWaveNumber += 1;
        if (_currentWaveNumber <= typesOfEnemies.Length)
        {
            _currentTypesOfEnemiesToSpawn = _currentWaveNumber;
        }
        totalEnemies += _currentWaveNumber;
        _roundEscaped = 0;
        _roundKilled = 0;
        Manager.Instance.SetCurrentGameState(GameState.Pause);
    }

    /// <summary>
    /// Handling game over
    /// </summary>
    private void OnGameOver()
    {
        UIManager.Instance.UpdateMenu("You loose!");
        ClearLevel();
        Manager.Instance.SetCurrentGameState(GameState.GameOver);
    }

    private void OnWin()
    {
        UIManager.Instance.UpdateMenu("Win!!!");
        ClearLevel();
        Manager.Instance.SetCurrentGameState(GameState.Win);
    }
    
    private bool IsWaveOver() => (_roundEscaped + _roundKilled) == totalEnemies;

    private bool IsFinalWave() => (_currentWaveNumber == totalWaves - 2);

    private bool IsGameOver() => _totalEscaped >= 10;

    private bool IsWin() => _currentWaveNumber >= totalWaves - 1;

    private void RegisterEnemy(Enemy enemy) => _enemyList.Add(enemy);

    private void UnRegisterEnemy(Enemy enemy) => _enemyList.Remove(enemy);

    /// <summary>
    /// Clearing level
    /// </summary>
    private void ClearLevel()
    {
        TowerManager.Instance.DestroyAllTowers();
        DestroyEnemies();
    }

    /// <summary>
    /// Removing enemies from scene
    /// </summary>
    public void DestroyEnemies()
    {
        foreach (var enemy in _enemyList)
        {
            if(enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        _enemyList.Clear();
    }
}