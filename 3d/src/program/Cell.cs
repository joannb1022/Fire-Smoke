using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;
using Wind;

namespace Cells{
  public class Cell{

      int x, y, z; //niekoniecznie potrzebne

      //parametry glownie z pracy od prof Wasa
      // float MAX_GRASS_TEMP = 250f;
      // float GRASS_BURN_TEMP = 200f;
      // float MAX_GRASS_HEIGHT = 0.5f; // w metrach
      // float MAX_DIAMETER_GRASS_SIZE = 0.1f;
      // float MAX_TRANSFER_GRASS_COEF = 0.2f;

      float MAX_GROUND_TEMP = 250f;
      float GROUND_BURN_TEMP  = 250f;
      float MAX_TRANSFER_GROUND_COEF = 0.8f;


      /*
      Parametry dotyczace drzewa i powietrza pochodza z paracy prof Wasa
      Warto rozroznic temperature zaplonu (to jet temperatura, przy ktorej komorka zapala sie, jesli ma palacego sie juz sasiada)
      Temperatura samozaplonu - komorka zapala przy niej jesli nie ma palacych sie sasiadow
      */
      float SELF_BURN_TEMP_TREE = 2000f; //wg mnie to za duzo
      float BURN_TEMP_TREE = 250f;
      float SMOKE_EMISSION = 0.3f;
      float ENERGY_TREE = 1500f; // [J/s]
      float DEN_TREE = 400f; // [kg/m^3] gestosc - nie uwzgledniamy zmiany gestosci pod wplywem ciepla
      float TRANSFER_COEF_TREE = 0.3f; //lambda w pracy, wspolczynnik przeowdzenia
      float SPEC_HEAT_TREE = 2390f; // [J / (kg * K)] cieplo wlasciwe

      float DEN_AIR = 1.29f; // [kg/m^3] gestosc - nie uwzgledniamy zmiany gestosci pod wplywem ciepla
      float SPEC_HEAT_AIR = 1005f;   // [J / (kg * K)] cieplo wlasciwe
      float TRANSFER_COEF_AIR = 0.026f;  //wspolczynnik przeowdzenia
      float CONVECTION_COEF_AIR = 5f; // [ W / (m * K)]  wspolczynnik przyjmowania ciepla

      CellFuel fuel;
      CellType type;
      double temperature;
      float mass;   //masa
      float density; //gestosc
      float volume; //objetosc
      float specHeat;
      ArrayList neighbours; //nie okresla sie tutaj typu arraylist chyba
      int burningNeighbours;
      int burnIterations; //liczba iteracji po ktorej przestanie sie palic (albo mozna jakos po temperaturze ale tak chyba latwiej)
      double initTemp; //temperatura powietrza, ta ktora cell ma na poczatku
      double maxTemperature; //np jakies 1000 stopni dla drzewa, zeby nie roslo w nieskonczonosc
      float fireSpeed = 0f; //nie wiem czy firespeed, czy windspeed sie przyda bardziej
      float windSpeed = 10f;
      // WindDir windDir; //kierunek wiatru
      float burnTemp;
      float selfBurnTemp;
      float heatTransferCoeff;


      System.Random random = new System.Random();


      //tu mozna dodac jeszcze windspeed
      public Cell(double t, CellFuel fuel, WindDir dir, int x, int y, int z) {
          this.x = x;
          this.y = y;
          this.z = z;
          this.temperature = t;
          this.initTemp = t;
          this.fuel = fuel;
          // this.type = CellType.AVAILABLE;
          this.neighbours = new ArrayList();
          this.burnTemp = GROUND_BURN_TEMP;
          setParameters();
      }

      public Cell(double t, CellFuel fuel, WindDir dir) {
          this.temperature = t;
          this.initTemp = t;
          this.fuel = fuel;
          // this.type = CellType.AVAILABLE;
          // this.windDir = dir;
          this.neighbours = new ArrayList();
          this.burnTemp = GROUND_BURN_TEMP;
          setParameters();
      }

