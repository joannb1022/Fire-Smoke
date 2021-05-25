package program;
import utils.CellFuel;
import utils.CellType;

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
    private float temperature;
    private Cell[][] burningCells; //nie wiem czy to sie przyda


    public Board(int length, int height) {
        addMouseListener(this);
        addComponentListener(this);
        addMouseMotionListener(this);
        setBackground(Color.WHITE);
        setOpaque(true);
        initialize(length, height);
    }


    public void iteration() {
      for (int x = 0; x < cells.length; ++x)
          for (int y = 0; y < cells[x].length; ++y)
              if (cells[x][y].getType() == CellType.BURNING){
                cells[x][y].fireSpread();
              }

        for (int x = 0; x < cells.length; ++x)
            for (int y = 0; y < cells[x].length; ++y)
                      cells[x][y].checkState();


      this.repaint();
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
                cells[x][y] = new Cell(temperature);

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
                    g.setColor(new Color(0x0c7d18));
                    break;
                  case GROUND:
                    g.setColor(new Color(0x1c0900));
                    break;
                  case FIRE:
                    g.setColor(new Color(0xe4b200));
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
