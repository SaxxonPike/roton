using Roton;
using System.Collections.Generic;

namespace Torch
{
    public class TileBufferCollection : IList<Tile>
    {
        public TileBufferCollection()
        {
            InnerList = new List<Tile>();
        }

        private List<Tile> InnerList { get; }

        public int IndexOf(Tile item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, Tile item)
        {
            InnerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            InnerList.RemoveAt(index);
        }

        public Tile this[int index]
        {
            get { return InnerList[index]; }
            set { InnerList[index] = value; }
        }

        public void Add(Tile item)
        {
            InnerList.Add(item);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public bool Contains(Tile item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(Tile[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public int Count => InnerList.Count;

        public bool IsReadOnly => false;

        public bool Remove(Tile item)
        {
            return InnerList.Remove(item);
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}