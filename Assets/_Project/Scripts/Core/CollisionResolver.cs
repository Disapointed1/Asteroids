using UnityEngine;

public class CollisionResolver
{
    public void ResolveCollision(PhysicsMovement bodyA, PhysicsMovement bodyB)
    {
        var normal = (bodyA.Position - bodyB.Position).normalized;
        var relativeVelocity = bodyB.Velocity - bodyA.Velocity;
        var velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

        if (velocityAlongNormal > 0)
            return;

        var impulse = 2 * velocityAlongNormal / (bodyA.Mass + bodyB.Mass);

        bodyA.Velocity -= impulse * bodyB.Mass * normal;
        bodyB.Velocity += impulse * bodyA.Mass * normal;
    }
}