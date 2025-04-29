package controller;

import model.*;
import model.Rectangle;
import view.CanvasPanel;

import java.awt.*;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.util.Optional;

public class CanvasController {
    private final EditorState editorState;
    private final CanvasPanel canvasPanel;
    private ToolMode mode = ToolMode.ADD_CIRCLE;
    private Figure prototype = new Circle(0, 0, 40, Color.RED);
    private Figure selectedFigure = null;
    private Point dragOffset;

    public CanvasController(EditorState editorState, CanvasPanel canvasPanel) {
        this.editorState = editorState;
        this.canvasPanel = canvasPanel;
        setupListeners();
    }

    public void setMode(ToolMode mode) {
        this.mode = mode;
        switch (mode) {
            case ADD_CIRCLE -> prototype = new Circle(0, 0, 40, Color.RED);
            case ADD_SQUARE -> prototype = new Square(0, 0, 40, Color.BLUE);
            case ADD_RECTANGLE -> prototype = new Rectangle(0, 0, 60, 40, Color.GREEN);
        }
    }

    private void setupListeners() {
        canvasPanel.addMouseListener(new MouseAdapter() {
            public void mousePressed(MouseEvent e) {
                Point p = e.getPoint();
                switch (mode) {
                    case ADD_CIRCLE, ADD_SQUARE, ADD_RECTANGLE -> {
                        Figure newFigure = prototype.clone();
                        newFigure.setPosition(p.x, p.y);
                        editorState.addFigure(newFigure);
                        canvasPanel.repaint();
                    }
                    case DELETE -> {
                        Optional<Figure> fig = editorState.findFigureAt(p);
                        fig.ifPresent(f -> {
                            editorState.removeFigure(f);
                            canvasPanel.repaint();
                        });
                    }
                    case MOVE -> {
                        Optional<Figure> fig = editorState.findFigureAt(p);
                        fig.ifPresent(f -> {
                            selectedFigure = f;
                            dragOffset = new Point(p.x - f.getX(), p.y - f.getY());
                        });
                    }
                }
            }

            public void mouseReleased(MouseEvent e) {
                selectedFigure = null;
                dragOffset = null;
            }
        });

        canvasPanel.addMouseMotionListener(new MouseAdapter() {
            public void mouseDragged(MouseEvent e) {
                if (mode == ToolMode.MOVE && selectedFigure != null && dragOffset != null) {
                    selectedFigure.setPosition(e.getX() - dragOffset.x, e.getY() - dragOffset.y);
                    editorState.recordMove(selectedFigure);
                    canvasPanel.repaint();
                }
            }
        });
    }
}
