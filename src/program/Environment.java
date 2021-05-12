package program;

public class Environment {
    private final int width;
    private final int height;
    private final int depth;
    private Cell[][][] cells;


    public Environment(int w, int h, int d) {
        this.width = w;
        this.height = h;
        this.depth = d;
        this.cells = new Cell[this.width][this.depth][this.height];
    }

    public void initialize() {

    }

    public void iterate(){

    }

    public int getDepth() {
        return this.depth;
    }

    public int getHeight() {
        return this.height;
    }

    public int getWidth() {
        return this.width;
    }
}
