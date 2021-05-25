package program;

import utils.CellFuel;
import utils.CellType;

import java.util.ArrayList;
import java.util.Random;

import static java.lang.Math.abs;

public class Cell {
    private final float MAX_TREE_HEIGHT = 40f; // w metrach
    private final float MAX_GRASS_HEIGHT = 0.5f; // w metrach
    private final float MAX_DIAMETER_SIZE = 5f;
    private final float MAX_TRANSFER_TREE_COEF = 0.5f;
    private final float MAX_DIAMETER_GRASS_SIZE = 0.1f;
    private final float MAX_TRANSFER_GRASS_COEF = 0.2f;
    private static final Random random = new Random();
    private CellType type;
    private CellFuel fuel;
    private double temperature;
    private ArrayList<Cell> neighbours;
    private float slope;
    private int burnTemp; //trzeba chyba ustawiac inna dla drzewa i trawy?
    private float[][] windArray;
    private float height = 1f;
    private float diameter = 1f;
    private float heatTransferCoeff = 1f;


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

        if (this.fuel == CellFuel.TREE) {
            this.burnTemp = 500;
            this.diameter = 1 + random.nextFloat() * (MAX_DIAMETER_SIZE - 1);
            this.height = 1 + random.nextFloat() * (MAX_TREE_HEIGHT - 1);
            this.heatTransferCoeff = random.nextFloat() * (MAX_TRANSFER_TREE_COEF);
        }
        if (this.fuel == CellFuel.GRASS){
            this.burnTemp = 250;
            this.height = 1 + random.nextFloat() * (MAX_GRASS_HEIGHT - 1);
            this.diameter = 1 + random.nextFloat() * (MAX_DIAMETER_GRASS_SIZE - 1);
            this.heatTransferCoeff = random.nextFloat() * (MAX_TRANSFER_GRASS_COEF);
        }
        if(this.fuel == CellFuel.FIRE){
            type = CellType.BURNING;
        }

  }

    public void checkState(){
      if (this.temperature > this.burnTemp && this.type == CellType.AVAILABLE){
        this.type = CellType.BURNING;
        this.fuel = CellFuel.FIRE;
      }

      //i tu warunek wygaszania tez trzeba
    }

  public void fireSpread(){
        temperature += 2*heatTransferCoeff*(diameter*height)*100/height;
      for (Cell c : this.neighbours){
        c.temperature += c.heatTransferCoeff*(c.diameter*c.height)*abs(temperature-c.temperature)/c.height;
      }
  }

  public void setTemperature(double temp){
      this.temperature = temp;
  }

  public void setFuel(CellFuel fuel){
      System.out.println("FUEL: " + fuel);
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
