using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image background;

    [SerializeField] private GameObject optionsWindow;
    
    public void OnNewGameButtonClick()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OnContinueButtonClick()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    
    public void OnOptionsButtonClick()
    {
        
    }
}
