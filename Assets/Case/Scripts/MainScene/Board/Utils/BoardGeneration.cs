using Case.MainScene.Candy;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Case.MainScene.Board
{
    internal class BoardGeneration
    {
        static Random random = new Random();
        public static List<CandyViewModel> CreateInitialBoard(int row, int column)
        {
            List<CandyViewModel> candyViewModels = new();

            CandyType[] candyTypes = new CandyType[column];
            var candyTypeValues = Enum.GetNames(typeof(CandyType)).ToList();

            for (int i = 0; i < column; ++i)
            {
                int index = random.Next(candyTypeValues.Count);
                var candyType = (CandyType)Enum.Parse(typeof(CandyType), candyTypeValues[index]);
                candyTypeValues.RemoveAt(index);
                candyTypes[i] = candyType;
            }

            for (int y = 0; y < column; y++)
            {
                for (int x = 0; x < row; x++)
                {
                    candyViewModels.Add(new CandyViewModel(candyTypes[y]));
                }
            }

            return candyViewModels;
        }

        public static List<CandyViewModel> CreateRandomBoard(int row, int column)
        {
            List<CandyViewModel> candyViewModels = new();

            int count = row * column;
            for (int i = 0; i < count; ++i)
            {
                candyViewModels.Add(CandyViewModel.GetRandomViewModel());
            }

            return candyViewModels;
        }

        public static List<CandyViewModel> CreateBoard(int row, int column, int minCount)
        {
            List<CandyViewModel> candyViewModels = new();

            List<CandyType> allCandyTypes = new();

            for (int i = 0; i <= (int)CandyType.Type7; i++)
            {
                var candyType = (CandyType)Enum.GetValues(typeof(CandyType)).GetValue(i);
                allCandyTypes.Add(candyType);
            }

            List<List<CandyType>> posCandyTypes = new();

            int count = row * column;

            for (int i = 0; i < count; i++)
            {
                posCandyTypes.Add(new(allCandyTypes));
            }

            for (int i = 0; i < count; i++)
            {
                candyViewModels.Add(new(posCandyTypes[i][random.Next(posCandyTypes[i].Count)]));

                foreach (int index in FindRadiusIndexes(i, row, column, minCount + 1))
                {
                    if (index >= candyViewModels.Count || index == i)
                        continue;

                    if (candyViewModels[i].CandyType == candyViewModels[index].CandyType)
                    {
                        var firstIndexRow = i % row;
                        var firstIndexColumn = i / row;

                        var secondIndexRow = i % row;
                        var secondIndexColumn = i / row;

                        var distance = Math.Abs(secondIndexColumn - firstIndexColumn) + Math.Abs(secondIndexRow - firstIndexRow) - 1;

                        if (distance > minCount)
                            continue;

                        foreach (int removeItemIndex in FindRadiusIndexes(index, row, column, minCount))
                        {
                            posCandyTypes[removeItemIndex].Remove(candyViewModels[i].CandyType);
                        }

                        foreach (int removeItemIndex in FindRadiusIndexes(i, row, column, minCount))
                        {
                            posCandyTypes[removeItemIndex].Remove(candyViewModels[i].CandyType);
                        }
                    }
                }
            }

            return candyViewModels;
        }

        private static List<int> FindRadiusIndexes(int index, int row, int column, int radius)
        {
            List<int> allRadiusIndexes = new();

            int currentRow = index % row;
            int currentColumn = index / row;

            currentRow -= radius;
            currentColumn -= radius;

            int count = radius + radius + 1;

            for (int y = currentColumn; y < currentColumn + count; y++)
            {
                for (int x = currentRow; x < currentRow + count; x++)
                {
                    if (!((x < row && x >= 0) && (y < column && y >= 0)))
                    {
                        continue;
                    }

                    float euclideanDistance = MathF.Sqrt(MathF.Pow(x - (currentRow + radius), 2) + MathF.Pow(y - (currentColumn + radius), 2));

                    if (euclideanDistance <= radius)
                    {
                        allRadiusIndexes.Add(y * row + x);
                    }
                }
            }


            return allRadiusIndexes;
        }
    }
}