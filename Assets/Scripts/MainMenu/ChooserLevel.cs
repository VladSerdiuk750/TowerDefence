using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class ChooserLevel : MonoBehaviour
    {
        private int _level;

        private Image _image;

        private Text _text;

        private GameObject _mapLevel;

        private GameObject _hud;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<Text>();
        }

        public void GetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void GetLevel(int level)
        {
            _level = level;
            _text.text = _level.ToString();
            var levelInfo = LevelManager.Instance.GetLevelInfo(level);
            //_mapLevel = levelInfo[0];
           //_hud = levelInfo[1];
        }

        private void OnLevelChosen()
        {
            LevelManager.Instance.LoadLevel(_mapLevel, _hud);
        }
    }
}