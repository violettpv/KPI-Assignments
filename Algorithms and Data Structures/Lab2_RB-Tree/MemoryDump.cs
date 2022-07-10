using System.Collections.Generic;

namespace Lab2_RB_Tree
{
    class MemoryDump // the list of data for the tree, here we store the data in the order in which it was added.
    {
        public List<RedBlackTree.Data> Data { get; set; }
        public MemoryDump()
        {
            this.Data = new List<RedBlackTree.Data>();
        }    
    }

}
