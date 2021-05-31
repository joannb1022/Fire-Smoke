package utils;

public class Wind{

  public static float[][] windMatrix(WindDir dir){

    float[][] windMatrix = {{0,0,0}, {0,0,0}, {0,0,0}};

      switch (dir){
        case N:
          windMatrix[0][0] = 1;
          windMatrix[0][1] = 1;
          windMatrix[0][2] = 1;
        case W:
          windMatrix[0][0] = 1;
          windMatrix[1][0] = 1;
          windMatrix[2][0] = 1;
      }

      return windMatrix;
}

}
