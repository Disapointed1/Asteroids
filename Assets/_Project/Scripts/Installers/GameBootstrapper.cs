using Zenject;

public class GameBootstrapper : IInitializable
{
    private readonly LevelFactory _levelFactory;

    public GameBootstrapper(LevelFactory levelFactory)
    {
        _levelFactory = levelFactory;
    }

    public void Initialize()
    {
        _levelFactory.BuildLevel();
    }
}