using UnityEngine;

public class PhysicsMovement
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Radius { get; set; }
    public float DragCoefficient { get; set; }
    public float Mass { get; set; }

    public void ApplyAcceleration(Vector2 acceleration, float deltaTime)
    {
        Velocity += acceleration * deltaTime;
    }

    public void UpdatePosition(float deltaTime)
    {
        Position += Velocity * deltaTime;
    }

    public void ApplyDrag(float deltaTime)
    {
        Velocity *= ( 1 - DragCoefficient * deltaTime);
    }
}
