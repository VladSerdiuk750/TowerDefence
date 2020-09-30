using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Transform exit; // точка выхода
    [SerializeField]
    Transform[] wayPoints; // массив пути (MovingPoint) по которым идет враг
    [SerializeField]
    float navigation; // для просчитывания передвижения нашего персонажа
    [SerializeField]
    int health; // жизни врага
    [SerializeField]
    int rewardAmount; // награда за убийтсво противника

    int target = 0; // сколько целей (MovingPoint) прошел противник
    Transform enemy; // текущее положение врага
    Collider2D enemyCollider; // коллайдер врага

    Animator anim; // переменная для анимации

    float navigationTime = 0; // обновляем положение врага в пространстве 

    bool isDead = false; // враг умер - не умер (изначально жив)

    public bool IsDead // свойство
    {
        get 
        {
            return isDead; // возвращаем свойство "isDead"
        }
    }
    void Start()
    {
        enemy = GetComponent<Transform>(); // чтобы могли считывать положения врага
        enemyCollider = GetComponent<Collider2D>(); // реализуем
        anim = GetComponent<Animator>(); // реализуем
        GameManager.Instance.RegisterEnemy(this); // указываем на регистрацию противников в этом скрипте     
    }

    void Update()
    {
        if (wayPoints != null && isDead == false) // если еще есть точки по которым можно идти и противник жив
        {
            navigationTime += Time.deltaTime; // продолжаем двигатся к следующей точке

            if (navigationTime > navigation)
            {
                if (target < wayPoints.Length) // если не дошли до конца
                {
                    
                    // двигаемся к следующей точке
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                }
                else // если закончились точки пути
                {
                    // двигаемя к выходу
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigationTime);
                }

                navigationTime = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MovingPoint")
        {
            target += 1; // засчитали пройденый "MovingPoint"
        }
        else if (collision.tag == "Finish")
        {
            GameManager.Instance.RoundEscaped += 1; // добавляем в getter инфу что противник прошел
            GameManager.Instance.TotalEscaped += 1; // и что всего сбежавших так же +1
            GameManager.Instance.UnRegisterEnemy(this); // снимает регистрацию с противников
            GameManager.Instance.IsWaveOver(); // волна может так же закончится 
            GameManager.Instance.IsGameOver();
        }
        else if (collision.tag == "ProjectTile") // при соприкосновении со снарядом
        {
            ProjectTile newP = collision.gameObject.GetComponent<ProjectTile>();
            EnemyHit(newP.AttackDamage); // наносим урон противнику 
            Destroy(collision.gameObject); // уничтожать снаряд
        }
    }
    public void EnemyHit(int hitPoints)
    {
        if (health - hitPoints > 0) // если еще есть жизни
        {
            health -= hitPoints; // отнимает от жизней врага - урон башни
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit); // звук нанисения урона 
            //anim.Play("Hurt"); // проигрываем анимацию получения урона
        }
        else
        {
            // DIE
            anim.SetTrigger("didDie"); // должен сработать триггер

            Invoke("HideDeadEnemy", 1.5f); // скрывает врага после смерти через (2.5 сек) 

            Die();
        }
    }
    public void Die()
    {
        isDead = true; // умер
        enemyCollider.enabled = false; // выключаем коллайдер врага 
        GameManager.Instance.TotalKilled += 1; // записываем что один враг умер (например для прописания награды)
        GameManager.Instance.RoundKilled += 1;
        //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death); // звук смерти
        GameManager.Instance.AddMoney(rewardAmount); // при смерти врага добавляем денег
        GameManager.Instance.IsWaveOver(); // волна может так же закончится
    }
    public void HideDeadEnemy()
    {
        enemy.gameObject.SetActive(false); // скрывает врага после смерти
        Destroy(enemy.gameObject);
    }
}
