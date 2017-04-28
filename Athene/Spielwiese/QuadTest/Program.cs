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
            WorldSizeX = 1000;
            WorldSizeY = 1000;
            TileSize = 4;
            int ListSize = 50000;
            int leafLimit = 9;


            //generate elements
            Rand = new Random();
            int runCounter = 0;
            var watch = new Stopwatch();
            var ElementList = new List<IQuadTreeElement>();
            Box2D playerHitbox;

            

            while(true)
            {
                if (runCounter % 4 == 0 && runCounter > 0)
                {
                    Console.ReadKey();
                    Console.Clear();
                }

                runCounter++;
                Console.WriteLine("\n\n\nNew run {0} \n===================================\n", runCounter);


                //new values
                playerHitbox = GetRandomBox();
                ElementList = new List<IQuadTreeElement>();
                for (int i = 0; i < ListSize; i++)
                {
                    ElementList.Add(new TestElement());
                }


                

                //test checking all
                var standardList = new List<TestElement>();
                watch.Start();
                    foreach(TestElement element in ElementList)
                    {
                    if (element.HitBox.IsInside(playerHitbox) || element.HitBox.IntersectsWith(playerHitbox))
                            standardList.Add(element);
                    }
                    standardList = standardList.Distinct().ToList();
                watch.Stop();
                var standardTime = watch.Elapsed.TotalMilliseconds * 1000;



                //test with octree
                var resultQuad = new List<IQuadTreeElement>();

                watch.Restart();
                    var tree = new QuadTreeRoot(new Box2D(0, 0, WorldSizeX, WorldSizeY), leafLimit, ElementList);
                watch.Stop();
                var createTime = watch.ElapsedMilliseconds;


                var quadList = new List<TestElement>();
                watch.Restart();
                    resultQuad = tree.GetElementsIn(playerHitbox);
                    foreach (TestElement element in resultQuad)
                        quadList.Add(element);
                watch.Stop();

                var quadTime = watch.Elapsed.TotalMilliseconds * 1000;





                Console.WriteLine("Results (Elements {0} ):", ListSize);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\tstandard : quadtree");

                if (quadTime <= standardTime)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tTime: {0} ns : {1} ns  -  {2}% diff", standardTime, quadTime, (standardTime > 0) ? 100 - (int)(quadTime * 100 / standardTime) : 0);

                if (standardList.Count == quadList.Count)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tHits: {0} : {1}", standardList.Count, quadList.Count);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\tTime creating tree: {0} ms", createTime);



                var exceptions = standardList.Except(quadList).ToList();
                exceptions = exceptions.Distinct().ToList();

                if (exceptions.Count > 0)
                {
                    Console.WriteLine("\nBoxes from gap (Elements {0} ):", exceptions.Count);
                    foreach (TestElement element in exceptions)
                        Console.WriteLine("\t" + element.ToString());
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Box2D GetRandomBox()
        {
            int posx = WorldSizeX - TileSize;
            int posy = WorldSizeY - TileSize;

            return new Box2D(Rand.Next(0, posx), Rand.Next(0, posy), TileSize, TileSize);
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

        /// <summary>
        /// 
        /// </summary>
        public new string ToString()
        {
            return String.Format("X: {0}, Y: {1}", HitBox.Position.X, HitBox.Position.Y);
        }
    }
}
