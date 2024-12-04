using UnityEngine;
using Zenject;

namespace Case.MainScene.Candy
{
    [CreateAssetMenu(fileName = "CandyInstallerSO", menuName = "Installers/CandyInstallerSO")]
    public class CandyInstallerSO : ScriptableObjectInstaller<CandyInstallerSO>
    {
        public CandyViewImage candyViewImage;

        public override void InstallBindings()
        {
            Container.Bind<CandyViewImage>().FromInstance(candyViewImage).AsSingle();
        }
    }
}