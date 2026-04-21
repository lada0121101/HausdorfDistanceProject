using System.Drawing;
using System.Collections;

namespace HausdorfDistanceProject
{
    internal class Convertor
    {
        public static K2TreeMatrix ConvertPointsToK2Matrix(HashSet<Point> points)
        {
            int val = 1;
            while(val < points.Count)
            {
                val *= 2;
            }
            var result = new  K2TreeMatrix();
            result.bitMatrix = new BitArrayExtension(new BitArray(val * val), val);
            foreach(var point in points)
            {
                result.bitMatrix[point.X, point.Y] = true ;
            }
            return result;
        }

        public static K2TreeNode ConvertK2MatrixToK2Tree(K2TreeMatrix K2Matrix)
        {
            Stack<K2TreeNode> stack = new Stack<K2TreeNode>();
            var tree = new K2TreeNode();
            tree.minx = 0;
            tree.miny = 0;
            tree.maxx = K2Matrix.bitMatrix.Length - 1;
            tree.maxy = K2Matrix.bitMatrix.Length - 1;
            stack.Push(tree);
            while(stack.Count != 0)
            {
                var node = stack.Pop();
                if(!isFullZero(K2Matrix, node.minx, node.maxx, node.miny, node.maxy))
                { 
                    node.value = 1;
                    if(node.maxx> node.minx)
                    {
                        var half = (node.maxx  - node.minx + 1) / 2;
                        var child1 = new K2TreeNode();
                        child1.minx = node.minx;
                        child1.maxx = node.minx + half - 1;
                        child1.miny = node.miny;
                        child1.maxy = node.miny + half - 1;
                        var child2 = new K2TreeNode();
                        child2.minx = node.maxx - half + 1; ;
                        child2.maxx = node.maxx;
                        child2.miny = node.miny;
                        child2.maxy = node.miny + half - 1;
                        var child3 = new K2TreeNode();
                        child3.minx = node.minx;
                        child3.maxx = node.minx + half - 1;
                        child3.miny = node.maxy - half + 1;
                        child3.maxy = node.maxy;
                        var child4 = new K2TreeNode();
                        child4.minx = node.maxx - half + 1;
                        child4.maxx = node.maxx;
                        child4.miny = node.maxy - half + 1;
                        child4.maxy = node.maxy;
                        node.childs.AddLast(child1);
                        node.childs.AddLast(child2);
                        node.childs.AddLast(child3);
                        node.childs.AddLast(child4);
                        stack.Push(child1);
                        stack.Push(child2);
                        stack.Push(child3);
                        stack.Push(child4);
                    }
                }
            }
            return tree;
        }

        public static MK2TreeNode ConvertK2MatrixToMK2Tree(K2TreeMatrix K2Matrix)
        {
            Stack<MK2TreeNode> stack = new Stack<MK2TreeNode>();
            var tree = new MK2TreeNode();
            tree.minx = 0;
            tree.miny = 0;
            tree.maxx = K2Matrix.bitMatrix.Length - 1;
            tree.maxy = K2Matrix.bitMatrix.Length - 1;
            stack.Push(tree);
            while (stack.Count != 0)
            {
                var node = stack.Pop();
                if (isFullZeroOROne(K2Matrix, node.minx, node.maxx, node.miny, node.maxy) == (false, false))
                {
                    node.value = 1;
                    var half = (node.maxx - node.minx + 1) / 2;
                    var child1 = new MK2TreeNode();
                    child1.minx = node.minx;
                    child1.maxx = node.minx + half - 1;
                    child1.miny = node.miny;
                    child1.maxy = node.miny + half - 1;
                    var child2 = new MK2TreeNode();
                    child2.minx = node.maxx - half + 1; ;
                    child2.maxx = node.maxx;
                    child2.miny = node.miny;
                    child2.maxy = node.miny + half - 1;
                    var child3 = new MK2TreeNode();
                    child3.minx = node.minx;
                    child3.maxx = node.minx + half - 1;
                    child3.miny = node.maxy - half + 1;
                    child3.maxy = node.maxy;
                    var child4 = new MK2TreeNode();
                    child4.minx = node.maxx - half + 1;
                    child4.maxx = node.maxx;
                    child4.miny = node.maxy - half + 1;
                    child4.maxy = node.maxy;
                    node.childs.AddLast(child1);
                    node.childs.AddLast(child2);
                    node.childs.AddLast(child3);
                    node.childs.AddLast(child4);
                    stack.Push(child1);
                    stack.Push(child2);
                    stack.Push(child3);
                    stack.Push(child4);
                }
                else
                    node.value = isFullZeroOROne(K2Matrix, node.minx, node.maxx, node.miny, node.maxy).Item2 ? 1 : 0;
            }
            return tree;
        }

        private static bool isFullZero(K2TreeMatrix K2Matrix, int minx,int maxx ,int miny, int maxy)
        {
            var isZeroMatrix = true;
            for(int i = minx; i<= maxx; i++)
            {
                for(int j = miny; j <= maxy; j++)
                {
                    if (K2Matrix.bitMatrix[i,j])
                    {
                        isZeroMatrix = false;
                        break;
                    }
                }
            }
            return isZeroMatrix;
        }

        private static (bool,bool) isFullZeroOROne(K2TreeMatrix K2Matrix, int minx, int maxx, int miny, int maxy)
        {
            var isZeroMatrix = true;
            var isOneMatrix = true;
            for (int i = minx; i <= maxx; i++)
            {
                for (int j = miny; j <= maxy; j++)
                {
                    if (K2Matrix.bitMatrix[i, j])
                        isZeroMatrix = false;
                    else
                        isOneMatrix = false;
                    if (!isZeroMatrix && !isOneMatrix)
                        break;
                }
            }
            return (isZeroMatrix,isOneMatrix);
        }
    }
}
