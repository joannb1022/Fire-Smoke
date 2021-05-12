package program;

import utils.CellType;

import java.util.ArrayList;

public class Cell {
    private CellType type;
    private ArrayList<Cell> neighbours;

    public Cell() {
        this.type = CellType.AVAIABLE;
        this.neighbours = new ArrayList<>();
    }

}
