using Case.MainScene.Candy;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using Cysharp.Threading.Tasks;
using GridLayout = Case.Shared.GridLayout.GridLayout;

namespace Case.MainScene.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] GridLayout gridLayout;
        [SerializeField] GridLayout secretGridLayout;
        [SerializeField] Transform topPivot;
        [SerializeField] Transform bottomPivot;

        [Inject] private CandyView.Factory candyFactory;

        List<CandyView> candyViews;
        List<CandyView> secretCandyViews;

        public List<CandyView> CandyViews => candyViews;

        public List<CandyView> CreateBoardView(int row, int column)
        {
            candyViews = new List<CandyView>();
            secretCandyViews = new List<CandyView>();

            gridLayout.count = new Vector2Int(row, column);
            secretGridLayout.count = new Vector2Int(row, 1);

            List<CandyViewModel> candyViewModels = BoardGeneration.CreateInitialBoard(row, column);

            int count = row * column;
            for (int i = 0; i < count; i++)
            {
                CandyView candy = candyFactory.Create();
                candy.ViewModel = candyViewModels[i];
                candy.transform.SetParent(gridLayout.transform);
                candyViews.Add(candy);
            }

            for (int i = 0; i < row; ++i)
            {
                CandyView candy = candyFactory.Create();
                candy.ViewModel = CandyViewModel.GetRandomViewModel();
                candy.transform.SetParent(secretGridLayout.transform);
                secretCandyViews.Add(candy);
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
            SpinAnimationLoop(speed, candyViews.Concat(secretCandyViews).Where((_, i) => { return i % gridLayout.count.x == column; }));
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
            await SpinAnimationStopLoop(speed, candyViews.Concat(secretCandyViews), boardModel.CandyViewModels);

            gridLayout.ForceLayout();
            secretGridLayout.ForceLayout();
        }

        public async UniTask SpinAnimationStopLoopColumn(float speed, int column, BoardModel boardModel)
        {
            await SpinAnimationStopLoop(speed,
                candyViews.Concat(secretCandyViews).Where((_, i) => { return i % gridLayout.count.x == column; }),
                boardModel.CandyViewModels.Where((_, i) => { return i % gridLayout.count.x == column; }).ToList());

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
                candyViews[i].ViewModel = boardModel.CandyViewModels[i];
            }
        }
    }
}