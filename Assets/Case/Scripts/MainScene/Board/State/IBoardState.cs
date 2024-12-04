using Cysharp.Threading.Tasks;

namespace Case.MainScene.Board
{
    public interface IBoardState
    {
        UniTask StateEnter();
        UniTask StateExit();
    }
}