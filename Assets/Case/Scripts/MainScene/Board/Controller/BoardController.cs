using Case.MainScene.Candy;
using Case.MainScene.Game.Signal;
using Case.MainScene.Swipe;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Case.MainScene.Board
{
    public class BoardController : IInitializable, IDisposable
    {
        public event Action OnFirstSwipe;

        [Inject] BoardView boardView;
        [Inject] CandyView.Factory candyFactory;
        [Inject] SignalBus signalBus;

        public BoardModel BoardModel { get; private set; }
        BoardConfig boardConfig = new();

        IBoardState boardState;

        public void Initialize()
        {
            BoardModel = new BoardModel(this, boardConfig.row, boardConfig.column);

            boardState = new NullBoardState();

            boardView.SetCandyInteractable(false);

            BoardModel.OnCanyViewModelChange += BoardModel_OnCanyViewModelChange;
        }

        public void Dispose()
        {
            BoardModel.OnCanyViewModelChange -= BoardModel_OnCanyViewModelChange;
        }

        public List<CandyView> CreateBoardView(int row, int column)
        {
            List<CandyView> candyViews = boardView.CreateBoardView(row, column);

            foreach (var candyView in candyViews)
            {
                candyView.OnSwipeEvent += CandyView_OnSwipeEvent;
                candyView.OnDestroyEvent += CandyView_OnDestroyEvent;
            }

            boardView.SetCandyInteractable(true);

            return candyViews;
        }

        private void CandyView_OnDestroyEvent(CandyView candyView)
        {
            candyView.OnSwipeEvent -= CandyView_OnSwipeEvent;
            candyView.OnDestroyEvent -= CandyView_OnDestroyEvent;
        }

        private void CandyView_OnSwipeEvent(CandyView candyView, SwipeDirection swipeDirection)
        {
            var candyViewModel = candyView.ViewModel;
            var index = BoardModel.CandyViewModels.FindIndex(x => { return x.Id == candyViewModel.Id; });

            var row = index % BoardModel.Row;
            var column = index / BoardModel.Column;

            switch (swipeDirection)
            {
                case SwipeDirection.Up:
                    column--;
                    break;
                case SwipeDirection.Down:
                    column++;
                    break;
                case SwipeDirection.Left:
                    row--;
                    break;
                case SwipeDirection.Right:
                    row++;
                    break;
            }

            if (!((row < BoardModel.Row && row >= 0) && (column < BoardModel.Column && column >= 0)))
            {
                return;
            }

            var targetIndex = column * BoardModel.Row + row;

            SwipeCandy(index, targetIndex).Forget();
        }

        public async UniTaskVoid SwipeCandy(int firstIndex, int secondIndex)
        {
            boardView.SetCandyInteractable(false);

            Vector3 firstCandyPosition = boardView.CandyViews[firstIndex].transform.position;
            Vector3 secondCandyPosition = boardView.CandyViews[secondIndex].transform.position;

            if(!BoardModel.IsSwiped)
            {
                OnFirstSwipe?.Invoke();
            }

            BoardModel.IsSwiped = true;

            await UniTask.WhenAll(
                boardView.CandyViews[firstIndex].transform.DOMove(secondCandyPosition, boardConfig.SwipeCandyDuration).SetEase(Ease.InOutFlash).ToUniTask(),
                boardView.CandyViews[secondIndex].transform.DOMove(firstCandyPosition, boardConfig.SwipeCandyDuration).SetEase(Ease.InOutFlash).ToUniTask()
                );

            var firstCandyViewModel = BoardModel.CandyViewModels[firstIndex];
            var secondCandyViewModel = BoardModel.CandyViewModels[secondIndex];

            boardView.CandyViews[firstIndex].ViewModel = secondCandyViewModel;
            boardView.CandyViews[secondIndex].ViewModel = firstCandyViewModel;

            BoardModel.CandyViewModels[firstIndex] = secondCandyViewModel;
            BoardModel.CandyViewModels[secondIndex] = firstCandyViewModel;

            boardView.ForceLayout();

            if (HasAnyMatch())
            {
                signalBus.Fire<GameEndSignal>();
            }
            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(.01f));
                boardView.SetCandyInteractable(true);
            }
        }

        public bool HasAnyMatch()
        {
            bool hasAnyMatch = false;

            var candyViewModels = BoardModel.CandyViewModels;

            for (int y = 0; y < BoardModel.Column - 2; ++y)
            {
                for (int x = 0; x < BoardModel.Row; x++)
                {
                    int index = y * BoardModel.Row + x;
                    int columnIndex = index + BoardModel.Row * 1;
                    int columnIndex2 = index + BoardModel.Row * 2;

                    if (candyViewModels[index].CandyType == candyViewModels[columnIndex].CandyType &&
                        candyViewModels[index].CandyType == candyViewModels[columnIndex2].CandyType)
                    {
                        hasAnyMatch = true;
                    }
                }
            }

            for (int y = 0; y < BoardModel.Column; ++y)
            {
                for (int x = 0; x < BoardModel.Row - 2; x++)
                {
                    int index = y * BoardModel.Row + x;
                    int rowIndex = index + 1;
                    int rowIndex2 = index + 2;

                    if (candyViewModels[index].CandyType == candyViewModels[rowIndex].CandyType &&
                        candyViewModels[index].CandyType == candyViewModels[rowIndex2].CandyType)
                    {
                        hasAnyMatch = true;
                    }
                }
            }

            return hasAnyMatch;
        }

        public void ShowView(bool isShow)
        {
            boardView.gameObject.SetActive(isShow);
        }

        public async UniTask SetSpinBoardState()
        {
            await boardState.StateExit();
            boardState = new SpinBoardState(boardConfig.SpinSpeed, boardConfig.MinSpeed, boardConfig.SlowAndStopDuration, boardView, BoardModel, boardConfig);
            await boardState.StateEnter();
        }

        public async UniTask SetNullBoardState()
        {
            await boardState.StateExit();
            boardState = new NullBoardState();
            await boardState.StateEnter();
        }

        private void BoardModel_OnCanyViewModelChange()
        {
            boardView.UpdateView(BoardModel);
        }
    }
}
