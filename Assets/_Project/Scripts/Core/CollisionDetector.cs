using UnityEngine;

public class CollisionDetector
{
    public bool CheckCollision(PhysicsMovement bodyA, PhysicsMovement bodyB)
    {
        var distance = Vector2.Distance(bodyA.Position, bodyB.Position);
        var radiusSum = bodyA.Radius + bodyB.Radius;

        return distance <= radiusSum;
    }

    public bool CheckLaserHit(Vector2 laserStart, Vector2 laserEnd, PhysicsMovement target)
    {
        var laserDirection = laserEnd - laserStart;
        var laserLength = laserDirection.magnitude;
        laserDirection.Normalize();

        var toTarget = target.Position - laserStart;
        var projection = Vector2.Dot(toTarget, laserDirection);
        projection = Mathf.Clamp(projection, 0f, laserLength);

        var closestPoint = laserStart + laserDirection * projection;
        var distance = Vector2.Distance(closestPoint, target.Position);

        return distance < target.Radius;
    }
}