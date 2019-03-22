using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utilities.ObservableList {
    public class ObservableList<T> : IList<T> {
        public delegate void ListUpdateHandler(object sender, object updatedValue);
        public event ListUpdateHandler ItemAdded;
        public event ListUpdateHandler ItemRemoved;
        public event EventHandler ListCleared;
        private List<T> m_list;

        public ObservableList() {
             m_list = new List<T>();
        }

        public ObservableList(IEnumerable<T> existingItems) {
            m_list = new List<T>();
            foreach (var item in existingItems) {
                m_list.Add(item);    
            }
        }

        #region IList[T] implementation
        public int IndexOf(T value) {
            return m_list.IndexOf(value);
        }

        public void Insert(int index, T value) {
            m_list.Insert(index, value);
        }

        public void RemoveAt(int index) {
            m_list.RemoveAt(index);
        }

        public T this[int index] {
            get {
                return m_list[index];
            }
            set {
                m_list[index] = value;
            }
        }
        #endregion

        #region IEnumerable implementation
        public IEnumerator<T> GetEnumerator() {
            return m_list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        #endregion

        #region ICollection[T] implementation
        public void Add(T item) {
            ItemAdded?.Invoke(this, item);
            m_list.Add(item);
        }

        public void Clear() {
            m_list.Clear();

            ListCleared?.Invoke(this, EventArgs.Empty);
        }

        public bool Contains(T item) {
            return m_list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            m_list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            ItemRemoved?.Invoke(this, item);

            return m_list.Remove(item);
        }

        public int Count {
            get {
                return m_list.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return false;
            }
        }
        #endregion
    }
}
