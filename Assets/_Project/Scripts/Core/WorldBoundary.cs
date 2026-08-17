
using UnityEngine;

public class WorldBoundary
{
    private readonly float _halfWidth;
    private readonly float _halfHeight;

    public float HalfWidth => _halfWidth;
    public float HalfHeight => _halfHeight;

    public WorldBoundary(float width, float height)
    {
        _halfWidth = width / 2f;
        _halfHeight = height / 2f;
    }

    public void WrapPosition(PhysicsMovement body)
    {
        Vector2 position = body.Position;

        if (position.x > _halfWidth)
            position.x = -_halfWidth;
        else if (position.x < -_halfWidth)
            position.x = _halfWidth;

        if (position.y > _halfHeight)
            position.y = -_halfHeight;
        else if (position.y < -_halfHeight)
            position.y = _halfHeight;

        body.Position = position;
    }

}
