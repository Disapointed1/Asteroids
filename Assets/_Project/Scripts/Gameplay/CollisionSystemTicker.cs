using UnityEngine;

public class CollisionSystemTicker : MonoBehaviour
{
    private CollisionSystem _collisionSystem;
    private BulletMovementSystem _bulletMovementSystem;
    private EnemyMovementSystem _enemyMovementSystem;

    private void FixedUpdate()
    {
        if (_collisionSystem == null) return;
        var deltaTime = Time.fixedDeltaTime;
        _bulletMovementSystem.UpdateMovement(deltaTime);
        _enemyMovementSystem.UpdateMovement(deltaTime);
        _collisionSystem.CheckCollisions();
    }

    public void Initialize(CollisionSystem collisionSystem, BulletMovementSystem bulletMovementSystem,
        EnemyMovementSystem enemyMovementSystem)
    {
        _collisionSystem = collisionSystem;
        _bulletMovementSystem = bulletMovementSystem;
        _enemyMovementSystem = enemyMovementSystem;
    }
}