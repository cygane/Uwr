package view;

import core.EventAggregator;
import events.UserAddedEvent;
import events.UserUpdatedEvent;
import models.User;

import javax.swing.*;
import javax.swing.tree.DefaultMutableTreeNode;
import java.awt.*;

public class UserFormDialog extends JDialog {

    public UserFormDialog(User user, boolean isEditMode,
                          EventAggregator aggregator,
                          String category,
                          DefaultMutableTreeNode nodeTarget) {

        setTitle(isEditMode ? "Edytuj użytkownika" : "Dodaj użytkownika");
        setModal(true);
        setLayout(new GridLayout(4, 2));

        JTextField nameField = new JTextField(user != null ? user.getName() : "");
        JTextField emailField = new JTextField(user != null ? user.getEmail() : "");
        JTextField categoryField = new JTextField(user != null ? user.getCategory() : category);

        add(new JLabel("Imię:"));
        add(nameField);
        add(new JLabel("Email:"));
        add(emailField);
        add(new JLabel("Kategoria:"));
        add(categoryField);

        JButton saveBtn = new JButton("Zapisz");
        saveBtn.addActionListener(e -> {
            if (isEditMode) {
                user.setName(nameField.getText());
                user.setEmail(emailField.getText());
                user.setCategory(categoryField.getText());
                aggregator.publish(new UserUpdatedEvent(user, nodeTarget));
            } else {
                User newUser = new User(nameField.getText(), emailField.getText(), categoryField.getText());
                aggregator.publish(new UserAddedEvent(category, newUser, nodeTarget));
            }
            dispose();
        });

        JButton cancelBtn = new JButton("Anuluj");
        cancelBtn.addActionListener(e -> dispose());

        add(saveBtn);
        add(cancelBtn);

        pack();
        setLocationRelativeTo(null);
        setVisible(true);
    }
}
