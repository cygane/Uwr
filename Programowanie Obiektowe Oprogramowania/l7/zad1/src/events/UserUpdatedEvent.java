package events;

import models.User;
import javax.swing.tree.DefaultMutableTreeNode;

public class UserUpdatedEvent {
    private final User user;
    private final DefaultMutableTreeNode userNode;

    public UserUpdatedEvent(User user, DefaultMutableTreeNode userNode) {
        this.user = user;
        this.userNode = userNode;
    }

    public User getUser() { return user; }
    public DefaultMutableTreeNode getUserNode() { return userNode; }
}
