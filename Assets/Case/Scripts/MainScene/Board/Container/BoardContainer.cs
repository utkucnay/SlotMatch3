using Zenject;

namespace Case.MainScene.Board
{
    public static class BoardContainer
    {
        public static void InstallBinding(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<BoardController>().AsSingle();
        }
    }
}
