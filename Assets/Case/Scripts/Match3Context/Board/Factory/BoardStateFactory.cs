using Zenject;

namespace Case.Match3.Board
{
    public enum BoardState 
    {
        Idle,
        Spin,
    }

    public class BoardStateFactory
    {
        DiContainer _container;

        public BoardStateFactory(DiContainer container)
        {
            _container = container;

            _container.Bind<IBoardState>().WithId(BoardState.Idle).To<IdleBoardState>().AsTransient();
            _container.Bind<IBoardState>().WithId(BoardState.Spin).To<SpinBoardState>().AsTransient();
        }

        public IBoardState Create(BoardState boardState)
        {
            return _container.ResolveId<IBoardState>(boardState);
        }
    }
}