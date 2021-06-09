using System;

namespace Enums{

    public enum CellFuel {
          AIR,
          FIRE,
          GRASS,
          TREE,
          GROUND,
          NONFUEL
    }


    public enum CellType {
        AVAILABLE,
        BURNING,
        BURNED,
    }

    public enum WindDir {
        N,
        E,
        S,
        W,
        NW,
        NE,
        SW,
        SE
    }

    public enum NeigbourPos{
      U,   //na gorze
      N,
      S,
      E,
      W,
      D  //na dole
    }


}
