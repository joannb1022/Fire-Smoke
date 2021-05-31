package program;

import utils.CellFuel;
import utils.CellType;
import utils.Wind;
import utils.WindDir;
import utils.Helper;


import java.util.ArrayList;
import java.util.Random;

import static java.lang.Math.abs;

public class Cell {
    int x, y;
    final float MAX_GRASS_TEMP = 250f;
    final float GRASS_BURN_TEMP = 200f;
    final float MAX_GROUND_TEMP = 250f;
    // final float MAX_GROUND_TEMP = 250f;
    final float MAX_GRASS_HEIGHT = 0.5f; // w metrach
    final float MAX_DIAMETER_SIZE = 5f;
    final float GROUND_BURN_TEMP  = 250f;
    final float MAX_TREE_HEIGHT = 40f; // w metrach
    final float MAX_TRANSFER_TREE_COEF = 0.5f;
    final float MAX_DIAMETER_GRASS_SIZE = 0.1f;
    final float MAX_TRANSFER_GRASS_COEF = 0.2f;
    final float MAX_TRANSFER_GROUND_COEF = 0.8f;
    static final Random random = new Random();
    CellType type;
    CellFuel fuel;
    double temperature;
    ArrayList<Cell> neighbours;
    float burnTemp; //trzeba chyba ustawiac inna dla drzewa i trawy?
    int burnIterations;
    double initTemp;
    double maxTemperature;
    float fireSpeed = 0f;
    float windSpeed = 10f;
    WindDir windDir;

    float[][] probablityMatrix;
    // float[][] slopeMatrix;
    float[][] windMatrix;
    float height = 1f;
    float diameter = 1f;
    float heatTransferCoeff = 1f;


    public Cell(double t, WindDir dir, int x, int y) {
        this.x = x;
        this.y = y;
        this.temperature = t;
        this.initTemp = t;
        this.fuel = CellFuel.GROUND;
        // this.type = CellType.AVAILABLE;
        this.windDir = dir;
        this.neighbours = new ArrayList<>();
        this.burnTemp = GROUND_BURN_TEMP;
        this.probablityMatrix = new float[3][3];
        setProbabiltyMatrix();
    }

    public void addNeighbour(Cell cell) {
        this.neighbours.add(cell);
    }

    public void clear(){
      this.fuel = CellFuel.GROUND;
      this.type = CellType.AVAILABLE;
      this.temperature = 0;
    }

    public void setParameters(){

        if (this.fuel == CellFuel.TREE) {
            this.type = CellType.AVAILABLE;
            this.maxTemperature = 1000;
            this.burnTemp = 500;
            this.burnIterations = 100;//do obliczenia
            this.diameter = 1 + random.nextFloat() * (MAX_DIAMETER_SIZE - 1);
            this.height = 1 + random.nextFloat() * (MAX_TREE_HEIGHT - 1);
            this.heatTransferCoeff = random.nextFloat() * (MAX_TRANSFER_TREE_COEF);

        }
        else if (this.fuel == CellFuel.GRASS){
            this.type = CellType.AVAILABLE;
            this.maxTemperature = MAX_GRASS_TEMP;
            this.burnTemp = GRASS_BURN_TEMP;
            // this.burnIterations = ;

            this.height = 1 + random.nextFloat() * (MAX_GRASS_HEIGHT - 1);
            this.diameter = 1 + random.nextFloat() * (MAX_DIAMETER_GRASS_SIZE - 1);
            this.heatTransferCoeff = random.nextFloat() * MAX_TRANSFER_GRASS_COEF;
        }
        else if (this.fuel == CellFuel.GROUND){
            this.type = CellType.AVAILABLE;
            this.burnTemp = GROUND_BURN_TEMP;
            this.maxTemperature = MAX_GROUND_TEMP;
            this.heatTransferCoeff = random.nextFloat() * (MAX_TRANSFER_GROUND_COEF);
            this.burnIterations = 10;//do obliczenia
        }
        else if(this.fuel == CellFuel.FIRE){
            type = CellType.BURNING;
        }
        else if (this.fuel == CellFuel.NONFUEL){
          //NO BO TO TEZ MOZE PRZEWODZIC CIEPLO W SUMIE
        }

  }

  public void setProbabiltyMatrix(){
    for (int i = 0; i < 3; i++){
      for (int j = 0; j < 3; j++){
        this.probablityMatrix[i][j] = 0;
      }
    }
  }

    public void calculateProbablityMatrix(){
      for (int i = 0; i < 3; i++){
        for (int j = 0; j < 3; j++){
          float prob = 0f;
          if (windMatrix[i][j] == 1)
            prob = Helper.nextFloatBetween2(0.5f , 1f) * this.windSpeed/10;
          else
            prob = Helper.nextFloatBetween2(0.f, 0.5f) * this.windSpeed/10;
          prob += this.initTemp/100;
          probablityMatrix[i][j] = prob;
        }
      }
    }

    public void checkState(){
      if (this.temperature > this.burnTemp && this.type == CellType.AVAILABLE){
        this.type = CellType.BURNING;
        this.fuel = CellFuel.FIRE;
      }

      // if(this.burnIterations <= 0 ){
      //   this.type = CellType.BURNED;
      //   this.setFuel(CellFuel.NONFUEL);
      // }


      //i tu warunek wygaszania tez trzeba
    }

  public void fireSpread(){
      //   if (this.fuel == CellFuel.GROUND){
      //     this.temperature -= MAX_TRANSFER_GROUND_COEF * this.initTemp + this.temperature;
      //     for (Cell c : this.neighbours){
      //       if (c.getFuel() == CellFuel.GROUND){
      //         c.temperature -= c.temperature * this.heatTransferCoeff;
      //     }
      //   }
      // // }
      // else {
          this.temperature += 2*this.heatTransferCoeff*(this.diameter*this.height)*100/this.height;
          for (Cell c : this.neighbours){
            float prob = probablityMatrix[c.getX() - this.x + 1][c.getY() - this.y + 1];
            if(prob > 1)
              c.temperature += c.heatTransferCoeff*(c.diameter*c.height)*abs(temperature-c.temperature)/c.height;
          }
      }
  // }

  public void setTemperature(double temp){
      this.temperature = temp;
  }

  public void setWindMatrix(float[][] matrix){
    this.windMatrix = matrix;
  }

  public void setFuel(CellFuel fuel){
      this.fuel = fuel;
      this.setParameters();

    }
    // public void setType(CellType type){
    //     this.type = type;
    //     this.changea();
    //   }


    public double getTemperature(){
      return this.temperature;
    }

    public CellType getType(){
      return this.type;
    }

    public CellFuel getFuel(){
      return this.fuel;
    }

    int getX(){
      return this.x;
    }


    int getY(){
      return this.y;
    }
    // public void setSlope(float slope){
    //   this.slope = slope;
    // }
}
