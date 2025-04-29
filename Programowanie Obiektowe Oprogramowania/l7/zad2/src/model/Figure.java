package model;

import java.awt.*;

public abstract class Figure implements Cloneable {
    protected int x, y;
    protected Color color;

    public Figure(int x, int y, Color color) {
        this.x = x;
        this.y = y;
        this.color = color;
    }

    public abstract void draw(Graphics2D g);
    public abstract boolean contains(Point p);
    public abstract Figure clone();

    public int getX() { return x; }
    public int getY() { return y; }

    public void setPosition(int x, int y) {
        this.x = x;
        this.y = y;
    }
}