      public void addNeighbour(Cell cell) {
          this.neighbours.Add(cell);
      }

      public void clear(){
        this.type = CellType.AVAILABLE; //ani sie nie pali ani nie jest spalona
        this.temperature = initTemp;
        // this.temperature = 0;
      }

      public void setParameters(){

          if (this.fuel == CellFuel.TREE) {
              this.type = CellType.AVAILABLE;
              this.maxTemperature = SELF_BURN_TEMP_TREE;
              this.burnTemp = BURN_TEMP_TREE;
              this.selfBurnTemp = SELF_BURN_TEMP_TREE;
              this.specHeat = SPEC_HEAT_TREE;
              this.density = DEN_TREE;
              this.heatTransferCoeff = TRANSFER_COEF_TREE;
              // this.volume =
              // this.burnIterations =

          }
          // else if (this.fuel == CellFuel.GRASS){
          //     this.type = CellType.AVAILABLE;
          //     this.maxTemperature = MAX_GRASS_TEMP;
          //     this.burnTemp = GRASS_BURN_TEMP;
          //     // this.burnIterations = ;
          //
          //     this.height = 1 + random.Next() * (MAX_GRASS_HEIGHT - 1);
          //     this.diameter = 1 + random.Next() * (MAX_DIAMETER_GRASS_SIZE - 1);
          //     this.heatTransferCoeff = random.Next() * MAX_TRANSFER_GRASS_COEF;
          // }

          else if (this.fuel == CellFuel.GROUND){
              this.type = CellType.AVAILABLE;
              this.burnTemp = GROUND_BURN_TEMP;
              this.maxTemperature = MAX_GROUND_TEMP;
              this.heatTransferCoeff = random.Next() * (MAX_TRANSFER_GROUND_COEF);
              this.burnIterations = 10;//do obliczenia
          }
          else if(this.fuel == CellFuel.FIRE){
              type = CellType.BURNING;
          }
          else if (this.fuel == CellFuel.NONFUEL){
            //NO BO TO TEZ MOZE PRZEWODZIC CIEPLO W SUMIE
          }
          else if (this.fuel == CellFuel.AIR){
            this.type = CellType.NONBURN;
          }

    }

    //powietrze - powietrze (bierzemy komorke nad nami)
    public void Convection(){
        // this.CONVECTION_COEF_AIR -=0.1f;
        this.CONVECTION_COEF_AIR = this.CONVECTION_COEF_AIR / (this.mass * this.specHeat);  //to jest wzor z tej pracy

        foreach (Cell cell in this.neighbours){
            if (cell.getFuel() == CellFuel.AIR && this.neighbours.IndexOf(cell) == (int)NeighbourPos.U){
                double neighTemp = cell.getTemperature();
                this.temperature = this.temperature - this.CONVECTION_COEF_AIR*(this.temperature - neighTemp);
                neighTemp  = neighTemp + this.CONVECTION_COEF_AIR*(this.temperature - neighTemp);
                cell.setTemperature(neighTemp);
            }
        }

    }

    public void checkState(){
      if (this.temperature > this.burnTemp && this.type == CellType.AVAILABLE){
        this.type = CellType.BURNING;
        this.fuel = CellFuel.FIRE;
      }

      if(this.type == CellType.BURNING){
        burnIterations-=1;
      }

      if(this.burnIterations <= 0 ){
        this.type = CellType.BURNED;
        this.setFuel(CellFuel.NONFUEL);
      }
    }


      public void fireSpread(){
          if (this.fuel == CellFuel.TREE){
            this.temperature +=200;
            foreach (Cell cell in this.neighbours){
              if (this.temperature > cell.getTemperature()  && cell.getFuel() == CellFuel.TREE){
                cell.setTemperature(this.temperature);
                }
            }
        }
          // if (this.fuel = CellFuel.TREE){
          //


      }


      public void smokeSpread(){


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

      public void setFuel(CellFuel fuel){
        this.fuel = fuel;
        setParameters();
      }




    }

}
