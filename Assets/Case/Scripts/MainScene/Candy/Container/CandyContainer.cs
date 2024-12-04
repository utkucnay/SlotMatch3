using Case.MainScene.Candy;
using UnityEngine;
using Zenject;

namespace Case.MainScene.Candy
{
    public static class CandyContainer
    {
        public static void InstallBinding(DiContainer container, GameObject candyPrefab)
        {
            container.BindFactory<CandyView, CandyView.Factory>().FromComponentInNewPrefab(candyPrefab).AsSingle();
            container.Bind<CandyViewService>().AsSingle();
        }
    }
}