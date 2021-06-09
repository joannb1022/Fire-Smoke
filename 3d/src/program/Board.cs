using System;

using Enums;
using Wind;
using Cells;


namespace Environment{

    public class Board{

      int width;
      int height;
      int depth;
      double temperature;
      Cell[,,] cells;
      CellFuel editFuel;
      //wybieramy przez klikniecie (?) nie wiem czy bedzie jedna gotowa czy bedziemy jakos inaczej klikac i tworzyc ta mape
      WindDir windDir;
      float windSPeed;


      public Board(int w, int h, int d, double t) {
          this.width = w;
          this.height = h;
          this.depth = d;
          this.temperature = t;
          this.cells = new Cell[this.width, this.depth, this.height];
      }

      public void initialize() {
          for (int x = 0; x < this.width; x++) {
              for (int y = 0; y < this.depth; y++) {
                  for (int z = 0; z < this.height; z++) {
                      //cells[x,y,z] = new Cell(this.temperature, editFuel, windDir);
                      cells[x, y, z] = new Cell(this.temperature, editFuel, windDir);
                      cells[x,y,z].setParameters();
                  }
              }
          }

          for (int x = 0; x < this.width; x++) {
              for (int y = 0; y < this.depth; y++) {
                  for (int z = 0; z < this.height; z++) {
                      if (z + 1 < this.height){
                          cells[x,y,z].addNeighbour(cells[x,y,z+1]);
                      }
                      if (z > 0){
                          cells[x,y,z].addNeighbour(cells[x,y,z-1]);
                      }
                      if (x + 1 < this.width){
                          cells[x,y,z].addNeighbour(cells[x+1,y,z]);
                      }
                      if (x > 0){
                          cells[x,y,z].addNeighbour(cells[x-1,y,z]);
                      }
                      if (y + 1 < this.depth){
                          cells[x,y,z].addNeighbour(cells[x,y+1,z]);
                      }
                      if (y > 0){
                        cells[x,y,z].addNeighbour(cells[x,y-1,z]);
                      }
                  }
            }
        }
    }


            public void iterate(){
                //   for (int x = 0; x < this.width; x++) {
                //       for (int y = 0; y < this.depth; y++) {
                //           for (int z = 0; z < this.height; z++) {
                //             applyWind(x, y, z);
                //           }
                //         }
                //       }
              for (int x = 0; x < this.width; x++) {
                  for (int y = 0; y < this.depth; y++) {
                      for (int z = 0; z < this.height; z++) {
                        cells[x, y, z].fireSpread();
                      }
                    }
                  }
            for (int x = 0; x < this.width; x++) {
                for (int y = 0; y < this.depth; y++) {
                    for (int z = 0; z < this.height; z++) {
                          if (cells[x,y,z].getFuel() == CellFuel.AIR)
                       cells[x,y,z].Convection();
                    }
                  }
                }


          //   for (int x = 0; x < this.width; x++) {
          //       for (int y = 0; y < this.depth; y++) {
          //           for (int z = 0; z < this.height; z++) {
          //               if (cells[x, y, z].getSmokeE() == SmokeE.SMOKE)
          //                 cells[x,y,z].smokeSpread();
          //            }
          //      }
          // }

          for (int x = 0; x < this.width; x++) {
              for (int y = 0; y < this.depth; y++) {
                  for (int z = 0; z < this.height; z++) {

                    cells[x,y,z].checkState();
                   }
             }
        }

        }


            private void applyWind(int x, int y, int z)
            {
                switch (windDir){
                    case WindDir.N:
                        if(z+1 < depth)
                            cells[x,y,z+1].fireSpread(); // nie wiem jak tutaj indeksujemy!
                        break;
                    case WindDir.W:
                        if(x-1 > 0)
                            cells[x-1,y,z].fireSpread(); // nie wiem jak tutaj indeksujemy!
                        break;
                    case WindDir.E:
                        if(x+1 < width)
                            cells[x+1,y,z].fireSpread(); // nie wiem jak tutaj indeksujemy!
                        break;
                    case WindDir.S:
                        if(z-1 > 0)
                            cells[x,y,z-1].fireSpread(); // nie wiem jak tutaj indeksujemy!
                        break;
                    case WindDir.NE:
                        if(x+1 < width && z+1 < depth)
                            cells[x+1,y,z+1].fireSpread(); // nie wiem jak tutaj indeksujemy!
                        break;
                    case WindDir.NW:
                        if(x-1 > 0 && z+1 < depth)
                            cells[x-1,y,z+1].fireSpread(); // nie wiem jak tutaj indeksujemy!
                        break;
                    case WindDir.SW:
                        if(x-1 > 0 && z-1 > 0)
                            cells[x-1,y + 1,z-1].fireSpread(); // nie wiem jak tutaj indeksujemy!
                        break;
                    case WindDir.SE:
                        if(x+1 < width && z-1 > 0)
                            cells[x+1,y,z-1].fireSpread(); // nie wiem jak tutaj indeksujemy!
                        break;
                }
            }
    public int getDepth() {
        return this.depth;
    }

    public int getHeight() {
        return this.height;
    }

    public int getWidth() {
        return this.width;
    }




    }




}
