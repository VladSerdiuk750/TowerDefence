using UnityEngine;

public class Manager : Singleton<Manager>
{
    private GameState _currentState;

    public GameState CurrentState => _currentState;

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
                _currentState = GameState.Menu;
                break;
            }
            case GameState.Play:
            {
                _currentState = GameState.Play;
                Time.timeScale = 1.0f;
                break;
            }
            case GameState.Pause:
            {
                _currentState = GameState.Pause;
                Time.timeScale = 0.0f;
                break;
            }
            
            case GameState.GameOver:
            {
                _currentState = GameState.GameOver;
                break;
            }
            case GameState.Win: 
            {
                _currentState = GameState.Win;
                break;
            }
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
        
        UserDataManager.Instance.SaveVolumeSettings(SoundManager.Instance.SoundVolume, SoundManager.Instance.MusicVolume);

        Application.Quit();
    }
}