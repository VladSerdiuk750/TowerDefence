using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    public Image currentSpeedImg;
    public Sprite speedX1Img;
    public Sprite speedX2Img;
    public Sprite speedX3Img;

    float _currectSpeed;
    public float speedX1 = 1f;
    public float speedX2 = 2f;
    public float speedX3 = 3f;

    void Start()
    {
        Time.timeScale = speedX1;
        currentSpeedImg.sprite = speedX1Img;
        _currectSpeed = speedX1;
    }
    public void SpeedChanger()
    {
        if (_currectSpeed == speedX1)
        {
            Time.timeScale = speedX2;
            currentSpeedImg.sprite = speedX2Img;
            _currectSpeed = speedX2;
        }
        else if (_currectSpeed == speedX2)
        {
            Time.timeScale = speedX3;
            currentSpeedImg.sprite = speedX3Img;
            _currectSpeed = speedX3;
        }
        else if (_currectSpeed == speedX3)
        {
            Time.timeScale = speedX1;
            currentSpeedImg.sprite = speedX1Img;
            _currectSpeed = speedX1;
        }
    }
}
