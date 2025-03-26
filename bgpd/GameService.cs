using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace bgpd;

public interface IGameService
{
    IObservable<List<BGEntity>> Entities { get; }
}

public class GameService : IGameService
{
    private readonly Subject<List<BGEntity>> _entities = new();
    private readonly ProcessHacker _processHacker = new();
    public IObservable<List<BGEntity>> Entities => _entities;

    public GameService()
    {
        _processHacker.Init();

        Task.Factory.StartNew(() =>
        {
            Logger.Debug("Main loop started");

            while (true)
            {
                try
                {
                    _processHacker.MainLoop();
                }
                catch (Exception ex)
                {
                    Logger.Error("Main loop error!", ex);
                }
            }
        });

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(_ => _processHacker.entityList)
            .Subscribe(_entities);
    }
}