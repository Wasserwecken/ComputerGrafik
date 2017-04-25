using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Tools;
using Lib.Tools.QuadTree;
using System.Diagnostics;

namespace QuadTest
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        private static Random Rand { get; set; }

        private static int WorldSizeX { get; set; }
        private static int WorldSizeY { get; set; }
        private static int TileSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Rand = new Random();
            WorldSizeX = 50000;
            WorldSizeY = 50000;
            TileSize = 100;

            //generate elements
            int runCounter = 0;
            var watch = new Stopwatch();
            var playerRange = GetRandomBox();
            int ListSize = 100000;
            int ProcessCollisionTime = 1;

            var ElementList = new List<IQuadTreeElement>();
            for (int i = 0; i < ListSize; i++)
                ElementList.Add(new TestElement());



            while(true)
            {
                if (runCounter % 4 == 0)
                {
                    Console.ReadKey();
                    Console.Clear();
                }

                runCounter++;
                Console.WriteLine("\n\nNew run {0} \n=================================== \n\n", runCounter);


                //new values
                playerRange = GetRandomBox();
                ElementList = new List<IQuadTreeElement>();
                for (int i = 0; i < ListSize; i++)
                    ElementList.Add(new TestElement());



                Console.WriteLine("Elements : {0}", ListSize);

                //test checking all
                Console.WriteLine("\n\nStandard method");
                var resultStandard = new List<TestElement>();

                watch.Start();
                foreach(TestElement element in ElementList)
                {
                    if (playerRange.IsInside(element.HitBox) || playerRange.IntersectsWith(element.HitBox))
                    {
                        resultStandard.Add(element);
                        System.Threading.Thread.Sleep(ProcessCollisionTime);
                    }
                }
                watch.Stop();

                Console.WriteLine("\t{0} Time", watch.ElapsedMilliseconds);
                Console.WriteLine("\t{0} Intersections", resultStandard.Count);





                //test with octree
                Console.WriteLine("\n\nQuadtree method");

                var resultQuad = new List<IQuadTreeElement>();

                watch.Restart();
                var tree = new QuadTreeRoot(new Box2D(0, 0, WorldSizeX, WorldSizeY), 25, ElementList);
                watch.Stop();
                var createTime = watch.ElapsedMilliseconds;


                watch.Restart();
                resultQuad = tree.GetElements(playerRange);
                foreach(TestElement element in resultQuad)
                    System.Threading.Thread.Sleep(ProcessCollisionTime);
                watch.Stop();

                Console.WriteLine("\t{0} Time", watch.ElapsedMilliseconds);
                Console.WriteLine("\t{0} Intersections", resultQuad.Count);
                Console.WriteLine("\t{0} Time creating tree", createTime);

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Box2D GetRandomBox()
        {
            int fieldSizeX = WorldSizeX - TileSize;
            int fieldSizeY = WorldSizeY - TileSize;

            return new Box2D(Rand.Next(0, fieldSizeX), Rand.Next(0, fieldSizeY), TileSize, TileSize);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class TestElement
        : IQuadTreeElement
    {
        /// <summary>
        /// 
        /// </summary>
        public Box2D HitBox { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public TestElement()
        {
            HitBox = Program.GetRandomBox();
        }
    }
}
