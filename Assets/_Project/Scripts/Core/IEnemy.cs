public interface IEnemy : IRewardable
{
     PhysicsMovement Physics { get; }
     void TakeHit();
}
