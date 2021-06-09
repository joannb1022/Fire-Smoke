using System;
using Enums;


namespace Wind{

  class Wind{

    public static float[,] windMatrix(WindDir dir){

        float[,] windMatrix = new float[3,3] {{0,0,0}, {0,0,0}, {0,0,0}};

          switch (dir){
            case WindDir.N:
                windMatrix[0,0] = 1;
                windMatrix[0,1] = 1;
                windMatrix[0,2] = 1;
                break;
            case WindDir.W:
                windMatrix[0,0] = 1;
                windMatrix[1,0] = 1;
                windMatrix[2,0] = 1;
                break;
            case WindDir.E:
                windMatrix[0,2] = 1;
                windMatrix[1,2] = 1;
                windMatrix[2,2] = 1;
                break;
            case WindDir.S:
                windMatrix[2,0] = 1;
                windMatrix[2,1] = 1;
                windMatrix[2,2] = 1;
                break;
            case WindDir.NE:
                windMatrix[0,1] = 1;
                windMatrix[0,2] = 1;
                windMatrix[1,2] = 1;
                break;
            case WindDir.NW:
                windMatrix[0,0] = 1;
                windMatrix[0,1] = 1;
                windMatrix[1,0] = 1;
                break;
            case WindDir.SW:
                windMatrix[1,0] = 1;
                windMatrix[2,0] = 1;
                windMatrix[2,1] = 1;
                break;
            case WindDir.SE:
                windMatrix[1,2] = 1;
                windMatrix[2,2] = 1;
                windMatrix[2,1] = 1;
                break;
          }

          return windMatrix;
    }
  }


  class SmokeClass{

      //funkcja zwraca wspolczynnik ilosciowy dn (strona 57, rownanie 3.19 i 3.20)
      public double getDn(NeighbourPos pos){
          switch(pos){
            case NeighbourPos.U:
                return 100.0;
            case NeighbourPos.N:
                return 50.0;
            case NeighbourPos.S:
                return 50.0;
            case NeighbourPos.W:
                return 50.0;
            case NeighbourPos.E:
                return 50.0;
            case NeighbourPos.D:
                return 25.0;
            default:
                return -1;
          }
      }
  }
}
