using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    TowerControl towerObject; // место для башни
    [SerializeField]
    Sprite dragSprite; // картинка башни выбранной кнопки
    [SerializeField]
    int towerPrice; // цена башни

    public TowerControl TowerObject
    {
        get
        {
            return towerObject;
        }
    }
    public Sprite DragSprite
    {
        get
        {
            return dragSprite;
        }
    }
    public int TowerPrice
    {
        get
        {
            return towerPrice; // возвращаем стоимость башни
        }
    }
}
