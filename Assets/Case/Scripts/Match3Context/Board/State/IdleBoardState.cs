using Cysharp.Threading.Tasks;

namespace Case.Match3.Board
{
    public class IdleBoardState : IBoardState
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
