using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int totalWaves; // максимальное число волн
    [SerializeField] private int totalEnemies = 5; // сколько всего должно появится противников на уровне
    [SerializeField] private int waveNumber = 1; // волна с которой начинаем
    [SerializeField] private Enemy[] typesOfEnemies; // массив обьектов врагов
    [SerializeField] private int enemiesPerSpawn; // сколько одновременно может спавниться на уровне
    [SerializeField] private Transform spawnPoint;

    private int _totalEscaped; // сколько прошло противников
    private int _roundEscaped; // сколько прошло противников за 1 игру
    private int _totalKilled; // всего уничтожено врагов (для механики денег, типо как вознаграждать игрока)    int currentTypesOfEnemiesToSpawn = 0; // рандом для врагов
    private int _roundKilled;
    private int _currentTypesOfEnemiesToSpawn;

    [SerializeField] private float spawnDelay = 0.75f;
    
    public int TotalEscaped
    {
        get 
        {
            return _totalEscaped;
        }
        set
        {
            _totalEscaped = value;
        }
    }
    public int RoundEscaped 
    {
        get
        {
            return _roundEscaped; 
        }
        set
        {
            _roundEscaped = value;
        }
    }
    public int TotalKilled 
    {
        get
        {
            return _totalKilled;
        }
        set
        {
            _totalKilled = value;
        }
    }
    
    public int TotalEnemies
    {
        get => totalEnemies;
        set => totalEnemies = value;
    }

    public int WaveNumber 
    {
        get => waveNumber;
        set => waveNumber = value;
    }

    public int RoundKilled
    {
        get => _roundKilled;
        set => _roundKilled = value;
    }
    
    public List<Enemy> enemyList = new List<Enemy>(); // лист содержащих всей противников на уровне (так же будет считывать по какому стрелять)

    public UnityEvent<int, Enemy> onEnemyDie;

    private void Start()
    {
        onEnemyDie = new UnityEvent<int, Enemy>();
        onEnemyDie.AddListener(OnEnemyDieHandler);
    }

    public void StartNewGame()
    {
        StartCoroutine(Spawn());
    }

    public void SetDefaultValues()
    {
        TotalKilled = 0;
        RoundEscaped = 0;
        TotalEscaped = 0;
        RoundKilled = 0;
        MoneyManager.Instance.TotalMoney = 45;
        TotalEnemies = 5;
        WaveNumber = 0;
    }

    private IEnumerator Spawn()
    {
        for (int i = 0; i < totalEnemies; i++) // создаем по одному обьекту
        {
            Enemy newEnemy = Instantiate(typesOfEnemies[Random.Range(0, _currentTypesOfEnemiesToSpawn)]) ; // создаем одного из врагов
            newEnemy.transform.position = spawnPoint.position; // на позиции спавна
            RegisterEnemy(newEnemy);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void NextLevel()
    {
        waveNumber += 1; // меняем волну
        totalEnemies += waveNumber; // с каждой волной больше противников
        RoundEscaped = 0;
        RoundKilled = 0;
        //TowerManager.Instance.ClearProjectiles();
        StartCoroutine(Spawn());
    }

    public void IsWaveOver() // закончилась ли волна
    {
        // если сумма сбежавших противников + всех что мы убили = всем противникам в волне
        if ((RoundEscaped + RoundKilled) == TotalEnemies)
        {
            if(IsWin())
            {
                ClearLevel();
                Manager.Instance.SetCurrentGameState(GameState.Win); // то статус - победа            
            }
            else
            {
                if (waveNumber <= typesOfEnemies.Length)
                {
                    _currentTypesOfEnemiesToSpawn = waveNumber;
                }
                if(IsFinalWave())
                {
                    Manager.Instance.SetCurrentGameState(GameState.NextWave); // выводим текущее состояние игры
                }
                else Manager.Instance.SetCurrentGameState(GameState.NextWave); // выводим текущее состояние игры
            }
        }
    }

    public bool IsFinalWave()
    {
        if (waveNumber == totalWaves - 2)
        {
            return true;
        }
        return false;
    }

    public void IsGameOver()
    {
        if (TotalEscaped >= 10) // если прошедших больше чем наших жизней
        {
            ClearLevel();
            Manager.Instance.SetCurrentGameState(GameState.GameOver);
        }
    }

    public bool IsWin()
    {
        if (waveNumber >= totalWaves - 1) // если число волн равняется максимальным числом волн
        {
            return true;
        }
        return false;
    }

    public void RegisterEnemy(Enemy enemy) // по какому (ближайшему) стрелять
    {
        enemyList.Add(enemy); // дабавляет в лист врага
    }

    public void UnRegisterEnemy(Enemy enemy) // убираем со списка
    {
        enemyList.Remove(enemy); // убирает из листа врага
        Destroy(enemy.gameObject); // уничтожает врага в листе 
    }

    private void ClearLevel()
    {
        SetDefaultValues();
        TowerManager.Instance.DestroyAllTowers(); // уничтожаем все башни
        DestroyEnemies(); // уничтожаем всех умерших врагов на волне
    }

    public void DestroyEnemies()
    {
        foreach (Enemy enemy in enemyList) // перебирает врагов на уровне в листе
        {
            if(enemy != null)
            {
                Destroy(enemy.gameObject); // уничтожает врага
            }
        }
        enemyList.Clear(); // Очищает лист и дает возможность для загрузки листа врагов для нового уровня
    }

    private void OnEnemyDieHandler(int rewardAmount, Enemy enemy)
    {
        GameManager.Instance.UnRegisterEnemy(enemy);
        GameManager.Instance.RoundKilled += 1;
        GameManager.Instance.TotalKilled += 1;
        MoneyManager.Instance.AddMoney(rewardAmount);
        GameManager.Instance.IsWaveOver();
    }
    
}