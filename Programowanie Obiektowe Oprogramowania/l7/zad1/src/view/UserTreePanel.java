package view;
import javax.swing.*;
import javax.swing.tree.*;
import java.awt.*;
import java.awt.event.*;
import core.EventAggregator;
import events.CategorySelectedEvent;
import events.UserAddedEvent;
import events.UserUpdatedEvent;
import models.User;

public class UserTreePanel extends JPanel {
    private JTree tree;
    private DefaultTreeModel treeModel;
    private EventAggregator aggregator;
    private final DefaultMutableTreeNode studentsNode;
    private final DefaultMutableTreeNode professorsNode;

    public UserTreePanel(EventAggregator aggregator) {
        this.aggregator = aggregator;


        DefaultMutableTreeNode root = new DefaultMutableTreeNode("Root");
        DefaultMutableTreeNode students = new DefaultMutableTreeNode("students");
        DefaultMutableTreeNode professors = new DefaultMutableTreeNode("professors");

        this.studentsNode = students;
        this.professorsNode = professors;

        root.add(students);
        root.add(professors);

        students.add(new DefaultMutableTreeNode(new User("Jan Kowalski", "jan@example.com", "students")));
        professors.add(new DefaultMutableTreeNode(new User("Krystian Kowalski", "jan@example.com", "professors")));

        treeModel = new DefaultTreeModel(root);
        tree = new JTree(treeModel);
        tree.setRootVisible(false);

        aggregator.subscribe(UserAddedEvent.class, event -> {
            DefaultMutableTreeNode categoryNode = event.getCategoryNode();
            DefaultMutableTreeNode newNode = new DefaultMutableTreeNode(event.getUser());

            if (categoryNode != null) {
                treeModel.insertNodeInto(newNode, categoryNode, categoryNode.getChildCount());
                TreePath path = new TreePath(newNode.getPath());
                tree.scrollPathToVisible(path);
            }
        });

        aggregator.subscribe(UserUpdatedEvent.class, event -> {
            event.getUserNode().setUserObject(event.getUser());
            treeModel.nodeChanged(event.getUserNode());
        });

        tree.addTreeSelectionListener(e -> {
            DefaultMutableTreeNode selected = (DefaultMutableTreeNode) tree.getLastSelectedPathComponent();
            if (selected == null) return;
            Object obj = selected.getUserObject();

            if (obj instanceof User user) {
//                System.out.println("User clicked: " + user.getName());
                aggregator.publish(user);
            } else if (obj instanceof String categoryName) {
//                System.out.println("Category clicked: " + categoryName);
                aggregator.publish(new CategorySelectedEvent(categoryName));
            }
            tree.repaint();

        });


        setLayout(new BorderLayout());
        add(new JScrollPane(tree), BorderLayout.CENTER);
    }

    public DefaultMutableTreeNode getCategoryNode(String category) {
        return switch (category) {
            case "students" -> studentsNode;
            case "professors" -> professorsNode;
            default -> null;
        };
    }

}
