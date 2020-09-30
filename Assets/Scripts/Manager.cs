using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : Singleton<Manager>
{
    public enum GameState // в каком положении находится игра
    { 
        menu,
        play,
        nextWave,
        gameover,
        win
    }
    AudioSource audioSource; // для звуков
    public AudioSource AudioSource 
    {
        get
        {
            return audioSource;
        }
    }
    [SerializeField] HUDController HUD;
    public GameState currentState = GameState.menu; // изначально статус игры "menu"

    void Start()
    {
        //audioSource = GetComponent<AudioSource>(); // реализуем
    }
    void Update()
    {

    }
    
    public void SetCurrentGameState(GameState state) // текущее состояние игры
    {
        switch(state)
        {
            case GameState.menu: 
            {
                currentState = GameState.menu;
                HUD.ShowMenu();
                break;
            }
            case GameState.play: 
            {
                currentState = GameState.play;
                Time.timeScale = 1.0f;
                HUD.PlayButtonSetActive(false);
                break;
            }
            case GameState.nextWave:
            {
                currentState = GameState.nextWave;
                HUD.UpdateMenu(currentState);
                break;
            }
            case GameState.gameover: 
            {
                currentState = GameState.gameover;
                Time.timeScale = 0.0f;
                HUD.UpdateMenu(currentState);
                ClearLevel();
                break;
            }
            case GameState.win: 
            {
                currentState = GameState.win;
                HUD.UpdateMenu(currentState);
                ClearLevel();
                break;
            }
            // если не срабатывает не один case
            // СТАРТОВЫЕ ЗНАЧЕНИЯ ЕСЛИ НИ ОДИН ИЗ "СASE" НЕ СРАБОТАЛ
            default: break;
        }
    }

    public void OnPlayBtnClick()
    {
        if(currentState != GameState.nextWave)
        {
            SetCurrentGameState(GameState.play);
            GameManager.Instance.StartNewGame();
        }
        else
        {
            SetCurrentGameState(GameState.play);
            GameManager.Instance.NextLevel();
        }
    }

    private void ClearLevel()
    {
        GameManager.Instance.SetDefaultValues();
        TowerManager.Instance.DestroyAllTowers(); // уничтожаем все башни
        TowerManager.Instance.RenameTagBuildSite(); // очищаем теги на пустые
        GameManager.Instance.DestroyEnemies(); // уничтожаем всех умерших врагов на волне
    }


    public void QuitGame()
    {
        // Clean up application as necessary
        // Maybe save the players game

        Debug.Log("[GameManager] Quit Game.");

        Application.Quit();
    }
    
}
    
    // private void HandleEscape() // отменить нажатие на башню
    // {
    //     if (Input.GetMouseButtonDown(1)) // при нажатии правой кнопки мыши
    //     {
    //         TowerManager.Instance.DisableDrag(); // отключаем возможность перетаскивать изображение
    //         TowerManager.Instance.towerBtnPressed = null; // восстановили не кликанье по кнопке
    //     }
    // }

    //HandleEscape(); // проверка нажали ли мы "Escape" постоянно

    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // загрузка следующей сцены
    //SceneManager.LoadScene("0");
    //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // перегружает активную сцену 


    // public void PlayButtonPressed()
    // {
    //     switch (currentState)
    //     {
    //         // если "next"
    //         case gameStatus.next:
                
    //             break;

                
    //         default:
                
                
                
                
    //             //HUDController.Instance.UpdateMenu();
    //             //audioSource.PlayOneShot(SoundManager.Instance.NewGame); // единожды играть
    //             break;
    //     }
        
        
    //     //HUDController.Instance.WaveNumberUpdate(waveNumber);
    //     StartCoroutine(Spawn()); // вызываем спавн новых противников
    //     //HUDController.Instance.playBtn.gameObject.SetActive(false); // отключаем кнопку
    // }





