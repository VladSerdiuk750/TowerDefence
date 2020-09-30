using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks; // скорость стрельбы (время между выстрелами)
    [SerializeField]
    float attackRadius; // радиус атаки
    [SerializeField]
    ProjectTile projectTile; // сам вид снаряда
    
    Enemy targetEnemy = null; // по какому потивнику стреляем (изначально не по кому стрелять)

    float attackCounter; // счетчик для стрельбы

    bool isAttacking = false; // можно ли стрелять (изначально = нет)

    bool projectileDel = false;

    void Start()
    {
        
    }
    void FixedUpdate()
    {
       attackCounter -= Time.deltaTime; // снижается с течением времени

        if (targetEnemy == null || targetEnemy.IsDead) // еще нет цели по которой стрелять или цель умерла (свойство)
        {
            Enemy nearestEnemy = GetNearestEnemy(); // считываем ближайшего противника из другого скрипта

            if (nearestEnemy != null && !nearestEnemy.IsDead && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius) // если есть кто-то по близости и расстояние от башни до противника меньше возможного радиуса для стрельбы
            {
                targetEnemy = nearestEnemy; // то цель становится ближайшей целью для стрельбы
            }
        }
        else
        {
            // если задержка между выстрелами прошла
            if (attackCounter <= 0)
            {
                // то можно стрелять
                isAttacking = true;
                projectileDel = false;
                Attack(); // стреляем

                // счетчик должен быть востановлен
                attackCounter = timeBetweenAttacks;
            }
            else
            {
                isAttacking = false;
            }

            // если расстояние до цели больше радиуса башни
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null; // то мы теряем цель
            }
        }

        //if (isAttacking == true) // если можно стрелять
        //{
        //    Attack(); // стреляем
        //}
    }
    public void Attack()
    {
        isAttacking = false; // изначально башня не стреляет
        ProjectTile newProjectTile = Instantiate(projectTile) as ProjectTile;
        newProjectTile.transform.localPosition = transform.localPosition; // где будет появлятся
        
        // воспроизводим звуки выстрелов разных типов снарядов
        if (newProjectTile.PType == projectTileType.cotton)
        {
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);
        }
        else if (newProjectTile.PType == projectTileType.arrow)
        {
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Rock);
        }
        else if (newProjectTile.PType == projectTileType.currency)
        {
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.FireBall);
        }

        if (targetEnemy == null) // если цель потеряна
        {
            Destroy(newProjectTile); // уничтожаем снаряд
        }
        else
        {
            // двигаем снаряд к врагу через корутину
            StartCoroutine(MoveProjectTile(newProjectTile));
        }
    }
    IEnumerator MoveProjectTile(ProjectTile projectTile)
    {
        // пока расстояние до противника больше чем (0.2f) и есть снаряд и есть цель
        while (GetTargetDistance(targetEnemy) > 0.2f && projectTile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition; // то дистанция уменьшается
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // направление относитльно противника (угол)
            projectTile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward); // следуем за противников поворачивая снаряд
            projectTile.transform.localPosition = Vector2.MoveTowards(projectTile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime); // с какой скоростью движется снаряд (5f)

            yield return null;
        }
        
        // если у нас есть снаряд но нет врага под него
        if (projectTile != null || targetEnemy == null || targetEnemy.IsDead || projectileDel)
        {
            Destroy(projectTile); // уничтожаем снаряд
        }
    }
    private float GetTargetDistance(Enemy thisEnemy) // запрашиваем расстояние до цели
    {
        if (thisEnemy == null) // если наш противник равняется Null
        {
            thisEnemy = GetNearestEnemy(); // то он становится ближайшим противником
            if (thisEnemy == null)
            {
                return 0f;
            }
        }

        // считывается постоянное растояние до противника
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }
    private List<Enemy> GetEnemiesInRange() // какие противники в зоне поражения
    {
        List<Enemy> enemiesInRange = new List<Enemy>(); // подтягивает из другого листа

        foreach (Enemy enemy in GameManager.Instance.EnemyList) // ищет в листе противников
        {
            // если расстояние от башни до противника меньше или равно радиусу стрельбы
            if (!enemy.IsDead && Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy); // то добавляем врага в лист возможных для выстрела
            }
        }
        return enemiesInRange; // возвращает противников дошедших до зоны поражения
    }
    private Enemy GetNearestEnemy() // находит ближайшего врага
    {
        Enemy nearestEmeny = null; // ближайший враг (изначально никакой)
        float smallestDistance = float.PositiveInfinity; // определяет ближайшего врага (расстояние)

        foreach (Enemy enemy in GetEnemiesInRange()) // считывает всех противников в диапозоне стрельбы
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance && !enemy.IsDead) // если расстоние от башни до противников минимальное из всех
            {
                // просчет самого расстояния
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition); 
                nearestEmeny = enemy; // делаем его ближайшим врагом
            }
        }
        return nearestEmeny; // возвращает ближайшего врага
    }

    public void DeleteAllProjectiles()
    {
        projectileDel = true;
    }

    public void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, attackRadius);
    }

    // ПРОПИСАТЬ GIZMOS ДЛЯ РАДИУСА
}
