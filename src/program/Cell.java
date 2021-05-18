package program;

import utils.CellFuel;
import utils.CellType;

import java.util.ArrayList;

public class Cell {
    private CellType type;
    private CellFuel fuel;
    private double temperature;
    private double convection; //parametr alfa
    private double conduction; //parametr lambda
    private double energy;
    private double burnTemp;

    //masa i cieplo wlasciwe i czas palenia moze przydatny do wygaszenia
    // private double mass;
    // private double heatEnergy;
    //private double burnTime;

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

    public void setParameters(){
      //wzielam te parametry z tej pracy
      this.convection = 0.3;
      this.conduction = 5;

      if (this.fuel == CellFuel.TREE || this.fuel == CellFuel.GRASS){
        this.burnTemp = 200; //w stopniach Celsjusza
      }
    }

    public void checkState(){
      if (this.temperature > this.burnTemp && this.type == CellType.AVAILABLE){
        this.type = CellType.BURNING;
      }

      //warunek na wygaszenie
      // if (this.type == BURNING){
      //   this.type = BURNED;
      // }

    }

    // public void updateTemperature(){
    //
    // }

    //powietrze - powietrze (bierzemy komorke nad nami, Td to my)
    public void Convection(/*Cell cell*/){
        this.convection -=0.1; //trzeba ten zminiejszac
        // this.convection = this.convection / (this.mass * this.heatEnergy);  //to jest wzor z tej pracy

        for (Cell cell : this.neighbours){
            //jesli jest nad nasza komorka
            if (cell.getFuel() == CellFuel.AIR){
                double neighTemp = cell.getTemperature();
                this.temperature = this.temperature - this.convection*(this.temperature - neighTemp);
                neighTemp  = neighTemp + this.convection*(this.temperature - neighTemp);
                cell.setTemperature(neighTemp);
            }
        }

    }

    //drzewa - drzewa (wszystko oprocz powietrza mam na mysli)
    // public void Conduction(){
    //     for (Cell cell : this.neighbours){
    //       if (cell.getFuel == TREE && cell.getTemperature < this.temperature){
    //
    //       }
    //
    //     }
    //
    // }

    //drzewa itd - powietrze
    public void Radiation(){

    }

    public void setTemperature(double temp){
      this.temperature = temp;
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
