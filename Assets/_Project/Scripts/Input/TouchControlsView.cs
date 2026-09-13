using UnityEngine;

public class TouchControlsView : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private TouchButton _fireButton;
    [SerializeField] private TouchButton _laserButton;

    public Joystick Joystick => _joystick;
    public TouchButton FireButton => _fireButton;
    public TouchButton LaserButton => _laserButton;
}