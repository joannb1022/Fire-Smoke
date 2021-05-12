package program;

import utils.CellType;

import java.util.ArrayList;

public class Cell {
    CellType type;
    ArrayList<Cell> neighbours;

    public Cell() {
        this.type = CellType.AVAIABLE;
        this.neighbours = new ArrayList<>();
    }

}
