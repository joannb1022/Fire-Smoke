package program;

import utils.CellFuel;

public class Environment {
    private final int width;
    private final int height;
    private final int depth;
    private final double temperature;
    private Cell[][][] cells;


    public Environment(int w, int h, int d, double t) {
        this.width = w;
        this.height = h;
        this.depth = d;
        this.temperature = t;
        this.cells = new Cell[this.width][this.depth][this.height];
    }

    public void initialize() {
        for (int x = 0; x < this.width; x++) {
            for (int y = 0; y < this.depth; y++) {
                for (int z = 0; z < this.height; z++) {
                    cells[x][y][z] = new Cell(this.temperature, CellFuel.AIR);
                    cells[x][y][z].setParameters();
                }
            }
        }

        for (int x = 0; x < this.width; x++) {
            for (int y = 0; y < this.depth; y++) {
                for (int z = 0; z < this.height; z++) {
                    if (z + 1 < this.height){
                        cells[x][y][z].addNeighbour(cells[x][y][z+1]);
                    }
                    if (z > 0){
                        cells[x][y][z].addNeighbour(cells[x][y][z-1]);
                    }
                    if (x + 1 < this.width){
                        cells[x][y][z].addNeighbour(cells[x+1][y][z]);
                    }
                    if (x > 0){
                        cells[x][y][z].addNeighbour(cells[x-1][y][z]);
                    }
                    if (y + 1 < this.depth){
                        cells[x][y][z].addNeighbour(cells[x][y+1][z]);
                    }
                    if (y > 0){
                      cells[x][y][z].addNeighbour(cells[x][y-1][z]);
                    }
                }
          }
      }
}


    public void iterate(){
    //   for (int x = 0; x < this.width; x++) {
    //       for (int y = 0; y < this.depth; y++) {
    //           for (int z = 0; z < this.height; z++) {
    //             cell[x][y][z].Conduction();
    //           }
    //         }
    //       }
    // for (int x = 0; x < this.width; x++) {
    //     for (int y = 0; y < this.depth; y++) {
    //         for (int z = 0; z < this.height; z++) {
    //            cell[x][y][z].Convection();
    //         }
    //       }
    //     }
    //   }

//   for (int x = 0; x < this.width; x++) {
//       for (int y = 0; y < this.depth; y++) {
//           for (int z = 0; z < this.height; z++) {
//             cell[x][y][z].updateTemperature();
//            }
//      }
// }

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
