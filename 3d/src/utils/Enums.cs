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
        NONBURN
    }

    public enum SmokeE{
      SMOKE,
      NOSMOKE
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

    public enum NeighbourPos{
      U, //na gorze //0
      N, //1
      S, //2
      E, //3
      W, //4
      D  //na dole //5
    }


}
