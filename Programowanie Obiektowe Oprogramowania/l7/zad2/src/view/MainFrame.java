package view;

import controller.CanvasController;
import model.EditorState;

import javax.swing.*;
import java.awt.*;

public class MainFrame extends JFrame {
    public MainFrame() {
        setTitle("Edytor graficzny");
        setSize(800, 600);
        setDefaultCloseOperation(EXIT_ON_CLOSE);

        EditorState state = new EditorState();
        CanvasPanel canvas = new CanvasPanel(state);
        CanvasController controller = new CanvasController(state, canvas);
        ToolBarPanel toolbar = new ToolBarPanel(controller, state, canvas);

        add(toolbar, BorderLayout.NORTH);
        add(canvas, BorderLayout.CENTER);
        setVisible(true);
    }
}