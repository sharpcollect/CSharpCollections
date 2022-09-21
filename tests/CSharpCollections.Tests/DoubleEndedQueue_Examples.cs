using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCollections.Tests
{
    /// <summary>
    /// Example usages of DoubleEndedQueue
    /// </summary>
    public class DoubleEndedQueue_Examples
    {
        // A generic node class
        public class Node<T>
        {
            // The value
            public T Value { get; set; }

            // The left child
            public Node<T> Left { get; set; }

            // The right child
            public Node<T> Right { get; set; }

            // Gets the next nodes
            public IEnumerable<Node<T>> GetNext()
            {
                if (Left != null) yield return Left;
                if (Right != null) yield return Right;
            }
        }

        // A typesafe node
        public class DN : Node<double> { }

        /// <summary>
        /// Uses a Double Ended Queue for Breadth-First-Search
        /// </summary>
        [Test]
        public void BreadthFirstSearch_Example()
        {
            // Arrange
            var tree = new DN
            {
                Value = 0,
                Left = new DN
                {
                    Value = 1,
                    Left = new DN { },
                    Right = new DN
                    {
                        Value = 5,
                        Left = new DN { },
                        Right = new DN { },
                    },
                },
                Right = new DN
                {
                    Value = 1,
                    Left = new DN
                    {
                        Value = 2,
                        Left = new DN { },
                        Right = new DN
                        {
                            Value = 3,
                            Left = new DN { },
                            Right = new DN
                            {
                                Value = 4,
                                Left = new DN { },
                                Right = new DN
                                {
                                    Value = 5,
                                    Left = new DN { },
                                    Right = new DN { },
                                },
                            },
                        },
                    },
                    Right = new DN { },
                },
            };

            // Act
            int FindDepthUsingBFS(Func<DN, bool> exitCondition, DN head)
            {
                var queue = new DoubleEndedQueue<(DN, int)>() { (head, 0) };
                var seen = new HashSet<DN>();
                while (queue.TryDequeue(out (DN node, int depth) current))
                {
                    if (exitCondition(current.node)) { return current.depth; }
                    foreach (DN next in current.node.GetNext()
                        .Where(obj => !seen.Contains(obj)))
                    {
                        seen.Add(next);
                        queue.Enqueue((next, current.depth + 1));
                    }
                }
                return -1;
            }

            var depth = FindDepthUsingBFS((obj) => obj.Value == 5, tree);

            // Assert
            Assert.That(depth, Is.EqualTo(2)); // The value of 5 is found where it is only 2 deep, not 5
        }
    }
}
