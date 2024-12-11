using Case.Match3.Candy;
using Case.MainScene.Game.Signal;
using Case.MainScene.Swipe;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Case.Collection;

namespace Case.Match3.Board
{
    public class BoardController : IInitializable, IDisposable
    {
        public event Action OnFirstSwipe;

        [Inject] BoardView boardView;
        [Inject] SignalBus signalBus;

        public BoardModel BoardModel { get; private set; }
        [Inject] BoardData boardData;

        IBoardState boardState;
        [Inject] BoardStateFactory boardStateFactory;

        public void Initialize()
        {
            BoardModel = new BoardModel(this, boardData.Row, boardData.Column);
            boardState = new IdleBoardState();
            boardView.SetCandyInteractable(false);
            BoardModel.OnCanyViewModelChange += BoardModel_OnCanyViewModelChange;
        }

        public void Dispose()
        {
            BoardModel.OnCanyViewModelChange -= BoardModel_OnCanyViewModelChange;
        }

        public Grid<CandyView> CreateBoardView(int row, int column)
        {
            Grid<CandyView> candyViews = boardView.CreateBoardView(row, column);

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
            var index = BoardModel.CandyViewModelGrids.FindIndex(x => { return x.Id == candyViewModel.Id; });

            var (row, column) = BoardModel.CandyViewModelGrids.IndexTo(index);

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

            if (!(row < BoardModel.Row && row >= 0 && column < BoardModel.Column && column >= 0))
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
                boardView.CandyViews[firstIndex].transform.DOMove(secondCandyPosition, boardData.SwipeCandyDuration).SetEase(Ease.InOutFlash).ToUniTask(),
                boardView.CandyViews[secondIndex].transform.DOMove(firstCandyPosition, boardData.SwipeCandyDuration).SetEase(Ease.InOutFlash).ToUniTask()
                );

            var firstCandyViewModel = BoardModel.CandyViewModelGrids[firstIndex];
            var secondCandyViewModel = BoardModel.CandyViewModelGrids[secondIndex];

            boardView.CandyViews[firstIndex].ViewModel = secondCandyViewModel;
            boardView.CandyViews[secondIndex].ViewModel = firstCandyViewModel;

            BoardModel.CandyViewModelGrids[firstIndex] = secondCandyViewModel;
            BoardModel.CandyViewModelGrids[secondIndex] = firstCandyViewModel;

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

            var candyViewModels = BoardModel.CandyViewModelGrids;

            for (int y = 0; y < BoardModel.Column - 2; ++y)
            {
                for (int x = 0; x < BoardModel.Row; x++)
                {
                    if (candyViewModels[x, y].CandyType == candyViewModels[x, y + 1].CandyType &&
                        candyViewModels[x, y].CandyType == candyViewModels[x, y + 2].CandyType)
                    {
                        hasAnyMatch = true;
                    }
                }
            }

            for (int y = 0; y < BoardModel.Column; ++y)
            {
                for (int x = 0; x < BoardModel.Row - 2; x++)
                {
                    if (candyViewModels[x, y].CandyType == candyViewModels[x + 1, y].CandyType &&
                        candyViewModels[x, y].CandyType == candyViewModels[x + 2, y].CandyType)
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
            boardState = boardStateFactory.Create(BoardState.Spin);
            await boardState.StateEnter();
        }

        public async UniTask SetNullBoardState()
        {
            await boardState.StateExit();
            boardState = boardStateFactory.Create(BoardState.Idle);
            await boardState.StateEnter();
        }

        private void BoardModel_OnCanyViewModelChange()
        {
            boardView.UpdateView(BoardModel);
        }
    }
}
