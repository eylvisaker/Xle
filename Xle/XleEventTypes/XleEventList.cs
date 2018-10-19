using System;
using System.Collections.Generic;
using System.Linq;

namespace Xle.XleEventTypes
{
    public class XleEventList : IList<XleEvent>
    {
        List<XleEvent> mList = new List<XleEvent>();

        #region IList<XleEvent> Members

        public int IndexOf(XleEvent item)
        {
            return mList.IndexOf(item);
        }

        public void Insert(int index, XleEvent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            mList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            mList.RemoveAt(index);
        }

        public XleEvent this[int index]
        {
            get
            {
                return mList[index];
            }
            set
            {
                mList[index] = value;
            }
        }

        #endregion

        public void AddRange(IEnumerable<XleEvent> events)
        {
            foreach (var evt in events)
            {
                if (evt == null)
                    throw new ArgumentNullException();

                mList.Add(evt);
            }
        }
        #region ICollection<XleEvent> Members

        public void Add(XleEvent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            mList.Add(item);
        }

        public void Clear()
        {
            mList.Clear();
        }

        public bool Contains(XleEvent item)
        {
            return mList.Contains(item);
        }

        public void CopyTo(XleEvent[] array, int arrayIndex)
        {
            mList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return mList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(XleEvent item)
        {
            return mList.Remove(item);
        }

        #endregion

        #region IEnumerable<XleEvent> Members

        public IEnumerator<XleEvent> GetEnumerator()
        {
            return mList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mList.GetEnumerator();
        }

        #endregion

        public TEventType FindFirst<TEventType>(Predicate<TEventType> condition) where TEventType : XleEvent
        {
            foreach (TEventType evt in this.OfType<TEventType>())
            {
                if (condition(evt) == true)
                    return evt;
            }

            return null;
        }
    }
}
