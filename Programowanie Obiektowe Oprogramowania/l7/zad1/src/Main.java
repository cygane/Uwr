import core.EventAggregator;

import javax.swing.*;
import view.MainFrame;

public class Main {
    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            EventAggregator aggregator = new EventAggregator();
            new MainFrame(aggregator);
        });
    }
}
