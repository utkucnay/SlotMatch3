using Case.MainScene.Board;
using System;
using Zenject;

namespace Case.MainScene.Spin
{
    public class SpinController : IInitializable, IDisposable
    {
        [Inject] SpinView spinView;
        [Inject] BoardController boardController;

        SpinModel spinModel = new();

        public void Initialize()
        {
            spinView.OnSpinButtonClick += SpinView_OnSpinButtonClick;
            boardController.OnFirstSwipe += BoardController_OnFirstSwipe;
        }

        public void Dispose()
        {
            spinView.OnSpinButtonClick -= SpinView_OnSpinButtonClick;
            boardController.OnFirstSwipe -= BoardController_OnFirstSwipe;
        }

        private void BoardController_OnFirstSwipe()
        {
            spinView.SetInteractableSpinButton(false);
        }

        private async void SpinView_OnSpinButtonClick(object sender, EventArgs arg)
        {
            if (spinModel.IsSpining)
            {
                spinView.SetSpinText("Spin");
                spinView.SetInteractableSpinButton(false);
                await boardController.SetNullBoardState();
                spinView.SetInteractableSpinButton(true);
            }
            else
            {
                spinView.SetSpinText("Stop");
                spinView.SetInteractableSpinButton(false);
                await boardController.SetSpinBoardState();
                spinView.SetInteractableSpinButton(true);
            }

            spinModel.IsSpining = !spinModel.IsSpining;
        }

        public void ShowView(bool isShow)
        {
            spinView.gameObject.SetActive(isShow);
        }
    }
}
