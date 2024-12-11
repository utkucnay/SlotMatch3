using Zenject;

namespace Case.Match3.Board
{
    public static class BoardContainer
    {
        public static void InstallBinding(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<BoardController>().AsSingle();
            container.Bind<BoardStateFactory>().AsCached();
        }
    }
}
