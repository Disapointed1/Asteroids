using UnityEngine;

public class CollisionResolver
{
    public void ResolveCollision(PhysicsMovement bodyA, PhysicsMovement bodyB)
    {
            Vector2 normal = (bodyA.Position - bodyB.Position).normalized;
            Vector2 relativeVelocity = bodyB.Velocity - bodyA.Velocity;
            float velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

            if (velocityAlongNormal > 0)
                return;

            float impulse = 2 * velocityAlongNormal / (bodyA.Mass + bodyB.Mass);

            bodyA.Velocity -= impulse * bodyB.Mass * normal;
            bodyB.Velocity += impulse * bodyA.Mass * normal;

    }
}
