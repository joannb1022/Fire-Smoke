using System;

namespace Enums
{

    public enum CellFuel
    {
        AIR,
        FIRE,
        GRASS,
        TREE,
        GROUND,
        SMOKE
    }


    public enum CellType
    {
        AVAILABLE,
        BURNING,
        BURNED,
        UNAVAILABLE 
    }

    public enum WindDir
    {
        N,
        E,
        S,
        W,
        NW,
        NE,
        SW,
        SE
    }
}
