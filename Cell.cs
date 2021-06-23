using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;
using Controllers;
using UnityEngine;


namespace Cells{
    public class Cell{

        /*
        DENSITY - [kg/m^3] 
      
        SPEC_HEAT - [J / (kg * K)]

        CONVECTION COEF - [W/ (m  * K)]

        BURN_TEMP - at this temperature cell starts burning 
        */

        int x, y, z; 
        float VOLUME = 0.125f; // [m^3]  0.5 x 0.5 x 0.5 [m]
        float SURFACE = 0.25f; // [m^2]
        float DIST = 0.5f; // distance between centers of cells [m]
        double STEFAN_BOLTZMAN = 5.67f * Math.Pow(10, -8);
        float SMOKE_DENSITY = 0.0006f; // medium dense smoke 

        float GRASS_MAX_TEMP = 600f;
        float GRASS_BURN_TEMP = 200f;
        float GRASS_DENSITY = 120f; 
        float GRASS_SPEC_HEAT = 500f; 
        float GRASS_TRANSFER_COEF = 0.035f;

        float GROUND_MAX_TEMP = 250f;
        float GROUND_SPEC_HEAT = 800f; // for dry soil
        float GROUND_TRANSFER_COEF = 0.55f; 
        float GROUND_DENSITY = 1600f; 

        float TREE_MAX_TEMP = 1000f;
        float TREE_BURN_TEMP = 300f;
        float TREE_DENSITY = 400f; 
        float TREE_TRANSFER_COEF = 0.14f; 
        float TREE_SPEC_HEAT = 2390f; 
        int TREE_BURN_ITERATIONS = 20;

        float AIR_DENSITY = 1.29f; 
        float AIR_SPEC_HEAT = 1005f;   
        float AIR_TRANSFER_COEF = 0.026f;  
        float AIR_MAX_TEMP = 1000f;
        float AIR_CONVECTION_COEF = 5f; 


        CellFuel fuel;
        int fuelT;
        CellType type;
        double temperature;
        float heatTransferCoeff;
        ArrayList neighbours; 
        int burningNeighbours;
        int burnIterations; 
        double initTemp;
        double maxTemperature; 
        WindDir windDir; 
        float burnTemp;
        Controller con;
        int visited;
        float mass;
        float volume;
        float density;
        float specHeat;
        float convection;
        float smokeVol;

        public Cell(Controller c, double t, CellFuel fuel, int fuelT, WindDir dir, int x, int y, int z)
        {
            this.con = c;
            this.x = x;
            this.y = y;
            this.z = z;
            this.temperature = t;
            this.initTemp = t;
            this.fuel = fuel;
            this.fuelT = fuelT;
            this.windDir = dir;
            this.volume = VOLUME;
            this.neighbours = new ArrayList();
            setParameters();
        }

        public void addNeighbour(Cell cell){
            this.neighbours.Add(cell);
        }

        public void setParameters(){

            if (this.fuel == CellFuel.TREE){
                this.type = CellType.AVAILABLE;
                this.maxTemperature = TREE_MAX_TEMP;
                this.burnIterations = TREE_BURN_ITERATIONS; 
                this.heatTransferCoeff = TREE_TRANSFER_COEF;
                this.density = TREE_DENSITY;
                this.burnTemp = TREE_BURN_TEMP;
                this.specHeat = TREE_SPEC_HEAT;
            }

            else if (this.fuel == CellFuel.GRASS){
                this.type = CellType.AVAILABLE;
                this.maxTemperature = GRASS_MAX_TEMP;
                this.burnTemp = GRASS_BURN_TEMP;
                this.burnIterations = TREE_BURN_ITERATIONS / 2;
            }

            else if (this.fuel == CellFuel.GROUND){
                this.type = CellType.UNAVAILABLE;
                this.density = GROUND_DENSITY;
                this.specHeat = GROUND_SPEC_HEAT;
                this.heatTransferCoeff = GROUND_TRANSFER_COEF;
                this.maxTemperature = GROUND_MAX_TEMP;

            }
            
            else if (this.fuel == CellFuel.AIR){
                this.type = CellType.AVAILABLE;
                this.burnTemp = 100;
                this.burnIterations = 1;
                this.heatTransferCoeff = AIR_TRANSFER_COEF;
                this.specHeat = AIR_SPEC_HEAT;
                this.density = AIR_DENSITY;
                this.maxTemperature = AIR_MAX_TEMP;
                this.convection = AIR_CONVECTION_COEF;

            }

            calculateMass(); 
            calculateSmokeVol();
        }

