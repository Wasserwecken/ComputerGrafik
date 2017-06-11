using Lib.Input.Mapping;
using Lib.Level.Physics;
using Lib.LevelLoader.LevelItems;
using Lib.Visuals.Graphics;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Level.Items
{
    public static class PlayerFactory
    {
        /// <summary>
        /// Initialises a player based on his ID (some properties has to be differ, especially the input)
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public static Player CreatePlayer(int playerId, Vector2 startPosition)
        {
            //Input binding
            var mapList = GetPlayerKeyboardMapping(playerId);
            mapList.AddMappingGamePad(playerId, pad => pad.ThumbSticks.Left, inp => inp.MoveLeft, (inval, curval) => inval.Length > 0.01 && inval.X > 0 ? -inval.X : 0);
            mapList.AddMappingGamePad(playerId, pad => pad.ThumbSticks.Left, inp => inp.MoveRight, (inval, curval) => inval.Length > 0.01 && inval.X < 0 ? inval.X : 0);
            mapList.AddMappingGamePad(playerId, pad => pad.ThumbSticks.Left, inp => inp.MoveUp, (inval, curval) => inval.Length > 0.01 && inval.Y > 0 ? inval.Y : 0);
            mapList.AddMappingGamePad(playerId, pad => pad.ThumbSticks.Left, inp => inp.MoveDown, (inval, curval) => inval.Length > 0.01 && inval.Y < 0 ? -inval.Y : 0);
            mapList.AddMappingGamePad(playerId, pad => pad.Triggers.Right, inp => inp.Helping, (inval, curval) => inval > 0.5f ? true : false);
            mapList.AddMappingGamePad(playerId, pad => pad.Buttons.A, inp => inp.Jump, (inval, curval) => inval == ButtonState.Pressed);
            mapList.AddMappingGamePad(playerId, pad => pad.Buttons.B, inp => inp.Shoot, (inval, curval) => inval == ButtonState.Pressed);

            //Optic
            SpriteAnimated playerSprite = new SpriteAnimated(new Vector2(0.75f));
            playerSprite.AddAnimation(String.Format("Animations/player/{0}/walk", playerId), 600);
            playerSprite.AddAnimation(String.Format("Animations/player/{0}/fall", playerId), 600);
            playerSprite.AddAnimation(String.Format("Animations/player/{0}/climb", playerId), 400);
            playerSprite.AddAnimation(String.Format("Animations/player/{0}/idle", playerId), 1000);
            playerSprite.AddAnimation(String.Format("Animations/player/{0}/swim", playerId), 1000);
            playerSprite.AddAnimation(String.Format("Animations/player/{0}/help", playerId), 1000);
            playerSprite.StartAnimation("walk");

            //Physics
            var impulseProps = new Dictionary<EnvironmentType, EnergyObjectProperties>
            {
                {EnvironmentType.Air, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {EnvironmentType.Solid, new EnergyObjectProperties(30f, 30f, 0.1f, 0f)},
                {EnvironmentType.Ladder, new EnergyObjectProperties(10f, 10f, 0.1f, 0f)},
                {EnvironmentType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, 0f)},
                {EnvironmentType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };
            var forceProps = new Dictionary<EnvironmentType, EnergyObjectProperties>
            {
                {EnvironmentType.Air, new EnergyObjectProperties(6f, 30f, 0.1f, 0.2f)},
                {EnvironmentType.Solid, new EnergyObjectProperties(6f, 30f, 0.1f, 0.2f)},
                {EnvironmentType.Ladder, new EnergyObjectProperties(6f, 6f, 0.1f, 0f)},
                {EnvironmentType.Water, new EnergyObjectProperties(30f, 30f, 0.06f, -0.01f)},
                {EnvironmentType.Lava, new EnergyObjectProperties(15f, 15f, 0.025f, 0f)}
            };

            //create instance
            return new Player(startPosition, mapList, playerSprite, impulseProps, forceProps);
        }


        /// <summary>
        /// returns keyboard mapping only for the players, because the keyboard has to be shared, unlike a gamepad
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        private static InputMapList<PlayerActions> GetPlayerKeyboardMapping(int playerId)
        {
            var mapList = new InputMapList<PlayerActions>();

            switch(playerId)
            {
                case 0:
                    mapList.AddMappingKeyboard(Key.Left, inp => inp.MoveLeft, (inval, curval) => inval ? +1 : 0);
                    mapList.AddMappingKeyboard(Key.Right, inp => inp.MoveRight, (inval, curval) => inval ? +1 : 0);
                    mapList.AddMappingKeyboard(Key.Up, inp => inp.MoveUp, (inval, curval) => inval ? +1 : 0);
                    mapList.AddMappingKeyboard(Key.Down, inp => inp.MoveDown, (inval, curval) => inval ? +1 : 0);
                    mapList.AddMappingKeyboard(Key.ControlRight, inp => inp.Jump, (inval, curval) => inval);
                    mapList.AddMappingKeyboard(Key.Keypad0, inp => inp.Helping, (inval, curval) => inval);
                    mapList.AddMappingKeyboard(Key.ShiftRight, inp => inp.Shoot, (inval, curval) => inval);
                    break;

                case 1:
                    mapList.AddMappingKeyboard(Key.A, inp => inp.MoveLeft, (inval, curval) => inval ? +1 : 0);
                    mapList.AddMappingKeyboard(Key.D, inp => inp.MoveRight, (inval, curval) => inval ? +1 : 0);
                    mapList.AddMappingKeyboard(Key.W, inp => inp.MoveUp, (inval, curval) => inval ? +1 : 0);
                    mapList.AddMappingKeyboard(Key.S, inp => inp.MoveDown, (inval, curval) => inval ? +1 : 0);
                    mapList.AddMappingKeyboard(Key.Space, inp => inp.Jump, (inval, curval) => inval);
                    mapList.AddMappingKeyboard(Key.ControlLeft, inp => inp.Helping, (inval, curval) => inval);
                    mapList.AddMappingKeyboard(Key.ShiftLeft, inp => inp.Shoot, (inval, curval) => inval);
                    break;

                case 2:
                    break;

                case 3:
                    break;

                default:
                    break;
            }

            return mapList;
        }
    }
}
