using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HausdorfDistanceProject
{
    internal class K2TreeNode
    {
        public int value;
        public int minx;
        public int maxx;
        public int miny;
        public int maxy;
        public LinkedList<K2TreeNode> childs = new LinkedList<K2TreeNode>();
    }
}
