using AVL_TreeLib;
namespace AVL_TreeTest
{
    public class AVL_TreeTest
    {
        [Fact]
        public void CountIncreaseAfterAdding()
        {
            var tree = new AVL_Tree<int, int>();
            int n = 10;
            for (int i = 0; i < n; i++)
            {
                tree.Add(i, i);
            }
            Assert.Equal(n, tree.Count);
        }
        [Fact]
        public void ItemsExistAfterAdding()
        {
            var tree = new AVL_Tree<int, int>();
            var a = new[] { 22, 30, 15, 5, 17, 24, 33, 10, 16, 26 };
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                tree.Add(a[i], i);
            }
            Assert.Equal(n, tree.Count);
            Array.Sort(a);
            int j = 0;
            foreach (var pair in tree)
            {
                Assert.Equal(a[j], pair.Key);
                j++;
            }

        }

        [Fact]
        public void ContainsExistingElement()
        {
            var tree = new AVL_Tree<int, int>();
            var arrayInts = new[] { 8, 3, 10, 1, 6, 14, 4, 7, 13 };
            foreach (var number in arrayInts)
            {
                tree.Add(number, 0);
            }

            foreach (var number in arrayInts)
            {
                Assert.True(tree.ContainsKey(number));
            }
        }

        [Fact]
        public void ContainsNotExistingElement()
        {
            var tree = new AVL_Tree<int, int>();
            var a = new[] { 8, 3, 10, 1, 6, 14, 4, 7, 13 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], 0);
            }
            Assert.False(tree.ContainsKey(37));
        }
       
        [Fact]
        public void AddingExistingKeyThrowsException()
        {
            var tree = new AVL_Tree<int, int>();
            int n = 10;
            for (int i = 0; i < n; i++)
            {
                tree.Add(i, i);
            }
            Assert.Throws<ArgumentException>(() => tree.Add(n - 1, n - 1));
        }
        [Fact]
        public void IfKeyNotFoundGetThrowsKeyNotFoundException()
        {
            var tree = new AVL_Tree<int, int>();
            int n = 10;
            for (int i = 0; i < n; i++)
            {
                tree.Add(i, i);
            }
            Assert.Throws<KeyNotFoundException>(() => tree[n + 1]);
        }
        [Fact]
        public void IfKeyNotFoundSetAddNewItem()
        {
            var tree = new AVL_Tree<int, int>();
            int n = 10;
            for (int i = 0; i < n; i++)
            {
                tree.Add(i, i);
            }
            tree[n] = n;
            Assert.True(tree.ContainsKey(n));
            Assert.True(tree.ContainsValue(n));
        }


        [Fact]
        public void TreeIsBalancedAfterSequentialInsert()
        {
            var tree = new AVL_Tree<int, int>();

            for (int i = 0; i < 1000; i++)
                tree.Add(i, i);

            Assert.True(tree.IsValidAVL());
        }
        [Fact]
        public void TreeIsBalancedAfterDeletes()
        {
            var tree = new AVL_Tree<int, int>();

            for (int i = 0; i < 1000; i++)
                tree.Add(i, i);

            for (int i = 0; i < 800; i++)
                tree.Remove(i);

            Assert.True(tree.IsValidAVL());
        }
        [Fact]
        public void CountDecreasesAfterRemove()
        {
            var tree = new AVL_Tree<int, int>();

            for (int i = 0; i < 10; i++)
                tree.Add(i, i);

            tree.Remove(5);

            Assert.Equal(9, tree.Count);
        }
        [Fact]
        public void RemoveNonExistingDoesNotCrash()
        {
            var tree = new AVL_Tree<int, int>();

            tree.Add(1, 1);
            tree.Remove(999);

            Assert.Equal(1, tree.Count);
        }
        [Fact]
        public void HeightIsLogarithmic()
        {
            var tree = new AVL_Tree<int, int>();

            for (int i = 0; i < 1000; i++)
                tree.Add(i, i);

            Assert.True(tree.Height < 20);
        }
    }
}