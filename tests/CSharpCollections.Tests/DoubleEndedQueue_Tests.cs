namespace CSharpCollections.Tests
{
    /// <summary>
    /// Tests for the DoubleEndedQueue
    /// </summary>
    public class DoubleEndedQueue_Tests
    {       
        /// <summary>
        /// Checks that the capacity is set in the constructor
        /// </summary>
        [Test]
        public void ConstructorBody_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>()
            {
                // Act
                0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31
            };
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dequeue.SyncRoot, Is.Not.Null); // The Sync Root should be available for thread-safety
                Assert.That(dequeue.IsReadOnly, Is.False); // The queue can be edited
                Assert.That(dequeue.IsSynchronized, Is.False); // This is not a thread-safe class
                Assert.That(dequeue, Has.Count.EqualTo(32));
            });
            for (int i = 0; i < 32; i++)
            {
                Assert.That(dequeue.Dequeue(), Is.EqualTo(i));
            }
        }

        /// <summary>
        /// Checks that the capacity is set in the constructor
        /// </summary>
        [Test]
        public void Capacity_Test()
        {
            // Arrange
            // Act
            var dequeue = new DoubleEndedQueue<int>(64);
            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(dequeue.TryDequeue(out _), Is.False); // There should be no items
                Assert.That(dequeue.TryDequeueBack(out _), Is.False); // There should be no items
                Assert.That(dequeue, Is.Empty); // There should be no items
                Assert.That(dequeue.Capacity, Is.EqualTo(64)); // The capacity should be equal to the constructor
                Assert.That(dequeue.Peek(), Is.EqualTo(default(int))); // The default value is returned with 0 capacity
                Assert.That(dequeue.PeekBack(), Is.EqualTo(default(int))); // The default value is returned with 0 capacity
            });
        }

        /// <summary>
        /// Checks that the capacity cannot be too low (must be 3 or greater)
        /// </summary>
        [Test]
        public void Capacity_Exception_Test()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ = new DoubleEndedQueue<int>(2);
            });
            Assert.DoesNotThrow(() => {
                _ = new DoubleEndedQueue<int>(3);
            });
        }

        /// <summary>
        /// Checks that the capacity changes based on objects added
        /// </summary>
        [Test]
        public void AdjustCapacity_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>(3);
            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Enqueue(i);
            }

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(dequeue.TryDequeue(out _), Is.True); // There should be no items
                Assert.That(dequeue.TryDequeueBack(out _), Is.True); // There should be no items
                Assert.That(dequeue, Is.Not.Empty); // There should be no items
                Assert.That(dequeue.Capacity, Is.GreaterThan(32)); // The capacity should have additional room
            });
        }

        /// <summary>
        /// Checks that the capacity changes based on objects added
        /// </summary>
        [Test]
        public void AdjustCapacity_ByShifting_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>(3);
            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Enqueue(i);
                if (i% 2 == 0 || i % 3 == 0)
                {
                    _ = dequeue.Dequeue();
                }
            }


            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(dequeue.TryDequeue(out _), Is.True); // There should be no items
                Assert.That(dequeue.TryDequeueBack(out _), Is.True); // There should be no items
                Assert.That(dequeue, Is.Not.Empty); // There should be no items
                Assert.That(dequeue.Capacity, Is.LessThanOrEqualTo(32)); // The capacity should have additional room
            });
        }

        /// <summary>
        /// Checks that trimming excess removes extra elements
        /// </summary>
        [Test]
        public void TrimExcess_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>(64);
            // Act
            for (int i = 0; i < 30; i++)
            {
                dequeue.Enqueue(i);
            }
            dequeue.TrimExcess();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(dequeue.Count, Is.EqualTo(30)); // There should be 30 items
                Assert.That(dequeue.Capacity, Is.EqualTo(32)); // The capacity should be 2 above the count
            });
        }

        /// <summary>
        /// Checks that trimming excess shrinks an empty queue to a capacity of 3
        /// </summary>
        [Test]
        public void TrimExcess_Empty_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>(64);
            // Act
            dequeue.TrimExcess();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(dequeue.Count, Is.EqualTo(0)); // There should be 0 items
                Assert.That(dequeue.Capacity, Is.EqualTo(3)); // The capacity should be 3 above the count
            });
        }

        /// <summary>
        /// Checks that Enqueue adds objects to the front
        /// </summary>
        [Test]
        public void Enqueue_Dequeue_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Enqueue(i);
            }

            // Assert
            for (int i = 0; i < 32; i++)
            {
                Assert.That(dequeue.Dequeue(), Is.EqualTo(i));
            }
        }

        /// <summary>
        /// Checks that Enqueue adds objects in order
        /// </summary>
        [Test]
        public void Enqueue_Index_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Enqueue(i);
            }

            // Assert
            for (int i = 0; i < 32; i++)
            {
                Assert.That(dequeue[i], Is.EqualTo(i));
            }
        }

        /// <summary>
        /// Checks that Enqueue adds objects that can be dequeued from the back
        /// </summary>
        [Test]
        public void Enqueue_DequeueBack_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Enqueue(i);
            }

            // Assert
            for (int i = 31; i >= 0; i--)
            {
                Assert.That(dequeue.DequeueBack(), Is.EqualTo(i));
            }
        }


        /// <summary>
        /// Checks that EnqueueFront adds objects that can be dequeued from the front
        /// </summary>
        [Test]
        public void EnqueueFront_Dequeue_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.EnqueueFront(i);
            }

            // Assert
            for (int i = 31; i >= 0; i--)
            {
                Assert.That(dequeue.Peek(), Is.EqualTo(i));
                Assert.That(dequeue.Dequeue(), Is.EqualTo(i));
            }
        }

        /// <summary>
        /// Checks that setting the -1 index adds objects to the front
        /// </summary>
        [Test]
        public void IndexFront_Dequeue_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue[-1]=i;
            }

            // Assert
            for (int i = 31; i >= 0; i--)
            {
                Assert.That(dequeue.Peek(), Is.EqualTo(i));
                Assert.That(dequeue.Dequeue(), Is.EqualTo(i));
            }
        }



        /// <summary>
        /// Checks that inserting null to the beginning or end does nothing
        /// </summary>
        [Test]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Smell", "CS8625:Cannot convert null literal to non-nullable reference type.", Justification = "Desired behavior to remove elements")]
        public void Index_Null_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<object>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue[i] = i;
            }
            #pragma warning disable CS8625
            dequeue[-1] = null;
            dequeue[dequeue.Count] = null;

            // Make sure the data can be overwritten
            for (int i = 0; i < 32; i++)
            {
                dequeue[i] = 31-i;
            }

            dequeue[-1] = null;
            dequeue[dequeue.Count] = null;
            #pragma warning restore CS8625

            // Assert
            for (int i = 31; i >= 0; i--)
            {
                Assert.That(dequeue.Peek(), Is.EqualTo(i));
                Assert.That(dequeue.Dequeue(), Is.EqualTo(i));
            }
        }

        /// <summary>
        /// Checks that inserting null at a valid index removes it
        /// </summary>
        [Test]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Smell", "CS8625:Cannot convert null literal to non-nullable reference type.", Justification = "Desired behavior to remove elements")]
        public void Index_Null_Removal_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<object>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue[i] = i;
            }

            // Make sure the data can be overwritten
            for (int i = 0; i < 32; i++)
            {
                #pragma warning disable CS8625
                dequeue[0] = null;
                #pragma warning restore CS8625
            }


            // Assert
            Assert.That(dequeue.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Checks that setting the last index adds objects to the front
        /// </summary>
        [Test]
        public void IndexBack_Dequeue_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue[dequeue.Count] = i;
            }

            // Assert
            for (int i = 0; i < 32; i++)
            {
                Assert.That(dequeue.Peek(), Is.EqualTo(i));
                Assert.That(dequeue.Dequeue(), Is.EqualTo(i));
            }
        }

        /// <summary>
        /// Checks that setting the index adds objects where they should be
        /// </summary>
        [Test]
        public void Index_Dequeue_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue[i] = i;
            }

            // Assert
            for (int i = 0; i < 32; i++)
            {
                Assert.That(dequeue.Peek(), Is.EqualTo(i));
                Assert.That(dequeue.Dequeue(), Is.EqualTo(i));
            }
        }

        /// <summary>
        /// Checks that bad indexes through out of range exceptions
        /// </summary>
        [Test]
        public void Index_OutOfRange_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();


            // Assert
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                // Act
                dequeue[77] = 77;
            });
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                // Act
                dequeue[-77] = -77;
            });
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                // Act
                _ = dequeue[77];
            });
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                // Act
                _ = dequeue[-77];
            });
        }

        /// <summary>
        /// Checks that EnqueueFront adds objects that can be dequeued from the back
        /// </summary>
        [Test]
        public void EnqueueFront_DequeueBack_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.EnqueueFront(i);
            }

            // Assert
            for (int i = 0; i < 32; i++)
            {
                Assert.That(dequeue.PeekBack(), Is.EqualTo(i));
                Assert.That(dequeue.DequeueBack(), Is.EqualTo(i));
            }
        }

        /// <summary>
        /// Checks that clear removes all items
        /// </summary>
        [Test]
        public void Clear_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Enqueue(i);
            }
            dequeue.Clear();

            // Assert
            Assert.That(dequeue, Is.Empty);
        }

        /// <summary>
        /// Checks that Contains returns correctly
        /// </summary>
        [Test]
        public void Contains_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>()
            {
                // Act
                0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31
            };

            // Assert
            for (int i = 0; i < 32; i++)
            {
                var contains = dequeue.Contains(i);
                Assert.True(contains);
            }
            for (int i = 32; i < 64; i++)
            {
                var contains = dequeue.Contains(i);
                Assert.False(contains);
            }
        }

        /// <summary>
        /// Checks that Contains returns correctly
        /// </summary>
        [Test]
        public void ContainsEnumerable_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>()
            {
                // Act
                0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31
            };

            // Assert
            for (int i = 0; i < 32; i++)
            {
                Assert.That(dequeue, Does.Contain(i)); // Checks using IEnumerable
            }
        }

        /// <summary>
        /// Checks that IndexOf returns correctly
        /// </summary>
        [Test]
        public void IndexOf_Enqueue_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Enqueue(i);
            }

            // Assert
            for (int i = 1; i < 32; i++)
            {
                Assert.That(dequeue.IndexOf(i), Is.EqualTo(i)); // Should find results
                Assert.That(dequeue.IndexOf(i+64), Is.EqualTo(-1)); // Should not find results
            }
        }


        /// <summary>
        /// Checks that CopyTo works correctly
        /// </summary>
        [Test]
        public void CopyTo_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();
            var target = new int[64];
            var target2 = new int[64];

            // Act
            for (int i = 1; i < 32; i++)
            {
                dequeue.Enqueue(i);
            }
            Assert.Throws<ArgumentNullException>(() => {
                dequeue.CopyTo(null, 32);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                dequeue.CopyTo(target, 64);
            });
            dequeue.CopyTo(target, 32);
            dequeue.CopyTo(target2, 42);

            // Assert
            for (int i = 1; i < 32; i++)
            {
                Assert.That(Array.IndexOf(target,i), Is.EqualTo(i+31));
                if (i <= 22)
                {
                    Assert.That(Array.IndexOf(target2, i), Is.EqualTo(i + 41));
                }
                else
                {
                    Assert.That(Array.IndexOf(target2, i), Is.EqualTo(-1));
                }
            }
        }

        /// <summary>
        /// Checks that ToArray works correctly
        /// </summary>
        [Test]
        public void ToArray_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Enqueue(i);
            }

            var newArray = dequeue.ToArray();

            // Assert
            for (int i = 0; i < 32; i++)
            {
                // The array should be copied exactly
                Assert.That(newArray[i], Is.EqualTo(i));
            }
        }



        /// <summary>
        /// Checks that Insert works correctly
        /// </summary>
        [Test]
        public void Insert_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Insert(i,i);
            }
            for (int i = 0; i < 16; i++)
            {
                dequeue.Insert(16, 64);
            }

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                dequeue.Insert(65, 65); // First check inserting out of bounds throws
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                dequeue.Insert(-1, 65); // Also check negative indexes that are too small
            });

            for (int i = 0; i < 48; i++)
            {
                if (i < 16)
                {
                    // The array should be copied exactly
                    Assert.That(dequeue[i], Is.EqualTo(i));
                }
                else if (i < 32)
                {
                    // The array should be copied exactly
                    Assert.That(dequeue[i], Is.EqualTo(64));
                }
                else
                {
                    // The array should be copied exactly
                    Assert.That(dequeue[i], Is.EqualTo(i-16));
                }
            }
        }

        /// <summary>
        /// Checks that Insert works correctly when inserting null values
        /// </summary>
        [Test]
        public void Insert_Null_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<object>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Insert(i, i);
            }
            for (int i = 0; i < 32; i++)
            {
                #pragma warning disable CS8625
                dequeue.Insert(i, null);
                #pragma warning restore CS8625
            }

            // Assert
            Assert.That(dequeue, Has.Count.EqualTo(32)); // nothing should have been added
        }

        /// <summary>
        /// Checks that Remove gets rid of values
        /// </summary>
        [Test]
        public void Remove_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<object>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Insert(i, i);
            }
            Assert.That(dequeue, Has.Count.EqualTo(32)); // nothing should have been added

            for (int i = 0; i < 32; i++)
            {
                if (i < 16)
                {
                    Assert.True(dequeue.Remove(i));
                }
                else
                {
                    Assert.False(dequeue.Remove(i+16));
                }
            }

            // Assert
            Assert.That(dequeue, Has.Count.EqualTo(16)); // only 16 items were removed
        }

        /// <summary>
        /// Checks that RemoveAt gets rid of values
        /// </summary>
        [Test]
        public void RemoveAt_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<object>();
            Assert.DoesNotThrow(() => {
                // Removing what isn't there raises no exception because it's technically gone already
                dequeue.RemoveAt(0); 
                dequeue.RemoveAt(55); 
            });

            // Act
            for (int i = 31; i >=0; i--)
            {
                dequeue.Insert(0, i);
            }
            Assert.That(dequeue, Has.Count.EqualTo(32)); // nothing should have been added

            for (int i = 0; i < 4; i++) // removes 8
            {
                dequeue.RemoveAt(0); // Try removing from the front
                dequeue.RemoveAt(dequeue.Count - 1); // Try removing from the back
            }
            var count = dequeue.Count;
            for (int i = 0; i < 15; i++)
            {
                dequeue.RemoveAt(-1); // Does nothing
                dequeue.RemoveAt(i); // Should remove every other item
                var newCount = dequeue.Count;
                if (i < 12)
                {
                    Assert.That(newCount, Is.EqualTo(count - 1));
                }
                else
                {
                    Assert.That(newCount, Is.EqualTo(count));
                }
                count = newCount;
            }

            // Assert
            Assert.That(dequeue, Has.Count.EqualTo(12)); // only 12 items remain
            for (int i = 0; i < 12; i++)
            {
                Assert.That(dequeue[i], Is.EqualTo(5+2*i)); // The first 5 (0-4) were removed, then every other one up to the last 4
            }
        }

        /// <summary>
        /// Checks that the enumerator works
        /// </summary>
        [Test]
        public void Enumerator_Test()
        {
            // Arrange
            var dequeue = new DoubleEndedQueue<int>();

            // Act
            for (int i = 0; i < 32; i++)
            {
                dequeue.Enqueue(i);
            }
            Assert.That(dequeue, Has.Count.EqualTo(32)); // nothing should have been added
            var enumerator = dequeue.GetEnumerator();
            var count = 0;

            // Assert
            while (enumerator.MoveNext())
            {
                Assert.That(enumerator.Current, Is.EqualTo(count));
                count++;
            }
        }
    }
}