using Cysharp.Threading.Tasks;
using Zenject;

namespace Case.MainScene.CongratulationPopup
{
    public class CongratulationPopupController
    {
        [Inject] CongratulationPopupView congratulationPopupView;

        CongratulationPopupConfig congratulationPopupConfig = new();

        public async UniTask ShowViewAndStartAnimation()
        {
            congratulationPopupView.gameObject.SetActive(true);
            await congratulationPopupView.GameEndAnimation(congratulationPopupConfig);
        }
    }
}
