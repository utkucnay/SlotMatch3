using Cysharp.Threading.Tasks;
using Zenject;

namespace Case.MainScene.CongratulationPopup
{
    public class CongratulationPopupController
    {
        [Inject] CongratulationPopupView congratulationPopupView;

        [Inject] CongratulationPopupAnimationData congratulationPopupAnimationData;

        public async UniTask ShowViewAndStartAnimation()
        {
            congratulationPopupView.gameObject.SetActive(true);
            await congratulationPopupView.GameEndAnimation(congratulationPopupAnimationData);
        }
    }
}
