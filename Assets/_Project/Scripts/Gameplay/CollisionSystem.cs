using System.Collections.Generic;

public class CollisionSystem
{
    private readonly List<ICollisionHandler> _collisionHandlers;
    private readonly LaserCollisionHandler _laserCollisionHandler;

    public CollisionSystem(List<ICollisionHandler> collisionHandlers, LaserCollisionHandler laserCollisionHandler)
    {
        _collisionHandlers = collisionHandlers;
        _laserCollisionHandler = laserCollisionHandler;
    }

    public void CheckCollisions()
    {
        foreach (var handler in _collisionHandlers )
                handler.CheckCollisions();
    }

    public void Dispose()
    {
        _laserCollisionHandler.Dispose();
    }
}