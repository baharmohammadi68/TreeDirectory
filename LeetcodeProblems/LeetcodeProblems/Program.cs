using System;
using System.Collections.Generic;

namespace LeetcodeProblems
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CheckValidString("(*))");
        }
        //Given a string containing only three types of characters: '(', ')' and '*', write a function to check whether this string is valid.We define the validity of a string by these rules:
        //Any left parenthesis '(' must have a corresponding right parenthesis ')'.
        //Any right parenthesis ')' must have a corresponding left parenthesis '('.
        //Left parenthesis '(' must go before the corresponding right parenthesis ')'.
        //'*' could be treated as a single right parenthesis ')' or a single left parenthesis '(' or an empty string.
        //An empty string is also valid.
        public static bool CheckValidString(string s)
        {
            int length = s.Length - 1;
            int openCount = 0, closedCount = 0;
            for (int i = 0; i <= length; i++)
            {
                if (s[i] == '*' || s[i] == '(') openCount++;
                else openCount--;
                if (s[length - i] == '*' || s[length - i] == ')') closedCount++;
                else closedCount--;
                if (openCount < 0 || closedCount < 0) return false;
            }
            return true;
        }

        //Given a 2d grid map of '1's(land) and '0's(water), count the number of islands.
        //An island is surrounded by water and is formed by connecting adjacent lands horizontally or vertically.
        //You may assume all four edges of the grid are all surrounded by water.
        public int NumIslands(char[][] grid)
        {
            int numberOfIslands = default;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == '1')
                    {
                        numberOfIslands += Dfs(grid, i, j);

                    }
                }
            }
            return numberOfIslands;

        }

        private int Dfs(char[][] grid, int i, int j)
        {
            if (i < 0 || i >= grid.Length || j < 0 || j >= grid[i].Length || grid[i][j] == '0')
            {
                return 0;
            }
            grid[i][j] = '0'; //visited
            Dfs(grid, i + 1, j);
            Dfs(grid, i, j + 1);
            Dfs(grid, i - 1, j);
            Dfs(grid, i, j - 1);

            return 1; ;
        }

        //Given a m x n grid filled with non-negative numbers, find a path 
        //from top left to bottom right which minimizes the sum of all numbers along its path.
        //Note: You can only move either down or right at any point in time.
        public int MinPathSum(int[][] grid)
        {
            if (grid == null || grid.Length == 0)
            {
                return 0;
            }
            int[][] minArr = new int[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    minArr[i][j] = grid[i][j];
                    if (i > 0 && j > 0)
                    {
                        minArr[i][j] += Math.Min(grid[i - 1][j], grid[i][j - 1]);
                    }
                    else if (i > 0)
                    {
                        minArr[i][j] += grid[i - 1][j];
                    }
                    else if (j > 0)
                    {
                        minArr[i][j] += grid[i][j - 1];
                    }
                }
            }
            return minArr[minArr.Length - 1][minArr[0].Length - 1];
        }

        //Suppose an array sorted in ascending order is rotated at some pivot unknown to you beforehand.

        //(i.e., [0,1,2,4,5,6,7] might become [4,5,6,7,0,1,2]).

        //You are given a target value to search.If found in the array return its index, otherwise return -1.

        //You may assume no duplicate exists in the array.

        //Your algorithm's runtime complexity must be in the order of O(log n).
        public int Search(int[] nums, int target)
        {
            if (nums == null || nums.Length == 0)
                return -1;
            int start = 0, end = nums.Length - 1;
            while (start <= end)
            {
                int mid = start + (end - start) / 2;
                if (target == nums[mid])
                    return mid;
                //check which part is sorted
                if (nums[mid] >= nums[start])
                {
                    //left part is sorted
                    //check if key  is present
                    if (target >= nums[start] && target < nums[mid])
                        end = mid - 1;
                    else
                        start = mid + 1;
                }
                else
                {
                    //right part is sorted
                    //check if key is present
                    if (target > nums[mid] && target <= nums[end])
                        start = mid + 1;
                    else
                        end = mid - 1;
                }
            }
            return -1;

        }

        // Construct Binary Search Tree from Preorder Traversal
        //Return the root node of a binary search tree that matches the given preorder traversal.

        //(Recall that a binary search tree is a binary tree where for every node, 
        //any descendant of node.left has a value < node.val, 
        //and any descendant of node.right has a value > node.val.
        //Also recall that a preorder traversal displays the value of the node first,
        //then traverses node.left, then traverses node.right.)
        //int i = 0;
        //public TreeNode bstFromPreorder(int[] arr)
        //{
        //    return helper(arr, Integer.MAX_VALUE);
        //}

        //public TreeNode helper(int[] arr, int bound)
        //{
        //    if (i == arr.length || arr[i] > bound) return null;
        //    TreeNode root = new TreeNode(arr[i++]);
        //    root.left = helper(arr, root.val);
        //    root.right = helper(arr, bound);
        //    return root;
        //}

        /*
            Design and implement a data structure for Least Recently Used (LRU) cache. 
            It should support the following operations: get and put. 
            get(key) - Get the value (will always be positive) of the key if the key exists in the cache,
            otherwise return -1.
            put(key, value) - Set or insert the value if the key is not already present. 
            When the cache reached its capacity, 
            it should invalidate the least recently used item before inserting a new item. 
            The cache is initialized with a positive capacity.
            */
        public class LRUCache
        {

            int capacity;
            Dictionary<int, LinkedListNode<Pair>> d = new Dictionary<int, LinkedListNode<Pair>>();
            LinkedList<Pair> list = new LinkedList<Pair>();

            public class Pair
            {    // create a helper pair class, that holds a key and a value
                public int Key, Val;

                public Pair(int Key, int Val)
                {
                    this.Key = Key;
                    this.Val = Val;
                }
            }

            public LRUCache(int capacity)
            {
                this.capacity = capacity;
            }

            public int Get(int key)
            {
                if (d.ContainsKey(key))
                {
                    list.Remove(d[key]);
                    list.AddFirst(d[key]);
                }
                return d.ContainsKey(key) ? d[key].Value.Val : -1;

            }

            public void Put(int key, int value)
            {
                LinkedListNode<Pair> new_node = new LinkedListNode<Pair>(new Pair(key, value));

                if (!d.ContainsKey(key))
                {
                    if (list.Count >= capacity)
                    {
                        d.Remove(list.Last.Value.Key);
                        list.RemoveLast();
                    }
                    d.Add(key, new_node);
                    list.AddFirst(new_node);
                }
                else
                {
                    list.Remove(d[key]);
                    list.AddFirst(new_node);
                    d.Remove(key);
                    d.Add(key, new_node);
                }
            }
        }
        /* You have a queue of integers, you need to retrieve the first unique integer in the queue.
         * Implement the FirstUnique class:
         * FirstUnique(int[] nums) Initializes the object with the numbers in the queue.
         * int showFirstUnique() returns the value of the first unique integer of the queue,
         * and returns -1 if there is no such integer.
         * void add(int value) insert value to the queue.*/
        public class FirstUnique
        {
            Dictionary<int, int> numsByOccurences = new Dictionary<int, int>();
            Queue<int> queue = new Queue<int>();
            public FirstUnique(int[] nums)
            {
                for (int i = 0; i < nums.Length; i++)
                {
                    if (numsByOccurences.ContainsKey(nums[i]))
                    {
                        numsByOccurences[nums[i]]++;
                    }
                    else
                    {
                        numsByOccurences.Add(nums[i], 1);
                    }
                    queue.Enqueue(nums[i]);
                }
            }

            public int ShowFirstUnique()
            {
                while (queue.Count > 0 && numsByOccurences[queue.Peek()] > 1)
                {
                    queue.Dequeue();
                }
                if (queue.Count == 0)
                {
                    return -1;
                }
                return queue.Dequeue();
            }

            public void Add(int value)
            {
                if (numsByOccurences.ContainsKey(value))
                {
                    numsByOccurences[value]++;
                }
                else
                {
                    numsByOccurences.Add(value, 1);
                    queue.Enqueue(value);
                }
            }
        }

        /**
         * Your FirstUnique object will be instantiated and called as such:
         * FirstUnique obj = new FirstUnique(nums);
         * int param_1 = obj.ShowFirstUnique();
         * obj.Add(value);
         */

        /*  Binary Tree Maximum Path Sum
         *  Solution - Given a non-empty binary tree, find the maximum path sum.
          For this problem, a path is defined as any sequence of nodes 
          from some starting node to any node in the tree along the parent-child connections. 
          The path must contain at least one node and does not need to go through the root.
         */

        // Definition for a binary tree node.
        public class TreeNode
        {
            public int val;
            public TreeNode left;
            public TreeNode right;
            public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
            {
                this.val = val;
                this.left = left;
                this.right = right;
            }
        }

        public class Solution
        {
            int maxPath = 0;

            public int MaxPathSum(TreeNode root)
            {
                MaxGain(root);
                return maxPath;

            }

            int MaxGain(TreeNode root)
            {
                if(root == null)
                {
                    return 0;
                }
                int maxLeftGain = Math.Max(MaxGain(root.left), 0);
                int maxRightGain = Math.Max(MaxGain(root.right), 0);
                int nodeVal = root.val + maxLeftGain + maxRightGain;
                maxPath = Math.Max(maxPath, nodeVal);
                return root.val + Math.Max(maxLeftGain, maxRightGain);
            }
        }
    }
}
