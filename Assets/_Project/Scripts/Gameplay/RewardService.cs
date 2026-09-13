using System.Collections.Generic;

public class RewardService
{
    private readonly Dictionary<EnemyType, int> _rewards;

    public RewardService(int asteroidLargeReward, int asteroidMediumReward, int asteroidSmallReward, int ufoReward)
    {
        _rewards = new Dictionary<EnemyType, int>
        {
            { EnemyType.AsteroidLarge, asteroidLargeReward },
            { EnemyType.AsteroidMedium, asteroidMediumReward },
            { EnemyType.AsteroidSmall, asteroidSmallReward },
            { EnemyType.Ufo, ufoReward }
        };
    }

    public int GetReward(EnemyType type)
    {
        return _rewards[type];
    }
}