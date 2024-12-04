using Case.MainScene.Game.Signal;
using Zenject;

namespace Case.MainScene.Game
{
    public static class GameContainer
    {
        public static void InstallBinding(DiContainer container)
        {
            container.DeclareSignal<GameEndSignal>();

            container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }
    }
}
