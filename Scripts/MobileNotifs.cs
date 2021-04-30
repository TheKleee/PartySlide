using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class MobileNotifs : MonoBehaviour
{
    [Header("Test Mode:")]
    [SerializeField] private bool Test = true;    //Turn this off before building the project -.-

    private string[] notifText = new string[]
    {
        "Slide into the Fun!",
        "Time for a Good Fun!"
    };

    private string[] notifTitle = new string[]
    {
        "Hey! Let's Party!",
        "Hello There!"
    };
    void Start()
    {
        if (!Test)
        {
            //Remove displayed messages
            AndroidNotificationCenter.CancelAllDisplayedNotifications();

            //Random Generating:
            int textId = Random.Range(0, notifText.Length);
            string randText = notifText[textId];

            int titleId = Random.Range(0, notifTitle.Length);
            string randTitle = notifTitle[titleId];

            //Create Notification Channel :D
            var channel = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            //Setup >:)
            var notification = new AndroidNotification();
            notification.Title = randTitle;
            notification.Text = randText;
            notification.FireTime = System.DateTime.Now.AddHours(9);

            //Send the notification:
            var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");
            if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
            {
                AndroidNotificationCenter.CancelAllNotifications();
                AndroidNotificationCenter.SendNotification(notification, "channel_id");
            }
        }
    }
}
