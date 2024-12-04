using Zenject;

namespace Case.MainScene.Spin
{
    public static class SpinContainer
    {
        public static void InstallBinding(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<SpinController>().AsSingle();
        }
    }
}