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
                }
            }
        }

        for (int x = 0; x < this.width; x++) {
            for (int y = 0; y < this.depth; y++) {
                for (int z = 0; z < this.height; z++) {

                }
            }
        }

    }

    public void iterate(){

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
