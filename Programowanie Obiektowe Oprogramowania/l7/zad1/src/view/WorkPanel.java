package view;

import core.EventAggregator;
import events.CategorySelectedEvent;
import events.UserAddedEvent;
import events.UserUpdatedEvent;
import models.User;

import javax.swing.*;
import javax.swing.tree.DefaultMutableTreeNode;
import java.awt.*;

public class WorkPanel extends JPanel {
    private final UserTreePanel treePanel;
    private CardLayout cardLayout = new CardLayout();
    private JPanel cardPanel = new JPanel(cardLayout);
    private EventAggregator aggregator;

    public WorkPanel(EventAggregator aggregator, UserTreePanel treePanel) {
        this.aggregator = aggregator;
        this.treePanel = treePanel;

        add(cardPanel, BorderLayout.CENTER);
        setLayout(new BorderLayout());

        aggregator.subscribe(User.class, this::showUserDetails);
        aggregator.subscribe(String.class, this::showUserList);
        aggregator.subscribe(CategorySelectedEvent.class, event -> showUserList(event.getCategoryName()));
        aggregator.subscribe(UserAddedEvent.class, event -> showUserList(event.getCategory()));
        aggregator.subscribe(UserUpdatedEvent.class, event -> showUserDetails(event.getUser()));
    }

    private void showUserList(String category) {
        JPanel panel = new JPanel(new BorderLayout());
        panel.add(new JLabel("Użytkownicy w kategorii: " + category), BorderLayout.NORTH);

        DefaultMutableTreeNode categoryNode = treePanel.getCategoryNode(category);
        JPanel userListPanel = new JPanel();
        userListPanel.setLayout(new BoxLayout(userListPanel, BoxLayout.Y_AXIS));

        if (categoryNode != null) {
            for (int i = 0; i < categoryNode.getChildCount(); i++) {
                DefaultMutableTreeNode childNode = (DefaultMutableTreeNode) categoryNode.getChildAt(i);
                Object obj = childNode.getUserObject();
                if (obj instanceof User user) {
                    JLabel userLabel = new JLabel(user.getName() + " - " + user.getEmail());
                    userListPanel.add(userLabel);
                }
            }
        } else {
            userListPanel.add(new JLabel("Brak użytkowników."));
        }

        panel.add(new JScrollPane(userListPanel), BorderLayout.CENTER);

        JButton addButton = new JButton("Dodaj");
        addButton.addActionListener(e -> {
            DefaultMutableTreeNode node = treePanel.getCategoryNode(category);
            new UserFormDialog(null, false, aggregator, category, node);
        });
        panel.add(addButton, BorderLayout.SOUTH);

        cardPanel.add(panel, "userList");
        cardLayout.show(cardPanel, "userList");
    }

    private void showUserDetails(User user) {
        System.out.println("showUserDetails called for: " + user.getName());
        JPanel panel = new JPanel(new BorderLayout());
        panel.add(new JLabel("Imię: " + user.getName()), BorderLayout.NORTH);
        panel.add(new JLabel("Email: " + user.getEmail()), BorderLayout.CENTER);

        JButton editButton = new JButton("Zmień");
        DefaultMutableTreeNode toChange = new DefaultMutableTreeNode(user);
        editButton.addActionListener(e -> new UserFormDialog(user, true, aggregator, user.getCategory(), toChange));
        panel.add(editButton, BorderLayout.SOUTH);

//        System.out.println("Adding userDetails panel");

        add(cardPanel, BorderLayout.CENTER);
        cardPanel.add(panel, "userDetails");
        cardLayout.show(cardPanel, "userDetails");
        cardPanel.revalidate();
        cardPanel.repaint();
    }
}
