using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Lib.Level.Items
{
    public interface IPlayerCommunication
    {
        void SetSpawnPosition(Vector2 position);
        bool IsShooter();
    }
}
