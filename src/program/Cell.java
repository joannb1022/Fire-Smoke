package program;

import utils.CellFuel;
import utils.CellType;

import java.util.ArrayList;

public class Cell {
    private CellType type;
    private CellFuel fuel;
    private double temperature;
    private ArrayList<Cell> neighbours;

    public Cell(double t, CellFuel f) {
        this.type = CellType.AVAILABLE;
        this.fuel = f;
        this.temperature = t;
        this.neighbours = new ArrayList<>();
    }

    public void addNeighbour(Cell cell) {
        this.neighbours.add(cell);
    }

}
