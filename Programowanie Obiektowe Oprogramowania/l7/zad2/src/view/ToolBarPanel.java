package view;

import controller.CanvasController;
import model.EditorState;
import model.ToolMode;

import javax.swing.*;
import java.awt.event.ActionEvent;

public class ToolBarPanel extends JPanel {
    public ToolBarPanel(CanvasController controller, EditorState state, CanvasPanel canvas) {
        JButton circleBtn = new JButton("Koło");
        JButton squareBtn = new JButton("Kwadrat");
        JButton rectBtn = new JButton("Prostokąt");
        JButton moveBtn = new JButton("Przesuwaj");
        JButton deleteBtn = new JButton("Usuń");
        JButton undoBtn = new JButton("Undo");
        JButton redoBtn = new JButton("Redo");

        circleBtn.addActionListener(e -> controller.setMode(ToolMode.ADD_CIRCLE));
        squareBtn.addActionListener(e -> controller.setMode(ToolMode.ADD_SQUARE));
        rectBtn.addActionListener(e -> controller.setMode(ToolMode.ADD_RECTANGLE));
        moveBtn.addActionListener(e -> controller.setMode(ToolMode.MOVE));
        deleteBtn.addActionListener(e -> controller.setMode(ToolMode.DELETE));
        undoBtn.addActionListener(e -> { state.undo(); canvas.repaint(); });
        redoBtn.addActionListener(e -> { state.redo(); canvas.repaint(); });

        add(circleBtn);
        add(squareBtn);
        add(rectBtn);
        add(moveBtn);
        add(deleteBtn);
        add(undoBtn);
        add(redoBtn);
    }
}
