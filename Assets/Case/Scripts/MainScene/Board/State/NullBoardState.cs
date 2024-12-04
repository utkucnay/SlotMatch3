using Cysharp.Threading.Tasks;

namespace Case.MainScene.Board
{
    public class NullBoardState : IBoardState
    {
        public async UniTask StateEnter()
        {
            await UniTask.NextFrame();
        }

        public async UniTask StateExit()
        {
            await UniTask.NextFrame();
        }
    }
}
