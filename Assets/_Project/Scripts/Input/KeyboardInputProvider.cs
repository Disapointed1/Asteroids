using UnityEngine;

public class KeyboardInputProvider : IInputProvider
{
    public Vector2 GetMovementInput()
    {
        if (Input.GetKey(KeyCode.W))
           return new Vector2(0, 1);
        return Vector2.zero;
    }

    public float GetRotationInput()
    {
        if (Input.GetKey(KeyCode.A))
            return 1f;
        if(Input.GetKey(KeyCode.D))
            return -1f;
        return 0f;
    }
}
