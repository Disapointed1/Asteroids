using UnityEngine;
using Zenject;

public class GameBootstrapper : IInitializable
{
    private readonly ShipView _shipView;

    public GameBootstrapper(ShipView shipView)
    {
        _shipView = shipView;
    }

    public void Initialize()
    {
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;

        WorldBoundary worldBoundary = new WorldBoundary(width, height);
        Ship ship = new Ship(0.5f, 1f, 0.5f);
        KeyboardInputProvider inputProvider  =  new KeyboardInputProvider();
        ShipController shipController = new ShipController(ship, inputProvider, 180f, 5f, worldBoundary);
        _shipView.Initialize(ship,shipController);
    }
}
