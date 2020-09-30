using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    public TowerBtn towerBtnPressed { 
        get
        {
            return _towerBtnPressed;
        }
        set
        {
            
            if(value == null || IsTowerAffordable(value)) // проверка можем ли мы её позволить
            {
                _towerBtnPressed = value;
            }
            else _towerBtnPressed = null;
        }
    } // нажата ли кнопка

    private TowerBtn _towerBtnPressed;

    SpriteRenderer spriteRenderer; // для отображения картинки при перетягивании башни
   
    // лист для башен
    private List<TowerControl> TowerList = new List<TowerControl>();
    // лист коллайдеров башен
    private List<Collider2D> BuildList = new List<Collider2D>(); // 
    private Collider2D buildTile;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // реализуем 
        //buildTile = GetComponent<Collider2D>(); // реализуем коллайдеры башен
        spriteRenderer.enabled = false; // на всякий случай в начале игры удаляем все картинки башен
    }

    void Update()
    {
        if(towerBtnPressed != null) // Если кто-то нажал на башню
        {
            EnableDrag(towerBtnPressed.DragSprite); // считывает с кнопки ту башню которая нам нужна
            if (Input.GetMouseButtonDown(0)) // Если нажимаем левую кнопку
            {
                Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // считывает где мы кликаем мышью относительно нашей камеры (и куда навели там и будет башня)
                RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero); // создаем луч считывающий можно ли нам ставить башню

                if (hit.collider.tag == "TowerSide") // если тег "TowerSide"
                {
                    BuyTower(towerBtnPressed.TowerPrice); // покупаем башню
                    PlaceTower(hit); // размещаем башню
                    DisableDrag(); // деактивируем картинку башни при перетаскивании
                    buildTile = hit.collider; // переименовываем
                    RegisterBuildSite(buildTile); // регистрируем новую башню
                    buildTile.tag = "TowerSideFull"; // заменяем тег чтобы считывать что бащня уже поставленна в этом месте
                    towerBtnPressed = null; // выключаем прожатую кнопку башни
                    //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt); // звук постройки башни
                }
            }   
            if(Input.GetMouseButtonDown(1))// скидываем башню при нажатии правой кнопки
            {
                DisableDrag();
                towerBtnPressed = null;
            }
            if (spriteRenderer.enabled) // если спрайт активен
            {
                FollowMouse();
            }
        }
    }

    public void OnTowerBtnClick(TowerBtn newTowerBtnPressed)
    {
        towerBtnPressed = newTowerBtnPressed;
    }

    private bool IsTowerAffordable(TowerBtn towerSelected) //проверка можем ли мы позволить купить эту башню
    {
        if(towerSelected.TowerPrice > GameManager.Instance.TotalMoney)
        {
            return false;
        }
        else return true;
    }
    public void PlaceTower(RaycastHit2D towerPlace) 
    {
        // если мы не навели не на одну из кнопок и если мы нажали на кнопку
        if (!EventSystem.current.IsPointerOverGameObject()) // мы не сможем поставить башню если мы навели курсор на одну из кнопок
        {
            TowerControl newTower = Instantiate(towerBtnPressed.TowerObject); // спавним башню привязанную к определенной кнопке 
            newTower.transform.position = towerPlace.transform.position; // башня помещается по тем координатам где мы нажали
            RegisterTower(newTower); // регистрируем новую башню
        }      
    }
    public void RegisterTower(TowerControl tower)
    {
        TowerList.Add(tower); // добавляем к уже зарегестрированным башням - новую
    }
    public void RegisterBuildSite(Collider2D buildTag) // считывает информацию о том где стоит какая башня
    {
        BuildList.Add(buildTag); // добавляем в список башню    
    }
    
    public void RenameTagBuildSite()
    {
        foreach (Collider2D buildTag in BuildList) // перебираем теги всего списка
        {
            buildTag.tag = "TowerSide"; // меняем тег на снова не "заполненую башней"
        }
        BuildList.Clear(); // очищаем список
    }
    public void DestroyAllTowers()
    {
        foreach (TowerControl tower in TowerList) // перебираем все башни
        {
            Destroy(tower.gameObject); // удаляем все башни
        }
        TowerList.Clear(); // очищаем список
    }

    public void DeleteAllProjectiles()
    {
        foreach (TowerControl tower in TowerList) // перебираем все башни
        {
            tower.DeleteAllProjectiles();
        }
    }
    
    public void BuyTower(int price)
    {
        GameManager.Instance.SubtractMoney(price); // вычитаем стоимость башни
    }

    public void FollowMouse() // слежение башни за мышью
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // привязка к курсору относительно экрана
        transform.position = new Vector2(transform.position.x, transform.position.y); // изменение позиции
    }

    public void EnableDrag(Sprite sprite) // метод что мы можем передвигать нашу башню за курсором
    {
        spriteRenderer.enabled = true; // включает отображение картинки
        spriteRenderer.sprite = sprite; // картинка одной из башен
    }
    public void DisableDrag() // деактивация картинки движения башни за курсором 
    {
        spriteRenderer.enabled = false; // выключает отображение картинки
    }

    // public void SelectedTower(TowerBtn towerSelected) // проверка какая башня выбрана для постройки
    // {
    //     // если башня стоит дешевле чем у нас есть денег
    //     if (towerSelected.TowerPrice <= Manager.Instance.TotalMoney)
    //     {
    //         // то мы можем её купить
    //         towerBtnPressed = towerSelected; // каждой кнопке соответсвует своя башня
    //     }
    // }
}
