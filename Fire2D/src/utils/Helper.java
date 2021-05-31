package utils;

import java.util.Random;

public class  Helper{

  public static float nextFloatBetween2(float min, float max) {
      return (new Random().nextFloat() * (max - min)) + min;
  }

}
