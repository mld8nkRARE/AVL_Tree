using System.Collections;
namespace AVL_TreeLib
{
    public class AVL_Tree<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public int Count { get; private set; }
        private IComparer<TKey> _comparer;
        private Node<TKey, TValue> _root;
        public AVL_Tree() : this(null, Comparer<TKey>.Default)
        { }
        public AVL_Tree(IComparer<TKey> comparer) : this(null, comparer)
        { }
        public AVL_Tree(IDictionary<TKey, TValue> dictionary) : this(dictionary, Comparer<TKey>.Default)
        { }
        public AVL_Tree(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
        {
            _comparer = comparer;
            Count = 0;
            _root = null;
            if (dictionary != null && dictionary.Count > 0)
            {
                foreach (var pair in dictionary)
                {
                    Add(pair.Key, pair.Value);
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            _root = Insert(_root, key, value);
        }
        private Node<TKey, TValue> Insert(Node<TKey, TValue> node, TKey key, TValue value)
        {
            if (node == null)
            {
                Count++;
                return new Node<TKey, TValue>(key, value);
            }

            int compare = _comparer.Compare(key, node.Key);

            if (compare < 0)
                node.Left = Insert(node.Left, key, value);
            else if (compare > 0)
                node.Right = Insert(node.Right, key, value);
            else
                throw new ArgumentException("Such key is already added");

            return Balance(node);
        }
        public int Height
        {
            get
            {
                if (_root == null)
                    return 0;

                return _root.Height;
            }
        }


        public bool ContainsKey(TKey key)
        {
            return Find(key) != null;
        }
        private Node<TKey, TValue> RotateRight(Node<TKey, TValue> y)
        {
            var x = y.Left;
            var T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            UpdateHeight(y);
            UpdateHeight(x);

            return x;
        }
        private void UpdateHeight(Node<TKey, TValue> node)
        {
            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        }
        private int GetHeight(Node<TKey, TValue> node)
        {

            return node?.Height ?? 0;
        }
        private int GetBalance(Node<TKey, TValue> node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }
        private Node<TKey, TValue> RotateLeft(Node<TKey, TValue> x)
        {
            var y = x.Right;
            var T2 = y.Left;

            y.Left = x;
            x.Right = T2;


            UpdateHeight(x);
            UpdateHeight(y);

            return y;
        }
        private Node<TKey, TValue> Balance(Node<TKey, TValue> node)
        {
            UpdateHeight(node);

            int balance = GetBalance(node);

            // LL
            if (balance > 1 && GetBalance(node.Left) >= 0)
                return RotateRight(node);

            // LR
            if (balance > 1 && GetBalance(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            // RR
            if (balance < -1 && GetBalance(node.Right) <= 0)
                return RotateLeft(node);

            // RL
            if (balance < -1 && GetBalance(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        public void Remove(TKey key)
        {
            _root = Remove(_root, key);
        }

        private Node<TKey, TValue> Remove(Node<TKey, TValue> node, TKey key)
        {
            if (node == null)
                return null;

            int cmp = _comparer.Compare(key, node.Key);

            if (cmp < 0)
            {
                node.Left = Remove(node.Left, key);
            }
            else if (cmp > 0)
            {
                node.Right = Remove(node.Right, key);
            }
            else
            {
                if (node.Left == null || node.Right == null)
                {
                    Count--;
                    return node.Left ?? node.Right;
                }

                var minNode = GetMin(node.Right);

                node.Key = minNode.Key;
                node.Value = minNode.Value;

                node.Right = Remove(node.Right, minNode.Key);
            }

            return Balance(node);
        }
        private Node<TKey, TValue> GetMin(Node<TKey, TValue> node)
        {
            while (node.Left != null)
                node = node.Left;

            return node;
        }



        public bool IsValidAVL()
        {
            return CheckAVL(_root).isValid;
        }

        private (bool isValid, int height) CheckAVL(Node<TKey, TValue> node)
        {
            if (node == null)
                return (true, 0);

            var left = CheckAVL(node.Left);
            var right = CheckAVL(node.Right);

            bool balanced = Math.Abs(left.height - right.height) <= 1;
            bool correctHeight = node.Height == Math.Max(left.height, right.height) + 1;

            bool isValid = left.isValid &&
                           right.isValid &&
                           balanced &&
                           correctHeight;

            int height = Math.Max(left.height, right.height) + 1;

            return (isValid, height);
        }


        private Node<TKey, TValue> Find(TKey findKey)
        {
            var current = _root;
            while (current != null)
            {
                int result = _comparer.Compare(current.Key, findKey);
                if (result > 0)
                {
                    current = current.Left;
                }
                else if (result < 0)
                {
                    current = current.Right;
                }
                else
                {
                    break;
                }
            }
            return current;
        }



        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException();
                var node = Find(key);
                return node == null ? throw new KeyNotFoundException() : node.Value;
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException();
                var node = Find(key);
                if (node == null)
                    Add(key, value);
                else node.Value = value;
            }
        }
        public void Clear()
        {
            _root = null;
            Count = 0;
        }

        public bool ContainsValue(TValue value)
        {
            var comparer = EqualityComparer<TValue>.Default;
            foreach (var keyValuePair in Traverse())
            {
                if (comparer.Equals(value, keyValuePair.Value))
                    return true;
            }
            return false;
        }
        IEnumerable<KeyValuePair<TKey, TValue>> Traverse(Node<TKey, TValue> node)
        {
            if (node == null)
                yield break;
            foreach (var item in Traverse(node.Left))
            {
                yield return item;
            }
            yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
            foreach (var item in Traverse(node.Right))
            {
                yield return item;
            }
        }
        public IEnumerable<KeyValuePair<TKey, TValue>> Traverse()
        {
            return Traverse(_root);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Traverse().GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Traverse().GetEnumerator();
        }



    }
}
