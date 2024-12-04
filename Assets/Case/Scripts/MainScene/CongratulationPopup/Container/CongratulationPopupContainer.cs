using Zenject;

namespace Case.MainScene.CongratulationPopup
{
    public static class CongratulationPopupContainer
    {
        public static void InstallBinding(DiContainer container)
        {
            container.Bind<CongratulationPopupController>().AsSingle();
        }
    }
}
