using Case.Match3.Candy;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using Cysharp.Threading.Tasks;
using GridLayout = Case.Shared.GridLayout.GridLayout;
using Case.Collection;

namespace Case.Match3.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] GridLayout gridLayout;
        [SerializeField] GridLayout secretGridLayout;
        [SerializeField] Transform topPivot;
        [SerializeField] Transform bottomPivot;

        [Inject] private CandyView.Factory candyFactory;

        Grid<CandyView> candyViews;
        Grid<CandyView> secretCandyViews;

        public Grid<CandyView> CandyViews => candyViews;

        public Grid<CandyView> CreateBoardView(int row, int column)
        {
            candyViews = new Grid<CandyView>(row, column);
            secretCandyViews = new Grid<CandyView>(row, 1);

            gridLayout.count = new Vector2Int(row, column);
            secretGridLayout.count = new Vector2Int(row, 1);

            Grid<CandyViewModel> candyViewModels = BoardGeneration.CreateInitialBoard(row, column);

            int count = row * column;
            for (int i = 0; i < count; i++)
            {
                CandyView candy = candyFactory.Create();
                candy.ViewModel = candyViewModels[i];
                candy.transform.SetParent(gridLayout.transform);
                candyViews[i] = candy;
            }

            for (int i = 0; i < row; ++i)
            {
                CandyView candy = candyFactory.Create();
                candy.ViewModel = CandyViewModel.GetRandomViewModel();
                candy.transform.SetParent(secretGridLayout.transform);
                secretCandyViews[i] = candy;
            }

            gridLayout.ForceLayout();
            secretGridLayout.ForceLayout();

            return candyViews;
        }

        public void ForceLayout()
        {
            gridLayout.ForceLayout();
        }

        public void SetCandyInteractable(bool isInteractable)
        {
            foreach (var candyView in candyViews)
            {
                candyView.interactable = isInteractable;
            }
        }

        public void SpinAnimationLoopAllColumn(float speed)
        {
            SpinAnimationLoop(speed, candyViews.Concat(secretCandyViews));
        }

        public void SpinAnimationLoopColumn(float speed, int column)
        {
            SpinAnimationLoop(speed, candyViews.GetColumn(column).Concat(secretCandyViews.GetColumn(column)));
        }

        private void SpinAnimationLoop(float speed, IEnumerable<CandyView> candyViews)
        {
            foreach (var candyView in candyViews)
            {
                candyView.transform.position += speed * Time.deltaTime * Vector3.up;
                if (candyView.transform.position.y > topPivot.position.y)
                {
                    float offset = candyView.transform.position.y - topPivot.position.y;
                    candyView.transform.position = new Vector3(candyView.transform.position.x, bottomPivot.transform.position.y + offset, candyView.transform.position.z);
                    candyView.ViewModel = CandyViewModel.GetRandomViewModel();
                }
            }
        }

        public async UniTask SpinAnimationStopLoopAllColumn(float speed, BoardModel boardModel)
        {
            await SpinAnimationStopLoop(speed, candyViews.Concat(secretCandyViews), boardModel.CandyViewModelGrids.ToList());

            gridLayout.ForceLayout();
            secretGridLayout.ForceLayout();
        }

        public async UniTask SpinAnimationStopLoopColumn(float speed, int column, BoardModel boardModel)
        {
            await SpinAnimationStopLoop(speed,
                candyViews.GetColumn(column).Concat(secretCandyViews.GetColumn(column)),
                boardModel.CandyViewModelGrids.GetColumn(column).ToList());

            gridLayout.ForceLayoutColumn(column);
            secretGridLayout.ForceLayoutColumn(column);
        }

        private async UniTask SpinAnimationStopLoop(float speed, IEnumerable<CandyView> candyViews, List<CandyViewModel> candyViewModels)
        {
            bool isResetBoard = false;
            while (!isResetBoard)
            {
                SpinAnimationLoop(Mathf.Abs(speed), candyViews);

                if (Mathf.Abs(candyViews.First().transform.position.y - gridLayout.transform.position.y) < .05f)
                {
                    isResetBoard = true;
                }

                await UniTask.NextFrame();
            }

            bool isFinish = false;
            while (!isFinish)
            {
                {
                    int i = 0;
                    foreach (var candyView in candyViews)
                    {
                        candyView.transform.position += speed * Time.deltaTime * Vector3.up;
                        if (candyView.transform.position.y > topPivot.position.y)
                        {
                            float offset = candyView.transform.position.y - topPivot.position.y;
                            candyView.transform.position = new Vector3(candyView.transform.position.x, bottomPivot.transform.position.y + offset, candyView.transform.position.z);
                            candyView.ViewModel = i < candyViewModels.Count() ? candyViewModels[i] : CandyViewModel.GetRandomViewModel();
                        }
                        i++;
                    }
                }

                if (Mathf.Abs(candyViews.First().transform.position.y - gridLayout.transform.position.y) < .05f)
                {
                    isFinish = true;
                    int i = 0;
                    foreach (var candyViewModel in candyViewModels)
                    {
                        if (candyViewModel.CandyType != candyViews.ElementAt(i).ViewModel.CandyType)
                        {
                            isFinish = false;
                        }
                        i++;
                    }
                }

                await UniTask.NextFrame();
            }
        }

        public void UpdateView(BoardModel boardModel)
        {
            for (int i = 0; i < candyViews.Count; ++i)
            {
                candyViews[i].ViewModel = boardModel.CandyViewModelGrids[i];
            }
        }
    }
}