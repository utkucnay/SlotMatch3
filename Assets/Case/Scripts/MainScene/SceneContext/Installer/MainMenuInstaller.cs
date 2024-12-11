using Case.MainScene.CongratulationPopup;
using Case.MainScene.Game;
using Case.Match3;
using UnityEngine;
using Zenject;

namespace Case.MainScene.SceneContext
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private GameObject candyPrefab;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Match3Container.InstallBinding(Container, candyPrefab);
            GameContainer.InstallBinding(Container);
            CongratulationPopupContainer.InstallBinding(Container);
        }
    }
}
