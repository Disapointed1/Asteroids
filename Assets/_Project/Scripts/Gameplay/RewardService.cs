using System.Collections.Generic;

public class RewardService
{
    private readonly Dictionary<EnemyType,int> _rewards = new Dictionary<EnemyType, int>()
    {
        {EnemyType.AsteroidLarge, 20},
        { EnemyType.AsteroidMedium , 50},
        { EnemyType.AsteroidSmall , 100},
        { EnemyType.Ufo , 200}
    };

    public int GetReward(EnemyType type)
    {
        return _rewards[type];
    }

}
