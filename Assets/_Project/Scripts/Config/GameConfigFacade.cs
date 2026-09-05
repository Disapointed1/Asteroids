public class GameConfigFacade
{
    public PlayerConfig Player { get; }
    public EnemyConfig Enemy { get; }
    public WorldConfig World { get; }

    public GameConfigFacade()
    {
        Player = ConfigLoader.Load<PlayerConfig>("player_config");
        Enemy = ConfigLoader.Load<EnemyConfig>("enemy_config");
        World = ConfigLoader.Load<WorldConfig>("world_config");
    }



}
