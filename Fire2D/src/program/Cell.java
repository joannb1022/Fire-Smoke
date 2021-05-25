package program;

import utils.CellFuel;
import utils.CellType;

import java.util.ArrayList;

public class Cell {
    private CellType type;
    private CellFuel fuel;
    private double temperature;
    private ArrayList<Cell> neighbours;
    private float slope;
    private int burnTemp; //trzeba chyba ustawiac inna dla drzewa i trawy?
    private float[][] windArray;


    public Cell(double t) {
        this.temperature = t;
        this.neighbours = new ArrayList<>();
        this.fuel = CellFuel.GROUND;
    }

    public void addNeighbour(Cell cell) {
        this.neighbours.add(cell);
    }

    public void clear(){
      this.fuel = CellFuel.GROUND;
      this.type = CellType.AVAILABLE;
      // this.type = CellType.NONFUEL;
      this.temperature = 0;
    }

    // public void clicked(){
    //   this.fuel = CellFuel.TREE;
    //
    // }

    public void setParameters(){
        if (this.fuel == CellFuel.TREE || this.fuel == CellFuel.GRASS){
              this.type = CellType.AVAILABLE;
        }

        if (this.fuel == CellFuel.TREE){
          this.burnTemp = 150;
        if (this.fuel == CellFuel.GRASS){
            this.burnTemp = 100;
        }
    }
  }

    public void checkState(){
      if (this.temperature > this.burnTemp && this.type == CellType.AVAILABLE){
        this.type = CellType.BURNING;
      }

      //i tu warunek wygaszania tez trzeba
    }

  public void fireSpread(){
      for (Cell c : this.neighbours){

      }

  }

  public void setTemperature(double temp){
      this.temperature = temp;
  }

  public void setFuel(CellFuel fuel){
      this.fuel = fuel;
      this.setParameters();

    }


    public double getTemperature(){
      return this.temperature;
    }

    public CellType getType(){
      return this.type;
    }

    public CellFuel getFuel(){
      return this.fuel;
    }
}
