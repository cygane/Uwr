package model;

import java.awt.*;

public class Circle extends Figure {
    private int radius;

    public Circle(int x, int y, int radius, Color color) {
        super(x, y, color);
        this.radius = radius;
    }

    public void draw(Graphics2D g) {
        g.setColor(color);
        g.fillOval(x - radius, y - radius, radius * 2, radius * 2);
    }

    public boolean contains(Point p) {
        int dx = p.x - x;
        int dy = p.y - y;
        return dx * dx + dy * dy <= radius * radius;
    }

    public Figure clone() {
        return new Circle(x, y, radius, color);
    }
}
