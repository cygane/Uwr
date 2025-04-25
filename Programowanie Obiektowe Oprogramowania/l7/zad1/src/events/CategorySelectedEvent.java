package events;

public class CategorySelectedEvent {
    private final String categoryName;

    public CategorySelectedEvent(String categoryName) {
        this.categoryName = categoryName;
    }

    public String getCategoryName() {
        return categoryName;
    }
}

