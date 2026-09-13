using UnityEngine;

public class CollisionSystemTicker : MonoBehaviour
{
    private CollisionSystem _collisionSystem;

    private void FixedUpdate()
    {
        if (_collisionSystem == null) return;
        _collisionSystem.CheckCollisions();
    }

    public void Initialize(CollisionSystem collisionSystem)
    {
        _collisionSystem = collisionSystem;
    }
}