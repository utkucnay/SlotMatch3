using UnityEngine;
using Zenject;

namespace Case.Match3.Board
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "Installers/BoardConfig")]
    public class BoardConfigSO : ScriptableObjectInstaller<BoardConfigSO>
    {
        public BoardData BoardData;

        public override void InstallBindings()
        {
            Container.Bind<BoardData>().FromInstance(BoardData).AsSingle();
        }
    }
}