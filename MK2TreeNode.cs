using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HausdorfDistanceProject
{
    internal class MK2TreeNode
    {
        public int value;
        public int minx;
        public int maxx;
        public int miny;
        public int maxy;
        public LinkedList<K2TreeNode> childs = new LinkedList<K2TreeNode>();
        public IEnumerator<(int, int)> GetEnumerator()
        {
            if (minx == maxx && value == 1)
                yield return (minx, miny);
            else
            {
                foreach (var child in childs)
                {
                    foreach (var point in child)
                        yield return point;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
