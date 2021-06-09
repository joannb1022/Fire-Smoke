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


  class Smoke{

      //funkcja zwraca wspolczynnik ilosciowy dn (strona 57, rownanie 3.19 i 3.20)
      public double getDn(NeigbourPos pos){
          switch(pos){
            case U:
                return 100;
            case N or S or W or E:
                return 50;
            case D:
                return 25;
          }
      }
  }
}
