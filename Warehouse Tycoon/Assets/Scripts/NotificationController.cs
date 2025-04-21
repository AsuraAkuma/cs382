using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NotificationController : MonoBehaviour
{
    public UIDocument notificationUI; // UI element for displaying notifications

    void Start()
    {
        // Initialize notifications list in Globals if it doesn't exist
        if (Globals.notifications == null)
        {
            Globals.notifications = new List<Notification>();
        }
    }
    void FixedUpdate()
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
