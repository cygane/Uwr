package events;

import models.User;
import javax.swing.tree.DefaultMutableTreeNode;

public class UserAddedEvent {
    private final String category;
    private final User user;
    private final DefaultMutableTreeNode categoryNode;

    public UserAddedEvent(String category, User user, DefaultMutableTreeNode categoryNode) {
        this.category = category;
        this.user = user;
        this.categoryNode = categoryNode;
    }

    public String getCategory() { return category; }
    public User getUser() { return user; }
    public DefaultMutableTreeNode getCategoryNode() { return categoryNode; }
}

