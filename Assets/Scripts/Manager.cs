using UnityEngine;

public class Manager : Singleton<Manager>
{
    public GameState currentState;

    private void Start()
    {
        SetCurrentGameState(GameState.Menu); // Startup state - Menu
    }

    /// <summary>
    /// Changing Current Game State
    /// </summary>
    /// <param name="state">Game state</param>
    public void SetCurrentGameState(GameState state)
    {
        switch(state)
        {
            case GameState.Menu: 
            {
                currentState = GameState.Menu;
                UIManager.Instance.ShowMenu();
                break;
            }
            case GameState.Play:
            {
                currentState = GameState.Play;
                break;
            }
            case GameState.NextWave:
            {
                currentState = GameState.NextWave;
                UIManager.Instance.UpdateMenu();
                break;
            }
            case GameState.GameOver:
            {
                currentState = GameState.GameOver;
                UIManager.Instance.UpdateMenu();
                break;
            }
            case GameState.Win: 
            {
                currentState = GameState.Win;
                UIManager.Instance.UpdateMenu();
                break;
            }
            // если не срабатывает не один case
            // СТАРТОВЫЕ ЗНАЧЕНИЯ ЕСЛИ НИ ОДИН ИЗ "СASE" НЕ СРАБОТАЛ
        }
    }
    
    /// <summary>
    /// Quiting Game
    /// </summary>
    public void QuitGame()
    {
        // Clean up application as necessary
        // Maybe save the players game

        Debug.Log("[Manager] Quit Game.");

        Application.Quit();
    }
}