package program;
import utils.CellFuel;
import utils.CellType;
import utils.WindDir;
import utils.Wind;

import javax.swing.*;
import javax.swing.event.MouseInputListener;
import java.awt.*;
import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.MouseEvent;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

import static java.util.stream.Collectors.toList;

public class Board extends JComponent implements MouseInputListener, ComponentListener {
    private static final long serialVersionUID = 1L;
    private Cell[][] cells;
    private final int size = 10;
    public CellFuel editType;
    private float temperature = 10;
    private Cell[][] burningCells; //nie wiem czy to sie przyda
    public WindDir windDir;


    public Board(int length, int height) {
        addMouseListener(this);
        addComponentListener(this);
        addMouseMotionListener(this);
        setBackground(Color.WHITE);
        setOpaque(true);
        initialize(length, height);
    }


    public void iteration() {
      for (int x = 0; x < cells.length; ++x) {
          for (int y = 0; y < cells[x].length; ++y)
            if (cells[x][y].getType() == CellType.BURNING)
                cells[x][y].calculateProbablityMatrix();
      }

      for (int x = 0; x < cells.length; ++x) {
          for (int y = 0; y < cells[x].length; ++y)
              if (cells[x][y].getType() == CellType.BURNING) {
                  cells[x][y].fireSpread();
              }
              // else{
              //bo to cieplo tez musi jakos miedzy tymi niepalacymi sie
              //   cells[x][y].tempRise();
              // }
      }
        for (int x = 0; x < cells.length; ++x) {
            for (int y = 0; y < cells[x].length; ++y) {
                cells[x][y].checkState();
            }
        }
      this.repaint();
    }

    public void firstIteration(){
      for (int x = 0; x < cells.length; ++x) {
          for (int y = 0; y < cells[x].length; ++y) {
              cells[x][y].setWindMatrix(Wind.windMatrix(windDir));
          }
      }

      this.iteration();

    }

    public void clear() {
        for (int x = 0; x < cells.length; ++x)
            for (int y = 0; y < cells[x].length; ++y) {
                cells[x][y].clear();
            }
        this.repaint();
    }


    private void initialize(int length, int height) {
        cells = new Cell[length][height];

        for (int x = 0; x < cells.length; ++x)
            for (int y = 0; y < cells[x].length; ++y)
                cells[x][y] = new Cell(this.temperature, this.windDir, x, y);

        for (int x = 0; x < cells.length; ++x)
            for (int y = 0; y < cells[x].length; ++y)
                for (int i = x - 1; i <= x + 1; i++) {
                    for (int j = y - 1; j <= y + 1; j++) {
                        if (i == -1 || j == -1 || i == cells.length || j == cells[x].length || (i == x && y == j))
                            continue;
                        cells[x][y].addNeighbour(cells[i][j]);

                }
              }
    }


    protected void paintComponent(Graphics g) {
        if (isOpaque()) {
            g.setColor(getBackground());
            g.fillRect(0, 0, this.getWidth(), this.getHeight());
        }
        g.setColor(Color.GRAY);
        drawNetting(g, size);
    }

    private void drawNetting(Graphics g, int gridSpace) {
        Insets insets = getInsets();
        int firstX = insets.left;
        int firstY = insets.top;
        int lastX = this.getWidth() - insets.right;
        int lastY = this.getHeight() - insets.bottom;

        int x = firstX;
        while (x < lastX) {
            g.drawLine(x, firstY, x, lastY);
            x += gridSpace;
        }

        int y = firstY;
        while (y < lastY) {
            g.drawLine(firstX, y, lastX, y);
            y += gridSpace;
        }

        for (x = 0; x < cells.length; ++x) {
            for (y = 0; y < cells[x].length; ++y) {
                switch(cells[x][y].getFuel()){
                  case TREE:
                    g.setColor(new Color(0x00841B));
                    break;
                  case GRASS:
                      g.setColor(new Color(0x56CE00));
                       break;
                  case GROUND:
                    g.setColor(new Color(0x4B2821));
                    break;
                  case FIRE:
                      if(cells[x][y].getTemperature() > 0 && cells[x][y].getTemperature() <= 200){
                          g.setColor(new Color(0xEFCC78));
                      }
                      else if(cells[x][y].getTemperature() > 200 && cells[x][y].getTemperature() <= 400){
                          g.setColor(new Color(0xF8C302));
                      }
                      else if(cells[x][y].getTemperature() > 400 && cells[x][y].getTemperature() <= 600){
                          g.setColor(new Color(0xFFA201));
                      }
                      else if(cells[x][y].getTemperature() > 600 && cells[x][y].getTemperature() <= 800){
                          g.setColor(new Color(0xFF7700));
                      }
                      else if(cells[x][y].getTemperature() > 1000 && cells[x][y].getTemperature() <= 1200){
                          g.setColor(new Color(0xFF692B));
                      }
                      else if(cells[x][y].getTemperature() > 1200){
                          g.setColor(new Color(0xE73700));
                      }
                    break;
                }

                g.fillRect((x * size) + 1, (y * size) + 1, (size - 1), (size - 1));
            }
        }

    }

    public void mouseClicked(MouseEvent e) {
        int x = e.getX() / size;
        int y = e.getY() / size;
        if ((x < cells.length) && (x > 0) && (y < cells[x].length) && (y > 0)) {
          cells[x][y].setFuel(editType);
            this.repaint();
        }
    }

    public void componentResized(ComponentEvent e) {
        int dlugosc = (this.getWidth() / size) + 1;
        int wysokosc = (this.getHeight() / size) + 1;
        initialize(dlugosc, wysokosc);
    }

    public void mouseDragged(MouseEvent e) {
        int x = e.getX() / size;
        int y = e.getY() / size;
        if ((x < cells.length) && (x > 0) && (y < cells[x].length) && (y > 0)) {
              cells[x][y].setFuel(editType);

            this.repaint();
        }
    }

    public void mouseExited(MouseEvent e) {
    }

    public void mouseEntered(MouseEvent e) {
    }

    public void componentShown(ComponentEvent e) {
    }

    public void componentMoved(ComponentEvent e) {
    }

    public void mouseReleased(MouseEvent e) {
    }

    public void mouseMoved(MouseEvent e) {
    }

    public void componentHidden(ComponentEvent e) {
    }

    public void mousePressed(MouseEvent e) {
    }

}
