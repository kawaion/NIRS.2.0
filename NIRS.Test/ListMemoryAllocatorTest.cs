using System;
using Xunit;
using NIRS.Memory_allocator;
using System.Collections.Generic;

namespace NIRS.Test
{
    public class ListMemoryAllocatorTest
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            List<string> list = new List<string>();
            bool isExist;

            // Act
            int i = 3;
            list = ListMemoryAllocator.AllocateUpTo(list, i);

            try
            {
                var a=list[i];
                isExist = true;       
            }
            catch
            {
                isExist = false;
            }

            // Assert
            Assert.True(isExist);
        }
    }
}
