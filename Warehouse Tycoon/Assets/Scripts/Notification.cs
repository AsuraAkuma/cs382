using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NotificationController : MonoBehaviour
{
    public static NotificationController instance; // Singleton instance of the NotificationController
    public UIDocument notificationUI; // UI element for displaying notifications

    private void OnEnable()
    {
        // Ensure that the singleton instance is set when the object is enabled
        if (instance == null)
        {
            instance = this; // Assign the singleton instance
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

    }

    private void Start()
    {
        // Initialize notifications list in Globals if it doesn't exist
        if (Globals.notifications == null)
        {
            Globals.notifications = new List<Notification>();
        }
    }
    private void FixedUpdate()
    {
        // Check for notifications and update their visibility
        if (Globals.notifications.Count > 0)
        {
            foreach (Notification notification in Globals.notifications)
            {
                if (notification.isVisible == false)
                {
                    notification.isVisible = true; // Set the notification to visible
                    Debug.Log(notification.message); // Display the notification message in the console
                }
            }
        }
    }

    public void CreateNotification(string message)
    {
        // Create a new notification and add it to the list
        Notification notification = new Notification(message);
        Globals.notifications.Add(notification); // Add the notification to the list
        notification.showNotification(notificationUI); // Show the notification in the UI
    }
}
public class Notification
{
    public string message; // Message to be displayed in the notification
    public bool isVisible; // Flag to indicate if the notification is currently visible

    // Constructor to initialize a Notification object
    public Notification(string message)
    {
        this.message = message;
        isVisible = false; // Default visibility is false
    }
    public static void addNotification(string message)
    {
        Notification notification = new Notification(message);
        Globals.notifications.Add(notification);
    }
    public static void removeNotification(Notification notification)
    {
        if (Globals.notifications.Contains(notification))
        {
            Globals.notifications.Remove(notification); // Remove the notification from the list
        }
    }
    public static void clearNotifications()
    {
        Globals.notifications.Clear(); // Clear all notifications from the list
    }
    public void showNotification(UIDocument notificationUI)
    {
        // Get the notification container from the UI document
        VisualElement notificationContainer = notificationUI.rootVisualElement.Q<VisualElement>("NotificationContainer");
        // Create a new notification element
        VisualElement notificationElement = new VisualElement();
        notificationElement.AddToClassList("notification"); // Add a class to the notification element for styling
        notificationContainer.Add(notificationElement); // Add the notification element to the container
        notificationElement.Add(new Label(message)); // Add the message label to the notification element
        isVisible = true; // Set the notification to visible
        // Create timeout to remove second after 10 seconds
        notificationElement.RegisterCallback<ClickEvent>(ev => RemoveNotification(notificationElement)); // Register a click event to remove the notification when clicked
        // Set a timeout to remove the notification after 10 seconds
        notificationUI.StartCoroutine(RemoveAfterDelay(notificationElement, 10f));
    }

    private IEnumerator RemoveAfterDelay(VisualElement notificationElement, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        RemoveNotification(notificationElement); // Remove the notification element
    }
    private void RemoveNotification(VisualElement notificationElement)
    {
        if (notificationElement == null) return; // Check if the notification element is null
        notificationElement.RemoveFromHierarchy(); // Remove the notification element from the hierarchy
        isVisible = false; // Set the notification to not visible
        // Remove the notification from the list in Globals
        if (Globals.notifications.Contains(this))
        {
            Globals.notifications.Remove(this); // Remove the notification from the list
        }
    }
}