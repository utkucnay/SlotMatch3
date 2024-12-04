using Case.MainScene.Board;
using Case.MainScene.Candy;
using Case.MainScene.CongratulationPopup;
using Case.MainScene.Game;
using Case.MainScene.Spin;
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

            CandyContainer.InstallBinding(Container, candyPrefab);
            BoardContainer.InstallBinding(Container);
            SpinContainer.InstallBinding(Container);
            GameContainer.InstallBinding(Container);
            CongratulationPopupContainer.InstallBinding(Container);
        }
    }
}