        public void applyWind()
        {
            foreach (Cell cell in this.neighbours) {
                
                switch (windDir)
                {
                    case WindDir.N:
                        if (cell.getFuel() == CellFuel.AIR && cell.getZ() == this.z && this.x-1 == cell.getX())
                        {
                            cell.type = CellType.BURNING;
                        }
                        break;
                    case WindDir.W:
                        if (cell.getFuel() == CellFuel.AIR && cell.getZ() == this.z && this.y-1 == cell.getY())
                        {
                            cell.type = CellType.BURNING;
                        }
                        break;
                    case WindDir.E:
                        if (cell.getFuel() == CellFuel.AIR && cell.getZ() == this.z && this.y+1 == cell.getY())
                        {
                            cell.type = CellType.BURNING;
                        }
                        break;
                    case WindDir.S:
                        if (cell.getFuel() == CellFuel.AIR && cell.getZ() == this.z && this.x+1 == cell.getX())
                        {
                            cell.type = CellType.BURNING;
                        }
                        break;
                }
            }
        }

        public void Convection(){

            this.convection = this.convection / (this.mass * this.specHeat); 

            foreach (Cell cell in this.neighbours) {
                // convection with cell above us 
                if (cell.getFuel() == CellFuel.AIR && cell.getZ() == this.z + 1 && this.temperature > cell.getTemperature()){  // str. 40 -41
                    double neighTemp = cell.getTemperature();
                    double newTempNeighbour  = neighTemp + this.convection * (this.temperature - neighTemp);
                    this.temperature -= this.convection * (this.temperature - neighTemp);
                    cell.setTemperature(newTempNeighbour);
                }
            }

        }

        public void checkState(){
            if (this.temperature >= this.burnTemp && this.type == CellType.AVAILABLE){
                this.type = CellType.BURNING;
            }

            if (this.burnIterations <= 0 && this.type == CellType.BURNING){
                this.type = CellType.BURNED;
            }

            if (this.type == CellType.BURNING && this.burnIterations > 0) {
                this.burnIterations--;
            }

            this.visited = 0;
        }


        public void fireSpread(){

               if (this.visited == 0){

                   if (this.TREE_MAX_TEMP - this.temperature > 50)
                        this.temperature +=50; //tego nie jestem pewna, mozna jakos naukowo bardziej
 
                    if (this.fuel == CellFuel.TREE){ 

                        this.heatTransferCoeff = this.heatTransferCoeff / (this.mass * this.specHeat); 

                        foreach (Cell cell in this.neighbours){

                            // tree - tree
                            if (cell.getFuel() == CellFuel.TREE && cell.getTemperature() < this.temperature){ 
                                double newTemp = heatTransfer(cell); 
                                Debug.Log("TREE - TREE newTemp: " + newTemp);
                                if (newTemp < TREE_MAX_TEMP)
                                    cell.setTemperature(newTemp);
                                else cell.setTemperature(TREE_MAX_TEMP);
                            }

                            // tree - air
                            if (cell.getFuel() == CellFuel.AIR && cell.getTemperature() < this.temperature){ 
                                double newTemp =  radiate(cell); 
                                Debug.Log("TREE - AIR: newTemp: " + newTemp);
                                if (newTemp < AIR_MAX_TEMP)
                                    cell.setTemperature(newTemp);
                                else 
                                    cell.setTemperature(AIR_MAX_TEMP);

                            }

                            // tree - grass
                            if (cell.getFuel() == CellFuel.GRASS && cell.getTemperature() < this.temperature){   
                                double newTemp =  heatTransfer(cell); 
                                Debug.Log("TREE - grass newTemp: " + newTemp);
                                if (newTemp < GRASS_MAX_TEMP)
                                    cell.setTemperature(newTemp);
                                else 
                                    cell.setTemperature(GRASS_MAX_TEMP);
                            }


                            // tree - ground
                            if (cell.getFuel() == CellFuel.GROUND && cell.getTemperature() < this.temperature){   // from warmer cell to  one
                                double newTemp =  heatTransfer(cell); 
                                Debug.Log("TREE - GROUND newTemp: " + newTemp);
                                if (newTemp < GROUND_MAX_TEMP)
                                    cell.setTemperature(newTemp);
                                else 
                                    cell.setTemperature(GROUND_MAX_TEMP);
                            }
                    }

                }

                if (this.fuel == CellFuel.GRASS){

                    this.heatTransferCoeff = this.heatTransferCoeff / (this.mass * this.specHeat); 

                    foreach(Cell cell in this.neighbours){

                        // grass - grass 
                        if (cell.getFuel() == CellFuel.GRASS && cell.getTemperature() < this.temperature){
                            double newTemp = heatTransfer(cell);
                            Debug.Log("GRASS - GRASS: " + newTemp);
                                if (newTemp < GRASS_MAX_TEMP)
                                    cell.setTemperature(newTemp);
                                else 
                                    cell.setTemperature(GRASS_MAX_TEMP);
                        }

                        //grass - tree
                        if (cell.getFuel() == CellFuel.TREE && cell.getTemperature() < this.temperature){
                            double newTemp = heatTransfer(cell);
                            Debug.Log("GRASS - TREE: " + newTemp);
                                if (newTemp < TREE_MAX_TEMP)
                                    cell.setTemperature(newTemp);
                                else 
                                    cell.setTemperature(TREE_MAX_TEMP);
                        }

                        //grass - air
                        if (cell.getFuel() == CellFuel.AIR && cell.getTemperature() < this.temperature){
                            double newTemp = heatTransfer(cell);
                            Debug.Log("GRASS - AIR: " + newTemp);
                                if (newTemp < AIR_MAX_TEMP)
                                    cell.setTemperature(newTemp);
                                else 
                                    cell.setTemperature(AIR_MAX_TEMP);
                        }

                        //grass - ground
                        if (cell.getFuel() == CellFuel.GROUND && cell.getTemperature() < this.temperature){
                            double newTemp = heatTransfer(cell);
                            Debug.Log("GRASS - GROUND: " + newTemp);
                                if (newTemp < GROUND_MAX_TEMP)
                                    cell.setTemperature(newTemp);
                                else 
                                    cell.setTemperature(GROUND_MAX_TEMP);
                        }


                    }
                }
                    if (this.fuel == CellFuel.AIR){ 

                        this.heatTransferCoeff = this.heatTransferCoeff / (this.mass * this.specHeat); 
                        foreach (Cell cell in this.neighbours){

                            // air - tree
                            if (cell.getFuel() == CellFuel.TREE && cell.getTemperature() < this.temperature){   
                                double newTemp =  heatTransfer(cell);
                                Debug.Log("AIR - TREE:" + newTemp);
                                if (this.temperature < TREE_MAX_TEMP)
                                    cell.setTemperature(this.temperature);
                                else 
                                    cell.setTemperature(TREE_MAX_TEMP);
                        }

                            // air - grass
                            if (cell.getFuel() == CellFuel.GRASS && cell.getTemperature() < this.temperature){   
                                double newTemp =  heatTransfer(cell);
                                Debug.Log("AIR - GRASS:" + newTemp);
                                if (this.temperature < GRASS_MAX_TEMP)
                                    cell.setTemperature(this.temperature);
                                else 
                                    cell.setTemperature(GRASS_MAX_TEMP);
                        }
                    }

                }
            }
            
        }

