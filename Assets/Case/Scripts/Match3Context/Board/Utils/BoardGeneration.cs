using Case.Collection;
using Case.Match3.Candy;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Case.Match3.Board
{
    internal class BoardGeneration
    {
        static Random random = new Random();
        public static Grid<CandyViewModel> CreateInitialBoard(int row, int column)
        {
            Grid<CandyViewModel> candyViewModels = new(row, column);

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
                    candyViewModels[x, y] = new CandyViewModel(candyTypes[y]);
                }
            }

            return candyViewModels;
        }

        public static Grid<CandyViewModel> CreateRandomBoard(int row, int column)
        {
            Grid<CandyViewModel> candyViewModels = new(row, column);

            for (int y = 0; y < column; y++)
            {
                for (int x = 0; x < row; x++)
                {
                    candyViewModels[x, y] = CandyViewModel.GetRandomViewModel();
                }
            }

            return candyViewModels;
        }

        public static Grid<CandyViewModel> CreateBoard(int row, int column, int minCount)
        {
            Grid<CandyViewModel> candyViewModels = new(row, column);

            List<CandyType> allCandyTypes = new();

            for (int i = 0; i <= (int)CandyType.Type7; i++)
            {
                var candyType = (CandyType)Enum.GetValues(typeof(CandyType)).GetValue(i);
                allCandyTypes.Add(candyType);
            }

            Grid<List<CandyType>> posCandyTypes = new(row, column);

            int count = row * column;

            for (int y = 0; y < column; y++)
            {
                for (int x = 0; x < row; x++)
                {
                    posCandyTypes[x, y] = new List<CandyType>(allCandyTypes);
                }
            }

            for (int y = 0; y < column; y++)
            {
                for (int x = 0; x < row; x++)
                {
                    candyViewModels[x, y] = new(posCandyTypes[x, y][random.Next(posCandyTypes[x, y].Count)]);

                    foreach (var (fX, fY) in FindRadiusIndexes(x, y, row, column, minCount + 1))
                    {
                        if (fX >= candyViewModels.SizeX || fY >= candyViewModels.SizeY || (x == fX && y == fY))
                            continue;

                        if (candyViewModels[x, y].CandyType == candyViewModels[fX, fY].CandyType)
                        {
                            var distance = Math.Abs(fY - y) + Math.Abs(fX - x) - 1;

                            if (distance > minCount)
                                continue;

                            foreach (var (removeFX, removeFY) in FindRadiusIndexes(fX, fY, row, column, minCount))
                            {
                                posCandyTypes[removeFX, removeFY].Remove(candyViewModels[x, y].CandyType);
                            }

                            foreach (var (removeX, removeY) in FindRadiusIndexes(x, y, row, column, minCount))
                            {
                                posCandyTypes[removeX, removeY].Remove(candyViewModels[x, y].CandyType);
                            }
                        }
                    }
                }
            }

            return candyViewModels;
        }

        private static List<(int, int)> FindRadiusIndexes(int currentRow, int currentColumn, int row, int column, int radius)
        {
            List<(int, int)> allRadiusIndexes = new();

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
                        allRadiusIndexes.Add((x, y));
                    }
                }
            }


            return allRadiusIndexes;
        }
    }
}