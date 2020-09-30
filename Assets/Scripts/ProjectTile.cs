using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum projectTileType // перечесление наших тайлов
{ 
    cotton, cellphone, arrow, currency
};
public class ProjectTile : MonoBehaviour
{
    [SerializeField]
    int attackDamage; // урон от снаряда

    [SerializeField]
    projectTileType pType; // тип тайла с которым работаем

    public int AttackDamage
    {
        get
        {
            return attackDamage; // возвращает урон от определенного тайла
        }
    }
    public projectTileType PType
    {
        get
        {
            return pType; // возвращаем тип снаряда
        }
    }
}
