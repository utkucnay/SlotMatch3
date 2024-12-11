using Zenject;

namespace Case.Match3.Spin
{
    public static class SpinContainer
    {
        public static void InstallBinding(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<SpinController>().AsSingle();
        }
    }
}