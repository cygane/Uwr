package model;

import java.awt.*;
import java.util.*;
import java.util.List;

public class EditorState {
    private final List<Figure> figures = new ArrayList<>();
    private final Deque<List<Figure>> undoStack = new ArrayDeque<>();
    private final Deque<List<Figure>> redoStack = new ArrayDeque<>();

    public void addFigure(Figure f) {
        saveState();
        figures.add(f);
    }

    public void removeFigure(Figure f) {
        saveState();
        figures.remove(f);
    }

    public void recordMove(Figure f) {
        saveState();
    }

    public Optional<Figure> findFigureAt(Point p) {
        ListIterator<Figure> it = figures.listIterator(figures.size());
        while (it.hasPrevious()) {
            Figure f = it.previous();
            if (f.contains(p)) return Optional.of(f);
        }
        return Optional.empty();
    }

    public void undo() {
        if (!undoStack.isEmpty()) {
            redoStack.push(new ArrayList<>(figures));
            figures.clear();
            figures.addAll(undoStack.pop());
        }
    }

    public void redo() {
        if (!redoStack.isEmpty()) {
            undoStack.push(new ArrayList<>(figures));
            figures.clear();
            figures.addAll(redoStack.pop());
        }
    }

    private void saveState() {
        undoStack.push(new ArrayList<>(figures));
        redoStack.clear();
    }

    public List<Figure> getFigures() {
        return figures;
    }
}
