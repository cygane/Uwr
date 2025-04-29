package view;

import model.EditorState;
import model.Figure;

import javax.swing.*;
import java.awt.*;

public class CanvasPanel extends JPanel {
    private final EditorState editorState;

    public CanvasPanel(EditorState editorState) {
        this.editorState = editorState;
        setBackground(Color.WHITE);
    }

    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g);
        for (Figure f : editorState.getFigures()) {
            f.draw((Graphics2D) g);
        }
    }
}
