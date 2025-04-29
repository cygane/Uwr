package model;

import java.awt.*;

public class Square extends Figure {
    private int size;

    public Square(int x, int y, int size, Color color) {
        super(x, y, color);
        this.size = size;
    }

    public void draw(Graphics2D g) {
        g.setColor(color);
        g.fillRect(x - size / 2, y - size / 2, size, size);
    }

    public boolean contains(Point p) {
        return p.x >= x - size / 2 && p.x <= x + size / 2 &&
                p.y >= y - size / 2 && p.y <= y + size / 2;
    }

    public Figure clone() {
        return new Square(x, y, size, color);
    }
}
