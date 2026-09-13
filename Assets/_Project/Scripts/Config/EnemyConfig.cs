using System;

[Serializable]
public class EnemyConfig
{
    public float AsteroidSpeed;
    public float UfoSpeed;
    public float AsteroidLargeRadius;
    public float AsteroidMediumRadius;
    public float AsteroidSmallRadius;
    public float UfoRadius;
    public float UfoMass;
    public float MinSpawnDelay;
    public float MaxSpawnDelay;
    public int FragmentsPerSplit;
    public float SmallerFragmentSpeedMultiplier;
    public int AsteroidLargeReward;
    public int AsteroidMediumReward;
    public int AsteroidSmallReward;
    public int UfoReward;
}