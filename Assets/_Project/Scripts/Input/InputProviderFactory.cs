using UnityEngine;
using Zenject;

public class InputProviderFactory
{
    private readonly Joystick _joystick;
    private readonly TouchButton _fireButton;
    private readonly TouchButton _laserButton;

    public InputProviderFactory(Joystick joystick, [Inject(Id = "Fire")]TouchButton fireButton, [Inject (Id = "Laser")]TouchButton laserButton)
    {
        _joystick = joystick;
        _fireButton = fireButton;
        _laserButton = laserButton;
    }

    public IInputProvider Create()
    {
        if (Application.isMobilePlatform)
        {
            return new TouchInputProvider(_joystick, _fireButton, _laserButton);
        }

        _joystick.gameObject.SetActive(false);
        _fireButton.gameObject.SetActive(false);
        _laserButton.gameObject.SetActive(false);
        return new KeyboardInputProvider();
    }
}