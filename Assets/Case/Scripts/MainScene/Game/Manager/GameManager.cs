using Case.MainScene.Board;
using Case.MainScene.CongratulationPopup;
using Case.MainScene.Game.Signal;
using Case.MainScene.Spin;
using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace Case.MainScene.Game
{
    public class GameManager : IInitializable, IDisposable
    {
        [Inject] SignalBus signalBus;
        [Inject] BoardController boardController;
        [Inject] SpinController spinController;
        [Inject] CongratulationPopupController congratulationPopupController;

        public void Initialize()
        {
            signalBus.Subscribe<GameEndSignal>(OnGameEnd);
        }
        public void Dispose()
        {
            signalBus.Unsubscribe<GameEndSignal>(OnGameEnd);
        }

        private async void OnGameEnd()
        {
            boardController.ShowView(false);
            spinController.ShowView(false);
            await congratulationPopupController.ShowViewAndStartAnimation();
            SceneManager.LoadScene(0);
        }
    }
}
