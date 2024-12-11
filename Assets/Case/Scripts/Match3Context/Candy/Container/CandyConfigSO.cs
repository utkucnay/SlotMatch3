using UnityEngine;
using Zenject;

namespace Case.Match3.Candy
{
    [CreateAssetMenu(fileName = "CandyConfigSO", menuName = "Installers/CandyConfigSO")]
    public class CandyConfigSO : ScriptableObjectInstaller<CandyConfigSO>
    {
        public CandyViewImage candyViewImage;

        public override void InstallBindings()
        {
            Container.Bind<CandyViewImage>().FromInstance(candyViewImage).AsSingle();
        }
    }
}