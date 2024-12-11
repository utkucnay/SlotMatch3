using UnityEngine;
using Zenject;

namespace Case.MainScene.CongratulationPopup
{
    [CreateAssetMenu(fileName = "CongratulationPopupConfig", menuName = "Installers/CongratulationPopupConfig")]
    public class CongratulationPopupConfigSO : ScriptableObjectInstaller<CongratulationPopupConfigSO>
    {
        public CongratulationPopupAnimationData congratulationPopupAnimationData;

        public override void InstallBindings()
        {
            Container.Bind<CongratulationPopupAnimationData>().FromInstance(congratulationPopupAnimationData).AsSingle();
        }
    }
}

