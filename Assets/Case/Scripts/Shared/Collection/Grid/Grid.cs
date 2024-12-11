using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Case.Collection
{
    public class Grid<T> : IEnumerable<T>
    {
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public int Count => SizeX * SizeY;
        T[,] data;

        public Grid(int x, int y)
        {
            SizeX = x;
            SizeY = y;

            data = new T[x,y];    
        }
        
        public Grid(int sizeX, int sizeY, IEnumerable<T> values)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            data = new T[sizeX, sizeY];    
            SetData(values);
        }

        public T GetValue(int x, int y)
        {
            return data[x, y];
        }

        public T GetValue(int i) 
        {
            var (x, y) = IndexTo(i);
            return GetValue(x, y);
        }

        public void SetValue(int x, int y, T value)
        {
            data[x, y] = value;
        }

        public void SetValue(int i, T value) 
        {
            var (x, y) = IndexTo(i);
            SetValue(x, y, value);
        }

        public (int, int) IndexTo(int index)
        {
            return (index % SizeX, index / SizeX);
        }

        public void SetData(IEnumerable<T> values)
        {
            int i = 0;
            foreach (T value in values)
            {
                var (x, y) = IndexTo(i);
                data[x, y] = value;
                i++;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    yield return GetValue(x, y);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<T> GetRow(int index)
        {
            for (int x = 0; x < SizeX; x++)
            {
                yield return GetValue(x, index);
            }
        }

        public IEnumerable<T> GetColumn(int index)
        {
            for (int y = 0; y < SizeY; y++)
            {
                yield return GetValue(index, y);
            }
        }

        public void Clear()
        {
            data = new T[SizeX, SizeY];
        }

        public bool Contains(T item)
        {
            foreach (var x in this)
            {
                if (x.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public int FindIndex(Func<T, bool> prediction)
        {
            int i = 0;
            foreach (T value in this)
            {
                bool isSucces = prediction.Invoke(value);
                if (isSucces)
                {
                    return i; 
                }
                i++;
            }
            return -1;
        }

        public T this[int index]
        {
            get => GetValue(index);
            set => SetValue(index, value);
        }

        public T this[int x, int y]
        {
            get => GetValue(x, y);
            set => SetValue(x, y, value);
        }
    }
}
