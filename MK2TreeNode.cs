using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HausdorfDistanceProject
{
    internal class MK2TreeNode:IEnumerable<(int,int)>
    {
        public int value;
        public int minx;
        public int maxx;
        public int miny;
        public int maxy;
        public LinkedList<MK2TreeNode> childs = new LinkedList<MK2TreeNode>();
        public IEnumerator<(int, int)> GetEnumerator()
        {
            if (value == 1)
                for(int i = minx; i <=maxx;i++)
                    for(int j = miny;j <=maxy;j++)
                        yield return (i,j);
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
