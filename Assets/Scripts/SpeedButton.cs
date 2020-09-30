using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    public Image currentSpeed_img;
    public Sprite speed_x1_img;
    public Sprite speed_x2_img;
    public Sprite speed_x3_img;

    float currectSpeed;
    public float speed_x1 = 1f;
    public float speed_x2 = 2f;
    public float speed_x3 = 3f;

    void Start()
    {
        Time.timeScale = speed_x1;
        currentSpeed_img.sprite = speed_x1_img;
        currectSpeed = speed_x1;
    }
    public void SpeedChanger()
    {
        if (currectSpeed == speed_x1)
        {
            Time.timeScale = speed_x2;
            currentSpeed_img.sprite = speed_x2_img;
            currectSpeed = speed_x2;
        }
        else if (currectSpeed == speed_x2)
        {
            Time.timeScale = speed_x3;
            currentSpeed_img.sprite = speed_x3_img;
            currectSpeed = speed_x3;
        }
        else if (currectSpeed == speed_x3)
        {
            Time.timeScale = speed_x1;
            currentSpeed_img.sprite = speed_x1_img;
            currectSpeed = speed_x1;
        }
    }
}
