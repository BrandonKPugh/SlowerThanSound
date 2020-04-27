using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    // Currently this class holds no data. 
    //      If we don't end up needing it, I'll remove the class altogether.
    public class Tile
    {
        public int X;
        public int Y;

        public Tile(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
