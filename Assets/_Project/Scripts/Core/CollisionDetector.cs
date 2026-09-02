using UnityEngine;

public class CollisionDetector
{
    public bool CheckCollision(PhysicsMovement bodyA, PhysicsMovement bodyB)
    {
        float distance = Vector2.Distance(bodyA.Position, bodyB.Position);
        float radiusSum = bodyA.Radius + bodyB.Radius;

        return ( distance <= radiusSum );

    }

    public bool CheckLaserHit(Vector2 laserStart, Vector2 laserEnd, PhysicsMovement target)
    {
        Vector2 laserDirection = laserEnd - laserStart;
        float laserLength  = laserDirection.magnitude;
        laserDirection.Normalize();

        Vector2 toTarget = target.Position - laserStart;
        float projection = Vector2.Dot(toTarget, laserDirection);
        projection = Mathf.Clamp(projection, 0f, laserLength);

        Vector2 clossetPoint = laserStart + laserDirection * projection;
        float distance = Vector2.Distance(clossetPoint, target.Position);

        return distance < target.Radius;
    }

}