        public void heatSpread(){

            if (this.fuel == CellFuel.GROUND){
            this.heatTransferCoeff = this.heatTransferCoeff / (this.mass * this.specHeat);
            foreach(Cell cell in neighbours){
                if (this.temperature > cell.getTemperature() && cell.getFuel() == CellFuel.GROUND){
                    double newTemp = heatTransfer(cell);
                    if (newTemp < GROUND_MAX_TEMP)
                        cell.setTemperature(newTemp);
                    else 
                        cell.setTemperature(GROUND_MAX_TEMP);
                    }
                }
            }

            if (this.fuel == CellFuel.AIR){
                 Convection();
            }
                
        }
        
        public void calculateMass(){
            this.mass = this.density * this.volume;
        }

        public double heatTransfer(Cell cell){
            return this.temperature + (this.heatTransferCoeff * (SURFACE / DIST)* (this.temperature - cell.getTemperature()));
        }

        public double radiate(Cell cell){
            return STEFAN_BOLTZMAN * 0.94 * SURFACE * ((Math.Pow(this.temperature, 4) - Math.Pow(cell.getTemperature(), 4)));
        }

        public void smokeSpread(){
            foreach(Cell cell in neighbours){

                if (cell.getFuel() == CellFuel.AIR){
                    float smokeTransfer;

                    // for convection (p. 57)
                    float dn = 0;

                    if (cell.getX() == this.x + 1 || cell.getX() == this.x - 1 || cell.getY() == this.y + 1 || cell.getY() == this.y - 1){
                        dn = 0.5f; 
                    }
                    else if(cell.getZ() == this.z + 1){
                        dn = 1f;
                    }

                    else if(cell.getZ() == this.z - 1){
                        dn = 0.25f; 
                    }
    
                    smokeTransfer = Math.Min((smokeVol/6), ((100 - cell.getSmokeVol())/6)); // p. 56
                    cell.addSmoke(dn * smokeTransfer);
                    removeSmoke(dn * smokeTransfer);
                }
            }


        }

        public void addSmoke(float smoke){

            this.smokeVol += smoke;
            if (this.smokeVol > 100){
                this.smokeVol = 100;
            }
            
        }

        public void calculateSmokeVol(){

            if (this.fuel == CellFuel.AIR)
                this.smokeVol = 0;
            else this.smokeVol = 100;
        }

        public float getSmokeVol(){
            return this.smokeVol;
        }

        public void removeSmoke(float smoke){
            this.smokeVol -= smoke;
            if (smokeVol < 0){
                this.smokeVol = 0;
            }
        }

        public void setTemperature(double temp){
            this.temperature = temp;
        }

        public void setType(CellType type){
            this.type = type;
        }
        public double getTemperature()
        {
            return this.temperature;
        }

        public CellType getType()
        {
            return this.type;
        }

        public CellFuel getFuel()
        {
            return this.fuel;
        }

        public int getFuelT() {
            return this.fuelT;
        }

        public int getX(){
            return this.x;
        }

        public int getY(){
            return this.y;
        }

        public int getZ(){
            return this.z;
        }




    }

}