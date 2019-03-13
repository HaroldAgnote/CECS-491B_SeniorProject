using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Tiles;

namespace Assets.Scripts.Model.Units {
    public interface IMover {
        bool CanMove(Tile tile);
        int MoveCost(Tile tile);
    }
}
