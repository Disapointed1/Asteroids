public class EnemyCounterTracker
{
    private readonly int _maxEnemies;
    private int _currentCount;

    public EnemyCounterTracker(int maxEnemies)
    {
        _maxEnemies = maxEnemies;
    }

    public bool CanSpawn()
    {
        return _currentCount < _maxEnemies;
    }

    public void RegisterSpawned()
    {
        _currentCount++;
    }
    public void RegisterDestroyed()
    {
        _currentCount--;
    }

}
