using Case.MainScene.Candy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Case.MainScene.Board
{
    public class BoardModel
    {
        public event Action OnCanyViewModelChange;

        private List<CandyViewModel> candyViewModels;
        public List<CandyViewModel> CandyViewModels
        {
            get
            {
                return candyViewModels;
            }
            set
            {
                candyViewModels = value;
                OnCanyViewModelChange?.Invoke();
            }
        }

        public int Row { get; private set; }
        public int Column { get; private set; }

        public bool IsSwiped { get; set; }

        public BoardModel(BoardController boardController, int row, int column)
        {
            Row = row;
            Column = column;
            IsSwiped = false;

            List<CandyView> candyViews = boardController.CreateBoardView(row, column);
            candyViewModels = candyViews.Select(x => x.ViewModel).ToList();
        }
    }
}
