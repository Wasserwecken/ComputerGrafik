using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Input.Mapping;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;
using OpenTK;
using OpenTK.Input;
using Lib.Tools;
using Lib.Tools.QuadTree;

namespace Lib.LevelLoader
{
    public class Level
    {
        /// <summary>
        /// List of all blocks drawn in the level, like walkable, water, lava, trees..
        /// </summary>
        public List<Block> Blocks { get; set; }

        /// <summary>
        /// List of all players in the level
        /// </summary>
        public List<LevelItemPlayer> Players { get; set; }


        /// <summary>
        /// QuadTree for all level Blocks
        /// </summary>
        private QuadTreeRoot BlocksQuadTree { get; set; }


        /// <summary>
        /// Initializes a empty level
        /// </summary>
        public Level()
		{
			Blocks = new List<Block>();
            Players = new List<LevelItemPlayer>();

            CreateTestPlayer();
		}

        /// <summary>
        /// This function should NOT be necessary
        /// </summary>
        public void InitialiseQuadTree()
        {
            var quadList = new List<IQuadTreeElement>();
            float MinX = 0;
            float MinY = 0;
            float MaxX = 0;
            float MaxY = 0;

            foreach (Block levelBlock in Blocks)
            {
                if (levelBlock.HitBox.Position.X < MinX)
                    MinX = levelBlock.HitBox.Position.X;

                if (levelBlock.HitBox.MaximumX > MaxX)
                    MaxX = levelBlock.HitBox.MaximumX;

                if (levelBlock.HitBox.Position.Y < MinY)
                    MinY = levelBlock.HitBox.Position.Y;

                if (levelBlock.HitBox.MaximumY > MaxY)
                    MaxY = levelBlock.HitBox.MaximumY;

                quadList.Add(levelBlock);
            }

            var levelSize = new Box2D(MinX, MinY, MaxX - MinX, MaxY - MinY);
            BlocksQuadTree = new QuadTreeRoot(levelSize, 4, quadList);
        }

        /// <summary>
        /// updates the level
        /// </summary>
        public void UpdateLogic()
        {
            //Moving all the characters first
            foreach (var player in Players)
            {
                var intersections = BlocksQuadTree.GetElementsIn(player.HitBox).ConvertAll(item => (LevelItemBase)item);
                player.UpdateLogic(intersections);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void CreateTestPlayer()
        {
            LevelItemPlayer player = null;

            SpriteAnimated playerSprite = new SpriteAnimated();
            playerSprite.AddAnimation("Pics/Worm/walk", 1000);
            playerSprite.AddAnimation("Pics/Worm/idle", 1000);
            playerSprite.StartAnimation("walk");

            var mapList = new InputMapList<LevelItemPlayerActions>();

            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveLeft, (inval, curval) => inval.Length > 0.01 && inval.X > 0 ? -inval.X : 0);
            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveRight, (inval, curval) => inval.Length > 0.01 && inval.X < 0 ? inval.X : 0);
            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveUp, (inval, curval) => inval.Length > 0.01 && inval.Y > 0 ? inval.Y : 0);
            mapList.AddMappingGamePad(0, pad => pad.ThumbSticks.Left, inp => inp.MoveDown, (inval, curval) => inval.Length > 0.01 && inval.Y < 0 ? -inval.Y : 0);
            mapList.AddMappingGamePad(0, pad => pad.Buttons.A, inp => inp.Jump, (inval, curval) => inval == ButtonState.Pressed);

            mapList.AddMappingKeyboard(Key.Left, inp => inp.MoveLeft, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Right, inp => inp.MoveRight, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Up, inp => inp.MoveUp, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Down, inp => inp.MoveDown, (inval, curval) => inval ? +1 : 0);
            mapList.AddMappingKeyboard(Key.Space, inp => inp.Jump, (inval, curval) => inval);

            player = new LevelItemPlayer(Vector2.Zero, mapList, playerSprite);
            Players.Add(player);

        }

        /// <summary>
        /// Draws the level
        /// </summary>
        public void Draw(Box2D cameraFOV)
        {
            //Using the quadtree here because, only blocks in the camera view has to be drawn
            foreach(Block levelBlock in BlocksQuadTree.GetElementsIn(cameraFOV))
                levelBlock.Draw();
            
            //Draw players
            foreach (var player in Players)
                player.Draw();
        }

    }
}
