package view;

import javax.swing.*;
import java.awt.*;
import core.EventAggregator;

public class MainFrame extends JFrame {
    public MainFrame(EventAggregator aggregator) {
        setTitle("Rejestr");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(800, 600);
        setLayout(new BorderLayout());

        UserTreePanel treePanel = new UserTreePanel(aggregator);
        WorkPanel workPanel = new WorkPanel(aggregator, treePanel);

        JSplitPane splitPane = new JSplitPane(JSplitPane.HORIZONTAL_SPLIT, treePanel, workPanel);
        splitPane.setDividerLocation(200);
        add(splitPane);

        setVisible(true);
    }
}
