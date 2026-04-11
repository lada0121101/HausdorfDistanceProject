using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HausdorfDistanceProject
{
    internal class Algorithms
    {
        public (double, double) pNN = (Double.PositiveInfinity,Double.PositiveInfinity);
        public double HDKP1(K2TreeNode a, K2TreeNode b) 
        {
            
            var cmax = 0.0;
            foreach(var p in a)
            {
                var minNN = Dist(p, pNN);
                if (minNN > cmax)
                    cmax = NNMax(b, p, cmax, minNN);
            }
            return cmax;
        }

        private double NNMax(K2TreeNode b, (int x, int y) p, double cmax, double minNN)
        {
            var pQ = new PriorityQueue<K2TreeNode,double>();
            var d = MaxDist(p, b);
            pQ.Enqueue(b, d);
            while(pQ.Count != 0)
            {
                K2TreeNode node;
                double dist;
                var suceess = pQ.TryDequeue(out node, out dist);
                if (!IsLeaf(node))
                {
                    if (dist <= cmax)
                        return cmax;
                    foreach(var child in node.childs)
                    {
                        if(child.childs.Count != 0)
                        {
                            var childMinDist = MinDist(p, child);
                            if(childMinDist < minNN)
                            {
                                var childMaxDist = MaxDist(p, child);
                                pQ.Enqueue(child, childMaxDist);
                            }
                        }
                    }
                }
                else
                {
                    if (dist <= cmax)
                        return cmax;
                    if(dist < minNN)
                    {
                        minNN = dist;
                        pNN = (node.minx, node.miny);
                    }
                }
            }
            return minNN;
        }

        private bool  IsLeaf(K2TreeNode node)
        {
            return node.minx == node.maxx && node.value == 1;
        }
        public double MaxDist((int x, int y) p, K2TreeNode node) 
        {
            var maxDx = 0;
            var maxDy = 0;
            maxDx = p.x > (node.maxx + node.minx) / 2.0 ? p.x - node.minx : node.maxx - p.x;
            maxDy = p.y > (node.maxy + node.miny) / 2.0 ? p.y - node.miny : node.maxy - p.y;
            return Math.Sqrt(maxDx * maxDx + maxDy * maxDy);
        }

        public double MinDist((int x, int y) p, K2TreeNode node)
        {
            var minDx = 0;
            var minDy = 0;
            minDx = p.x > node.maxx ? p.x - node.maxx : p.x < node.minx ? node.minx - p.x : 0;
            minDy = p.y > node.maxy ? p.y - node.maxy : p.y < node.miny ? node.miny - p.y : 0;
            return Math.Sqrt(minDx * minDx + minDy * minDy);
        }

        public double Dist((double x, double y) a, (double x, double y) b) 
        {
            return Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
        }
    }
}
