using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Case.Match3.Board
{
    public class SpinBoardState : IBoardState
    {
        public float SpinSpeed { get; private set; }
        public float SlowAndStopDuration { get; private set; }
        public float MinSpeed { get; private set; }
        public bool IsSpinning { get; private set; }
        public int MinCount { get; private set; }

        [Inject] private BoardView boardView;
        private BoardModel boardModel;

        public SpinBoardState(BoardData boardData, BoardController boardController)
        {
            SpinSpeed = boardData.SpinSpeed;
            SlowAndStopDuration = boardData.SlowAndStopDuration;
            MinSpeed = boardData.MinSpeed;
            MinCount = boardData.MinCount;
            boardModel = boardController.BoardModel;
        }

        public async UniTask StateEnter()
        {
            boardModel.CandyViewModelGrids = new(boardModel.Row, boardModel.Column, BoardGeneration.CreateBoard(boardModel.Row, boardModel.Column, MinCount));
            boardView.SetCandyInteractable(false);
            SpinAnimation().Forget();
            await UniTask.NextFrame();
        }

        public async UniTask StateExit()
        {
            IsSpinning = false;
            await StopSpinAnimation();
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            boardView.SetCandyInteractable(true);
        }

        private async UniTaskVoid SpinAnimation()
        {
            IsSpinning = true;

            while (IsSpinning)
            {
                boardView.SpinAnimationLoopAllColumn(SpinSpeed);
                await UniTask.NextFrame();
            }
        }

        private async UniTask StopSpinAnimation()
        {
            Task spinStopAnimationColumn = null;
            for (int firstColumn = 0; firstColumn < boardModel.Column; ++firstColumn)
            {
                float speed = SpinSpeed;

                while (true)
                {
                    boardView.SpinAnimationLoopColumn(speed, firstColumn);

                    for (int secondColumn = firstColumn + 1; secondColumn < boardModel.Column; ++secondColumn)
                    {
                        boardView.SpinAnimationLoopColumn(SpinSpeed, secondColumn);
                    }

                    await UniTask.NextFrame();

                    speed -= (SpinSpeed * Time.deltaTime) * SlowAndStopDuration;
                    if (speed < MinSpeed)
                    {
                        if (spinStopAnimationColumn != null && !spinStopAnimationColumn.IsCompleted)
                        {
                            speed = MinSpeed;
                            continue;
                        }
                        spinStopAnimationColumn = boardView.SpinAnimationStopLoopColumn(speed, firstColumn, boardModel).AsTask();
                        break;
                    }
                }
            }
        }
    }
}
