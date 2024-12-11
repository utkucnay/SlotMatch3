using Cysharp.Threading.Tasks;

namespace Case.Match3.Board
{
    public interface IBoardState
    {
        UniTask StateEnter();
        UniTask StateExit();
    }
}