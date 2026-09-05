public class FireLaserCommand : ICommand
{
    private readonly Ship _ship;

    public FireLaserCommand(Ship ship)
    {
        _ship = ship;
    }

    public void Execute()
    {
        _ship.TryUseLaserCharge();
    }


}
