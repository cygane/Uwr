package core;

import java.util.*;

public class EventAggregator {
    private final Map<Class<?>, List<EventListener<?>>> listeners = new HashMap<>();

    public <T> void subscribe(Class<T> eventType, EventListener<T> listener) {
        listeners.computeIfAbsent(eventType, k -> new ArrayList<>()).add(listener);
    }

    @SuppressWarnings("unchecked")
    public <T> void publish(T event) {
        List<EventListener<?>> registered = listeners.get(event.getClass());
        if (registered != null) {
            for (EventListener<?> listener : registered) {
//                System.out.println("Publishing event: " + event.getClass().getName());
                ((EventListener<T>) listener).onEvent(event);
            }
        }
//        else{
//            System.out.println("No listeners for event: " + event.getClass().getName());
//        }
    }

    public interface EventListener<T> {
        void onEvent(T event);
    }
}
