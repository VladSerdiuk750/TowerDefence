using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    [SerializeField] private Transform exitPoint; //point of exit

    [SerializeField] private int health;
    
    [SerializeField] private int rewardAmount;

    public int RewardAmount => rewardAmount;

    private Vector3 _target; //target way point

    private int _wayPointsIndex = 0; //current way point index
    
    private bool _isDead;
    public bool IsDead => _isDead; // is Enemy dead
    
    private Collider2D _enemyCollider;
    private Animator _animator;
    
    private void Awake()
    {
        _enemyCollider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        if (MovingPoints.Points.Length != 0)
        {
            _target = MovingPoints.Points[0].position;
        }

        _isDead = false;
    }

    private void FixedUpdate()
    {
        if (_isDead) return;
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        var direction = _target - transform.position;
        transform.Translate(direction.normalized * (speed * Time.deltaTime), Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingPoint"))
        {
            _wayPointsIndex += 1;
            _target = _wayPointsIndex < MovingPoints.Points.Length ? 
                MovingPoints.Points[_wayPointsIndex].position : exitPoint.position;
        }
        else if (collision.CompareTag("Finish"))
        {
            GameManager.Instance.OnEnemyEscape(this);
        }
        else if (collision.CompareTag("ProjectTile"))
        {
            var newP = collision.gameObject.GetComponent<ProjectTile>();
            EnemyHit(newP.AttackDamage);
        }
    }

    private void EnemyHit(int hitPoints)
    {
        if (health - hitPoints > 0)
        {
            health -= hitPoints;
        }
        else
        {
            _animator.SetTrigger("didDie");
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        HideDeadEnemy(1.0f);
        
        _enemyCollider.enabled = false;

        GameManager.Instance.OnEnemyDie(this);
    }

    private void HideDeadEnemy(float time)
    {
        Destroy(gameObject, time);
    }
}