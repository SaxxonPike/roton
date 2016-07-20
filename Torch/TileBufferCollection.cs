using System.Collections.Generic;
using Roton.Core;

namespace Torch
{
    public class TileBufferCollection : IList<ITile>
    {
        public TileBufferCollection()
        {
            InnerList = new List<ITile>();
        }

        private List<ITile> InnerList { get; }

        public int IndexOf(ITile item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, ITile item)
        {
            InnerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            InnerList.RemoveAt(index);
        }

        public ITile this[int index]
        {
            get { return InnerList[index]; }
            set { InnerList[index] = value; }
        }

        public void Add(ITile item)
        {
            InnerList.Add(item);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public bool Contains(ITile item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(ITile[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public int Count => InnerList.Count;

        public bool IsReadOnly => false;

        public bool Remove(ITile item)
        {
            return InnerList.Remove(item);
        }

        public IEnumerator<ITile> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}