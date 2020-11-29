using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private List<GameObject[]> levelMaps;

    public GameObject[] GetLevelInfo(int level)
    {
        return Resources.LoadAll("/Levels/" + level.ToString()) as GameObject[];
    }

    public void LoadLevel(GameObject map, GameObject hud)
    {
        Manager.Instance.SetCurrentGameState(GameState.Pause);
        SceneManager.LoadScene("AnyLevel");
        Instantiate(map);
        Instantiate(hud);
    }
}