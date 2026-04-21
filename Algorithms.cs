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
        public double MHDKP1(MK2TreeNode a, MK2TreeNode b)
        {
            var cmax = 0.0;
            foreach (var p in a)
            {
                var minNN = Dist(p, pNN);
                if (minNN > cmax)
                    cmax = MNNMax(b, p, cmax, minNN);
            }
            return cmax;
        }

        private double MNNMax(MK2TreeNode b, (int x, int y) p, double cmax, double minNN)
        {
            var pQ = new PriorityQueue<MK2TreeNode, double>();
            var d = MaxDist(p, b);
            pQ.Enqueue(b, d);
            while (pQ.Count != 0)
            {
                MK2TreeNode node;
                double dist;
                var suceess = pQ.TryDequeue(out node, out dist);
                if (node.value == 1)
                {
                    if (dist <= cmax)
                        return cmax;
                    foreach (var child in node.childs)
                    {
                        if (child.childs.Count != 0)
                        {
                            var childMinDist = MinDist(p, child);
                            if (childMinDist < minNN)
                            {
                                var childMaxDist = MaxDist(p, child);
                                pQ.Enqueue(child, childMaxDist);
                            }
                        }
                    }
                }
                else
                {
                    var np = NN(p, node);
                    dist = Dist(p, np);
                    if (dist <= cmax)
                        return cmax;
                    if (dist < minNN)
                    {
                        minNN = dist;
                        pNN = (np.Item1, np.Item2);
                    }
                }
            }
            return minNN;
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
        private bool IsLeaf(MK2TreeNode node)
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

        public double MaxDist((int x, int y) p, MK2TreeNode node)
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
        public double MinDist((int x, int y) p, MK2TreeNode node)
        {
            var minDx = 0;
            var minDy = 0;
            minDx = p.x > node.maxx ? p.x - node.maxx : p.x < node.minx ? node.minx - p.x : 0;
            minDy = p.y > node.maxy ? p.y - node.maxy : p.y < node.miny ? node.miny - p.y : 0;
            return Math.Sqrt(minDx * minDx + minDy * minDy);
        }

        public (int,int) NN((int x, int y) p, MK2TreeNode node)
        {
           
            var minX = p.x > node.maxx ? p.x - node.maxx : p.x < node.minx ? node.minx - p.x : 0;
            var minY = p.y > node.maxy ? p.y - node.maxy : p.y < node.miny ? node.miny - p.y : 0;
            return (minX,minY);
        }

        public double Dist((double x, double y) a, (double x, double y) b) 
        {
            return Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
        }

        public double MaxMaxDist(K2TreeNode a, K2TreeNode b)
        {
            var pairs = new HashSet<((double, double), (double, double))> (){ 
                ((a.minx,a.miny),(b.maxx,b.maxy)),
                ((a.minx,a.maxy) ,(b.maxx,a.miny)),
                ((a.maxx, a.miny) , (b.minx, b .maxy)),
                ((a.maxx, a.maxy) , (b.minx, b .miny)),
            };
            return pairs.Max(pair => Dist(pair.Item1, pair.Item2));

        }

        public double MaxMaxDist(MK2TreeNode a, MK2TreeNode b)
        {
            var pairs = new HashSet<((double, double), (double, double))>(){
                ((a.minx,a.miny),(b.maxx,b.maxy)),
                ((a.minx,a.maxy) ,(b.maxx,a.miny)),
                ((a.maxx, a.miny) , (b.minx, b .maxy)),
                ((a.maxx, a.maxy) , (b.minx, b .miny)),
            };
            return pairs.Max(pair => Dist(pair.Item1, pair.Item2));

        }


        public double HDKP2(K2TreeNode a, K2TreeNode b)
        {
            var cmax = 0.0;
            var d = Math.Max(a.maxx, b.maxx) * Math.Sqrt(2);
            var pQ = new PriorityQueue<K2TreeNode, double>(Comparer<double>.Create((double x, double y) => x > y ? -1 : x < y ? 1 : 0));
            pQ.Enqueue(a, d);
            while (pQ.Count != 0)
            {
                K2TreeNode e;
                double dist;
                var OK = pQ.TryDequeue(out e,out dist);
                if (dist <= cmax)
                    return cmax;
                if (IsLeaf(e)) 
                {
                    var nn = NNMax(b, (e.maxx, e.maxy), cmax, Double.PositiveInfinity);
                    if (nn > cmax)
                        cmax = nn;
                }
                else
                {
                    foreach(var h in e.childs)
                    {
                        if(h.childs.Count != 0)
                        {
                            var dh = IsCandidate(h, b, cmax);
                            if (dh != -1)
                                pQ.Enqueue(h, dh);
                        }
                    }
                }
            }
            return cmax;
        }

        private double IsCandidate(K2TreeNode R , K2TreeNode b, double cmax)
        {
            var pR = new PriorityQueue<K2TreeNode, double>();
            var d = MaxMaxDist(R, b);
            pR.Enqueue(R, d);
            while (pR.Count != 0)
            {
                K2TreeNode e;
                double dist;
                var OK = pR.TryDequeue(out e, out dist);
                if (IsLeaf(e))
                    return dist;
                else
                {
                    foreach(var h in e.childs)
                    {
                        if(h.childs.Count!= 0)
                        {
                            var maxDist = MaxMaxDist(R, h);
                            if (maxDist <= cmax)
                                return -1;
                            else
                                pR.Enqueue(h, maxDist);
                        }
                    }
                }
            }
            return -1;
        }

        double MinMaxDist(MK2TreeNode R, MK2TreeNode S)
        {
            var dx = S.minx > (R.minx + R.maxx)/2.0 ? S.minx : S.maxx < (R.maxx + R.minx)/2.0 ? S.maxx : (R.maxx + R.minx)/2;
            var dy = S.miny > (R.miny + R.maxy) / 2.0 ? S.miny : S.maxy < (R.maxy + R.miny) / 2.0 ? S.maxy : (R.maxy + R.miny) / 2;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private double IsCandidate(MK2TreeNode R, MK2TreeNode b, double cmax)
        {
            var pR = new PriorityQueue<MK2TreeNode, double>();
            var d = MaxMaxDist(R, b);
            pR.Enqueue(R, d);
            while (pR.Count != 0)
            {
                MK2TreeNode e;
                double dist;
                var OK = pR.TryDequeue(out e, out dist);
                if (e.value == 1)
                {
                    return MinMaxDist(R, e);
                }
                else
                {
                    foreach (var h in e.childs)
                    {
                        if (h.childs.Count != 0)
                        {
                            var maxDist = MaxMaxDist(R, h);
                            if (maxDist <= cmax)
                                return -1;
                            else
                                pR.Enqueue(h, maxDist);
                        }
                    }
                }
            }
            return -1;
        }

        private HashSet<MK2TreeNode> GetChilds(MK2TreeNode n)
        {
            var half = n.maxx - n.minx;
            var child1 = new MK2TreeNode();
            child1.minx = n.minx;
            child1.maxx = n.minx + half - 1;
            child1.miny = n.miny;
            child1.maxy = n.miny + half - 1;
            child1.value = 1;
            var child2 = new MK2TreeNode();
            child2.minx = n.maxx - half + 1;
            child2.maxx = n.maxx;
            child2.miny = n.miny;
            child2.maxy = n.miny + half - 1;
            child2.value = 1;
            var child3 = new MK2TreeNode();
            child3.minx = n.minx;
            child3.maxx = n.minx + half - 1;
            child3.miny = n.maxy - half + 1;
            child3.maxy = n.maxy;
            child3.value = 1;
            var child4 = new MK2TreeNode();
            child4.minx = n.maxx - half + 1;
            child4.maxx = n.maxx;
            child4.miny = n.maxy - half + 1;
            child4.maxy = n.maxy;
            child4.value = 1;
            var result = new HashSet<MK2TreeNode>() { child1, child2, child3, child4 };
            return result;
        }

        public double HDKP2(MK2TreeNode a, MK2TreeNode b)
        {
            var cmax = 0.0;
            var d = Math.Max(a.maxx, b.maxx) * Math.Sqrt(2);
            var pQ = new PriorityQueue<MK2TreeNode, double>(Comparer<double>.Create((double x, double y) => x > y ? -1 : x < y ? 1 : 0));
            pQ.Enqueue(a, d);
            while (pQ.Count != 0)
            {
                MK2TreeNode e;
                double dist;
                var OK = pQ.TryDequeue(out e, out dist);
                if (dist <= cmax)
                    return cmax;
                if (IsLeaf(e))
                {
                    var nn = 
                       MNNMax(b, (e.maxx, e.maxy), cmax, Double.PositiveInfinity);
                    if (nn > cmax)
                        cmax = nn;
                }
                else
                {
                    if (e.childs.Count != 0)
                    {
                        foreach (var h in e.childs)
                        {
                            if (h.childs.Count != 0)
                            {
                                var dh = IsCandidate(h, b, cmax);
                                if (dh != -1)
                                    pQ.Enqueue(h, dh);
                            }
                        }
                    }
                    else
                    {
                        foreach (var h in GetChilds(e))
                        {
                            if (h.childs.Count != 0)
                            {
                                var dh = IsCandidate(h, b, cmax);
                                if (dh != -1)
                                    pQ.Enqueue(h, dh);
                            }
                        }
                    }
                }
            }
            return cmax;
        }
    }
}
