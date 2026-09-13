public class GameConfigProvider
{
    private const string PlayerConfigFileName = "player_config";
    private const string EnemyConfigFileName = "enemy_config";
    private const string WorldConfigFileName = "world_config";

    public GameConfigProvider()
    {
        Player = ConfigLoader.Load<PlayerConfig>(PlayerConfigFileName);
        Enemy = ConfigLoader.Load<EnemyConfig>(EnemyConfigFileName);
        World = ConfigLoader.Load<WorldConfig>(WorldConfigFileName);
    }

    public PlayerConfig Player { get; }
    public EnemyConfig Enemy { get; }
    public WorldConfig World { get; }
}