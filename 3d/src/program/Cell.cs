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
      float MAX_GRASS_TEMP = 250f;
      float GRASS_BURN_TEMP = 200f;
      float MAX_GROUND_TEMP = 250f;
      //  float MAX_GROUND_TEMP = 250f;
      float MAX_GRASS_HEIGHT = 0.5f; // w metrach
      float MAX_DIAMETER_SIZE = 5f;
      float GROUND_BURN_TEMP  = 250f;
      float MAX_TREE_HEIGHT = 40f; // w metrach
      float MAX_TRANSFER_TREE_COEF = 0.5f;
      float MAX_DIAMETER_GRASS_SIZE = 0.1f;
      float MAX_TRANSFER_GRASS_COEF = 0.2f;
      float MAX_TRANSFER_GROUND_COEF = 0.8f;
      CellFuel fuel;
      CellType type;
      double temperature;
      float height = 1f;
      float diameter = 1f;
      float heatTransferCoeff = 1f;
      ArrayList neighbours; //nie okresla sie tutaj typu arraylist chyba
      float burnTemp; //temperatura zaplonu
      int burnIterations; //liczba iteracji po ktorej przestanie sie palic (albo mozna jakos po temperaturze ale tak chyba latwiej)
      double initTemp; //temperatura powietrza, ta ktora cell ma na poczatku
      double maxTemperature; //np jakies 1000 stopni dla drzewa, zeby nie roslo w nieskonczonosc
      float fireSpeed = 0f; //nie wiem czy firespeed, czy windspeed sie przyda bardziej
      float windSpeed = 10f;
      WindDir windDir; //kierunek wiatru
      float[,,] probablityMatrix; //teraz chyba powinna byc 3d (?) nie wiem czy sie przyda
      // float[,] slopeMatrix;
      // float[,] windMatrix;

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
          this.windDir = dir;
          this.neighbours = new ArrayList();
          this.burnTemp = GROUND_BURN_TEMP;
      }

      public Cell(double t, CellFuel fuel, WindDir dir) {
          this.temperature = t;
          this.initTemp = t;
          this.fuel = fuel;
          // this.type = CellType.AVAILABLE;
          this.windDir = dir;
          this.neighbours = new ArrayList();
          this.burnTemp = GROUND_BURN_TEMP;
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
              this.maxTemperature = 1000;
              this.burnTemp = 500;
              this.burnIterations = 100;//do obliczenia
              this.diameter = 1 + random.Next() * (MAX_DIAMETER_SIZE - 1);
              this.height = 1 + random.Next() * (MAX_TREE_HEIGHT - 1);
              this.heatTransferCoeff = random.Next() * (MAX_TRANSFER_TREE_COEF);

          }
          else if (this.fuel == CellFuel.GRASS){
              this.type = CellType.AVAILABLE;
              this.maxTemperature = MAX_GRASS_TEMP;
              this.burnTemp = GRASS_BURN_TEMP;
              // this.burnIterations = ;

              this.height = 1 + random.Next() * (MAX_GRASS_HEIGHT - 1);
              this.diameter = 1 + random.Next() * (MAX_DIAMETER_GRASS_SIZE - 1);
              this.heatTransferCoeff = random.Next() * MAX_TRANSFER_GRASS_COEF;
          }
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

    }

    }

}
