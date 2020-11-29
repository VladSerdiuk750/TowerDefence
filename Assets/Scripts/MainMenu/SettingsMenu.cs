using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Slider soundSlider;

        [SerializeField] private Slider musicSlider;

        private void Awake()
        {
            soundSlider.value = SoundManager.Instance.SoundVolume;
            musicSlider.value = SoundManager.Instance.MusicVolume;
        }

        public void OnMuteBtnClick(Slider slider)
        {
            slider.value = 0f;
        }
    }
}