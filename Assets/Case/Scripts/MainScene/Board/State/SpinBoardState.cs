using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Case.MainScene.Board
{
    public class SpinBoardState : IBoardState
    {
        public float SpinSpeed { get; private set; }
        public float SlowAndStopDuration { get; private set; }
        public bool IsSpinning { get; private set; }
        public float MinSpeed { get; private set; }

        private readonly BoardView boardView;
        private readonly BoardModel boardModel;
        private readonly BoardConfig boardConfig;

        public SpinBoardState(float spinSpeed, float minSpeed, float slowAndStopDuration, BoardView boardView, BoardModel boardModel, BoardConfig boardConfig)
        {
            SpinSpeed = spinSpeed;
            this.boardView = boardView;
            this.boardModel = boardModel;
            this.SlowAndStopDuration = slowAndStopDuration;
            this.MinSpeed = minSpeed;
            this.boardConfig = boardConfig;
        }

        public async UniTask StateEnter()
        {
            boardModel.CandyViewModels = BoardGeneration.CreateBoard(boardModel.Row, boardModel.Column, boardConfig.MinCount);
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
