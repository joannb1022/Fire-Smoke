package program;
import javax.swing.JFrame;


import utils.CellFuel;
import utils.CellType;


public class Program extends JFrame {

	private static final long serialVersionUID = 1L;
	private GUI gof;

	public Program() {
		setTitle("Fire simulation");
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		gof = new GUI(this);
		gof.initialize(this.getContentPane());
		this.setSize(1024, 768);
		this.setVisible(true);
	}

	public static void main(String[] args) {
		new Program();
	}
}
