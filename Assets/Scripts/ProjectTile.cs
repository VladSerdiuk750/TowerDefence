using UnityEngine;

/// <summary>
/// Enum for types of projectiles
/// </summary>
public enum ProjectTileType 
{ 
    Cotton, Cellphone, Arrow, Currency
}

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private int attackDamage; // projectile damage

    [SerializeField] private ProjectTileType pType; // type of projectile

    [SerializeField] private float speed;

    public Enemy Target { get; set; } // target enemy of projectile

    public int AttackDamage => attackDamage;
    
    public ProjectTileType PType => pType;
    
    private void Update()
    {
        if (Target is null || Target.IsDead)
        {
            Destroy(gameObject);
            return;
        }
        Move();
    }

    /// <summary>
    /// If projectile enters Enemy it self-destroy after 0.1 seconds
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject, 0.001f);
        }
    }

    /// <summary>
    /// Moving our projectile towards target
    /// </summary>
    private void Move()
    {
        if (!(GetTargetDistance(Target.transform) > 0.2f)) return;
        var localPosition = transform.localPosition;
        var position = Target.transform.localPosition;
        var direction = position - localPosition;
        var angleDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
        localPosition = Vector2.MoveTowards(
            localPosition, position, speed * Time.deltaTime);
        transform.localPosition = localPosition;
    }
    
    /// <summary>
    /// Getting distance to target
    /// </summary>
    /// <param name="enemyTransform">Transform of enemy</param>
    /// <returns>Distance to target</returns>
    private float GetTargetDistance(Transform enemyTransform) => 
        Mathf.Abs(Vector2.Distance(transform.localPosition, enemyTransform.localPosition));
}
