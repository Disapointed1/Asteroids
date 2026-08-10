using UnityEngine;

public class CollisionDetector
{
    public bool CheckCollision(PhysicsMovement bodyA, PhysicsMovement bodyB)
    {
        float distance = Vector2.Distance(bodyA.Position, bodyB.Position);
        float radiusSum = bodyA.Radius + bodyB.Radius;

        return ( distance <= radiusSum );

    }
}
