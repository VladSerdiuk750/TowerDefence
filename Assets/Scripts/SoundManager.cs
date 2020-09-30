using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : Singleton<SoundManager>
{
    // ПЕРЕМЕННЫЕ ДЛЯ ЗВУКОВ
    [SerializeField]
    AudioClip arrow;
    [SerializeField]
    AudioClip death;
    [SerializeField]
    AudioClip fireBall;
    [SerializeField]
    AudioClip gameOver;
    [SerializeField]
    AudioClip hit;
    [SerializeField]
    AudioClip level;
    [SerializeField]
    AudioClip newGame;
    [SerializeField]
    AudioClip rock;
    [SerializeField]
    AudioClip towerBuilt;

    // СВОЙСТВА ДЛЯ ЗВУКОВ
    public AudioClip Arrow
    {
        get
        {
            return arrow;
        }
    }
    public AudioClip Death
    {
        get
        {
            return death;
        }
    }
    public AudioClip FireBall
    {
        get
        {
            return fireBall;
        }
    }
    public AudioClip GameOver
    {
        get
        {
            return gameOver;
        }
    }
    public AudioClip Hit
    {
        get
        {
            return hit;
        }
    }
    public AudioClip Level
    {
        get
        {
            return level;
        }
    }
    public AudioClip NewGame
    {
        get
        {
            return newGame;
        }
    }
    public AudioClip Rock
    {
        get
        {
            return rock;
        }
    }
    public AudioClip TowerBuilt
    {
        get
        {
            return towerBuilt;
        }
    }
}
