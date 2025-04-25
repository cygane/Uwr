package models;

public class User {
    private String name;
    private String email;
    private String category;  // Dodane pole dla kategorii

    public User(String name, String email, String category) {
        this.name = name;
        this.email = email;
        this.category = category;
    }

    // Gettery i settery
    public String getName() { return name; }
    public String getEmail() { return email; }
    public String getCategory() { return category; }

    public void setName(String name) { this.name = name; }
    public void setEmail(String email) { this.email = email; }
    public void setCategory(String category) { this.category = category; }

    @Override
    public String toString() {
        return name + " (" + category + ")";
    }
}
