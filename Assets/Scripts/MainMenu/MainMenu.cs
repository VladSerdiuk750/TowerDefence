using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject settingsMenu;

        [SerializeField] private GameObject quickPlayMenu;

        [SerializeField] private GameObject campaignMenu;

        [SerializeField] private GameObject mainMenuBtns;

        public void OnRateUsBtnClick()
        {
            // TODO: Open Play Market Ratting Chooser
            Debug.Log("Rate Us Button Clicked");
        }

        public void OnSettingsBtnClick()
        {
            mainMenuBtns.SetActive(false);
            settingsMenu.SetActive(true);
        }
    
        public void OnCampaignBtnClicked()
        {
            mainMenuBtns.SetActive(false);
            campaignMenu.SetActive(true);
        }

        public void OnQuickPlayBtnClicked()
        {
            Manager.Instance.SetCurrentGameState(GameState.Pause);
            // TODO: Make Loading Transition
            SceneManager.LoadScene("QuickPlay");
        }

        public void OnCloseBtnClicked(GameObject menu)
        {
            menu.SetActive(false);
            mainMenuBtns.SetActive(true);
        }
    }
}
