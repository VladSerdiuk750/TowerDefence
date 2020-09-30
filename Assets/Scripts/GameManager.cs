using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] int totalWaves; // максимальное число волн
    [SerializeField] int totalEnemies = 5; // сколько всего должно появится противников на уровне
    [SerializeField] int waveNumber = 1; // волна с которой начинаем
    [SerializeField] Enemy[] typesOfEnemies; // массив обьектов врагов
    [SerializeField] int enemiesPerSpawn; // сколько одновременно может спавниться на уровне
    [SerializeField] Transform spawnPoint;
    int totalMoney = 45; // текущее колличесво денег
    int _totalEscaped = 0; // сколько прошло противников
    int roundEscaped = 0; // сколько прошло противников за 1 игру
    int totalKilled = 0; // всего уничтожено врагов (для механики денег, типо как вознаграждать игрока)    int currentTypesOfEnemiesToSpawn = 0; // рандом для врагов
    int roundKilled = 0;

    int currentTypesOfEnemiesToSpawn = 0;
    [SerializeField] float spawnDelay = 0.75f;

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
            return roundEscaped; 
        }
        set
        {
            roundEscaped = value;
        }
    }
    public int TotalKilled 
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }
    public int TotalMoney 
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
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
        get => roundKilled;
        set => roundKilled = value;
    }
    
    public List<Enemy> EnemyList = new List<Enemy>(); // лист содержащих всей противников на уровне (так же будет считывать по какому стрелять)

    IEnumerator spawningCoroutine;

    bool spawning;

    private void Start() 
    {
        spawningCoroutine = Spawn();
    }

    void Update()
    {

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
        TotalMoney = 45;
        TotalEnemies = 5;
        WaveNumber = 0;
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < totalEnemies; i++) // создаем по одному обьекту
        {
            Enemy newEnemy = Instantiate(typesOfEnemies[Random.Range(0, currentTypesOfEnemiesToSpawn)]) as Enemy; // создаем одного из врагов
            newEnemy.transform.position = spawnPoint.position; // на позиции спавна
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void NextLevel()
    {
        waveNumber += 1; // меняем волну
        totalEnemies += waveNumber; // с каждой волной больше противников
        RoundEscaped = 0;
        RoundKilled = 0;
        TowerManager.Instance.DeleteAllProjectiles();
        StartCoroutine(Spawn());
    }

    public void RegisterEnemy(Enemy enemy) // по какому (ближайшему) стрелять
    {
        EnemyList.Add(enemy); // дабавляет в лист врага
    }

    public void UnRegisterEnemy(Enemy enemy) // убираем со списка
    {
        EnemyList.Remove(enemy); // убирает из листа врага
        Destroy(enemy.gameObject); // уничтожает врага в листе 
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount; // добавляем деньги
    }

    public void SubtractMoney(int amount)
    {
        TotalMoney -= amount; // вычитаем деньги
    }

    public void IsWaveOver() // закончилась ли волна
    {
        // если сумма сбежавших противников + всех что мы убили = всем противникам в волне
        if ((RoundEscaped + RoundKilled) == TotalEnemies)
        {
            if (waveNumber <= typesOfEnemies.Length)
            {
                currentTypesOfEnemiesToSpawn = waveNumber;
            }
            if(IsWin())
            {
                Manager.Instance.SetCurrentGameState(Manager.GameState.win); // то статус - победа            
            }
            else Manager.Instance.SetCurrentGameState(Manager.GameState.nextWave); // выводим текущее состояние игры
        }
    }

    public void IsGameOver()
    {
        if (TotalEscaped >= 10) // если прошедших больше чем наших жизней
        {
            Manager.Instance.SetCurrentGameState(Manager.GameState.gameover);
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

    public void DestroyEnemies()
    {
        foreach (Enemy enemy in EnemyList) // перебирает врагов на уровне в листе
        {
            if(enemy != null)
            {
                Destroy(enemy.gameObject); // уничтожает врага
            }
        }
        EnemyList.Clear(); // Очищает лист и дает возможность для загрузки листа врагов для нового уровня
    }
}

// public void Spawn()
    // {
    //     if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) // если могут еще создаваться враги
    //     {
    //         Debug.Log("s");
    //         for (int i = 0; i < enemiesPerSpawn; i++) // создаем по одному обьекту
    //         {
    //             if (EnemyList.Count < totalEnemies)
    //             {
    //                 Enemy newEnemy = Instantiate(typesOfEnemies[Random.Range(0, currentTypesOfEnemiesToSpawn)]) as Enemy; // создаем одного из врагов
    //                 newEnemy.transform.position = spawnPoint.position; // на позиции спавна
    //             }
    //         }
    //         Wait();
    //     }
    // }

    // IEnumerator Wait()
    // {
    //     yield return new WaitForSeconds(spawnDelay);// делаем спавн через "spawnDelay" колл-во секунд
    // }
