using UnityEngine;

public class InputProviderFactory
{
    private readonly TouchControlsView _touchControlsView;

    public InputProviderFactory(TouchControlsView touchControlsView)
    {
        _touchControlsView = touchControlsView;
    }

    public IInputProvider Create()
    {
        if (Application.isMobilePlatform)
            return new TouchInputProvider(_touchControlsView.Joystick, _touchControlsView.FireButton,
                _touchControlsView.LaserButton);

        _touchControlsView.Joystick.gameObject.SetActive(false);
        _touchControlsView.FireButton.gameObject.SetActive(false);
        _touchControlsView.LaserButton.gameObject.SetActive(false);
        return new KeyboardInputProvider();
    }
}