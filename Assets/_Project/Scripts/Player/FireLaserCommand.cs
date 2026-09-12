public class FireLaserCommand : ICommand
{
    private readonly LaserWeapon _laserWeapon;

    public FireLaserCommand(LaserWeapon laserWeapon)
    {
        _laserWeapon = laserWeapon;
    }

    public void Execute()
    {
        _laserWeapon.TryFire();
    }


}
