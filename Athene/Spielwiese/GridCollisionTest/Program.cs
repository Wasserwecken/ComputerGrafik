using Lib.Level.Base;
using Lib.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCollisionTest
{
    public static class Parameters
    {
        public static int ItemCount { get; set; }   = 200000;
        public static int TileSize { get; set; }    = 1;
        public static int LevelSize { get; set; }   = 2000;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            float gridCreateTime = 0;
            float gridSearchTime = 0;
            float normalSearchTime = 0;

            List<IIntersectable> Items = new List<IIntersectable>();
            List<IIntersectable> GridList = new List<IIntersectable>();
            List<IIntersectable> NormalList = new List<IIntersectable>();


            while (true)
            {

                Items.Clear();
                for (int i = 0; i < Parameters.ItemCount; i++)
                    Items.Add(new Item());

                Item PlayerTile = new Item();


                Grid GridTest = new Grid(Parameters.LevelSize, 10);

                watch.Restart();
                foreach (var item in Items)
                    GridTest.AddItem(item);
                watch.Stop();
                gridCreateTime = watch.ElapsedMilliseconds;

                watch.Restart();
                GridList = GridTest.GetElementsIn(PlayerTile.HitBox);
                watch.Stop();
                gridSearchTime = watch.ElapsedMilliseconds;




                watch.Restart();
                NormalList = new List<IIntersectable>();
                foreach (var item in Items)
                {
                    if (PlayerTile.HitBox.IntersectsWith(item.HitBox))
                        NormalList.Add(item);
                }
                watch.Stop();
                normalSearchTime = watch.ElapsedMilliseconds;



                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("----------------------------------");

                if (normalSearchTime < gridSearchTime)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("\tSearch: {0}:{1}", normalSearchTime, gridSearchTime);


                if (normalSearchTime < gridSearchTime + gridCreateTime)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("\tComplete: {0}:{1}", normalSearchTime, gridSearchTime + gridCreateTime);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\tCreate time: {0}\n\n", gridCreateTime);



                Console.ReadKey();
            }
        }
    }

    public class Grid
    {
        /// <summary>
        /// 
        /// </summary>
        private List<IIntersectable>[,] Container { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private int ContainerSize { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Grid(int levelSize, int containerSize)
        {
            var gridSize = levelSize / containerSize;

            Container = new List<IIntersectable>[gridSize, gridSize];
            ContainerSize = containerSize;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItem"></param>
        public void AddItem(IIntersectable newItem)
        {
            int containerX = (int)newItem.HitBox.Position.X / ContainerSize;
            int containerY = (int)newItem.HitBox.Position.Y / ContainerSize;

            int containerXEnd = (int)newItem.HitBox.MaximumX / ContainerSize;
            int containerYEnd = (int)newItem.HitBox.MaximumY / ContainerSize;

            for(;containerX < containerXEnd + 1; containerX++)
            {
                for (; containerY < containerYEnd + 1; containerY++)
                {
                    if (Container[containerX, containerY] == null)
                        Container[containerX, containerY] = new List<IIntersectable>();

                    Container[containerX, containerY].Add(newItem);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public List<IIntersectable> GetElementsIn(Box2D reference)
        {
            var result = new List<IIntersectable>();
            int rangeX = (int)reference.Position.X / ContainerSize;
            int rangeY = (int)reference.Position.Y / ContainerSize;

            int rangeXEnd = (int)reference.MaximumX / ContainerSize;
            int rangeYEnd = (int)reference.MaximumY / ContainerSize;


            for (; rangeX < rangeXEnd + 1; rangeX++)
            {
                for (; rangeY < rangeYEnd + 1; rangeY++)
                {
                    foreach(var item in Container[rangeX, rangeY])
                    {
                        if (reference.IntersectsWith(item.HitBox))
                            result.Add(item);
                    }
                }
            }

            return result.Distinct().ToList();
        }
    }


    public class Item
        : IIntersectable
    {
        /// <summary>
        /// 
        /// </summary>
        public Box2D HitBox { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasCollisionCorrection { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Item()
        {
            Random rand = new Random();

            var x = rand.Next(0, Parameters.LevelSize - Parameters.TileSize);
            var y = rand.Next(0, Parameters.LevelSize - Parameters.TileSize);
            HitBox = new Box2D(x, y, Parameters.TileSize, Parameters.TileSize);
        }

        public void HandleCollisions(List<IIntersectable> intersectingItems) { }
    }
}
