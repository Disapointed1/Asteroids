using UnityEngine;

public class WorldBoundary
{
    private const float SpawnMargin = 1f;

    public WorldBoundary(float width, float height)
    {
        HalfWidth = width / 2f;
        HalfHeight = height / 2f;
    }

    public float HalfWidth { get; }

    public float HalfHeight { get; }

    public void WrapPosition(PhysicsMovement body)
    {
        var position = body.Position;

        if (position.x > HalfWidth)
            position.x = -HalfWidth;
        else if (position.x < -HalfWidth)
            position.x = HalfWidth;

        if (position.y > HalfHeight)
            position.y = -HalfHeight;
        else if (position.y < -HalfHeight)
            position.y = HalfHeight;

        body.Position = position;
    }

    public Vector2 GetRandomPositionOutside()
    {
        var side = (Side)Random.Range(0, 4);
        float x, y;

        switch (side)
        {
            case Side.Top:
                x = Random.Range(-HalfWidth, HalfWidth);
                y = HalfHeight + SpawnMargin;
                break;
            case Side.Bottom:
                x = Random.Range(-HalfWidth, HalfWidth);
                y = -HalfHeight - SpawnMargin;
                break;
            case Side.Left:
                x = -HalfWidth - SpawnMargin;
                y = Random.Range(-HalfHeight, HalfHeight);
                break;
            default:
                x = HalfWidth + SpawnMargin;
                y = Random.Range(-HalfHeight, HalfHeight);
                break;
        }

        return new Vector2(x, y);
    }
}