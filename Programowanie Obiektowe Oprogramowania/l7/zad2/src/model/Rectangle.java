package model;

import java.awt.*;

public class Rectangle extends Figure {
    private int width, height;

    public Rectangle(int x, int y, int width, int height, Color color) {
        super(x, y, color);
        this.width = width;
        this.height = height;
    }

    public void draw(Graphics2D g) {
        g.setColor(color);
        g.fillRect(x - width / 2, y - height / 2, width, height);
    }

    public boolean contains(Point p) {
        return p.x >= x - width / 2 && p.x <= x + width / 2 &&
                p.y >= y - height / 2 && p.y <= y + height / 2;
    }

    public Figure clone() {
        return new Rectangle(x, y, width, height, color);
    }
}
