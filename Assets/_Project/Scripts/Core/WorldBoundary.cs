using UnityEngine;

public class WorldBoundary
{
    private const float SpawnMargin = 1f;

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

    public Vector2 GetRandomPositionOutside()
    {
        int side = Random.Range(0, 4);
        float x, y;

        switch (side)
        {
            case 0:
                x = Random.Range(-_halfWidth, _halfWidth);
                y = _halfHeight + SpawnMargin;
                break;
            case 1:
                x = Random.Range(-_halfWidth, _halfWidth);
                y = -_halfHeight - SpawnMargin;
                break;
            case 2:
                x = -_halfWidth - SpawnMargin;
                y = Random.Range(-_halfHeight, _halfHeight);
                break;
            default:
                x = _halfWidth + SpawnMargin;
                y = Random.Range(-_halfHeight, _halfHeight);
                break;
        }
        return new Vector2(x, y);
    }
}