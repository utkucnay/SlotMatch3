using Case.Match3.Board;
using Case.Match3.Candy;
using Case.Match3.Spin;
using UnityEngine;
using Zenject;

namespace Case.Match3
{
    public static class Match3Container
    {
        public static void InstallBinding(DiContainer container, GameObject candyPrefab)
        {
            BoardContainer.InstallBinding(container);
            CandyContainer.InstallBinding(container, candyPrefab);
            SpinContainer.InstallBinding(container);
        }
    }
}
