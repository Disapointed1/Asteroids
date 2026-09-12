using UnityEngine;

public static class DirectionMath
{
    public static Vector2 FromAngle(float rotationDegrees)
    {
        return new Vector2(-Mathf.Sin(rotationDegrees * Mathf.Deg2Rad), Mathf.Cos(rotationDegrees * Mathf.Deg2Rad));
    }
}
