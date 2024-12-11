using Case.Collection;
using Case.Match3.Candy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Case.Match3.Board
{
    public class BoardModel
    {
        public event Action OnCanyViewModelChange;

        private Grid<CandyViewModel> candyViewModelGrids;
        public Grid<CandyViewModel> CandyViewModelGrids
        {
            get
            {
                return candyViewModelGrids;
            }
            set
            {
                candyViewModelGrids = value;
                OnCanyViewModelChange?.Invoke();
            }
        }

        public int Row { get => candyViewModelGrids.SizeX; }
        public int Column { get => candyViewModelGrids.SizeY; }

        public bool IsSwiped { get; set; }

        public BoardModel(BoardController boardController, int row, int column)
        {
            IsSwiped = false;
            
            Grid<CandyView> candyViews = boardController.CreateBoardView(row, column);
            candyViewModelGrids = new(row, column, candyViews.Select(x => x.ViewModel));
        }
    }
}
