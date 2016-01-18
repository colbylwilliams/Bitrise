using Foundation;
using UIKit;
using Parse;

namespace Bitrise.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window { get; set; }

		public AppDelegate ()
		{
			ParseClient.Initialize(PrivateKeys.ParseKeys.ApplicationId, PrivateKeys.ParseKeys.DotNetKey);
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// Settings.RegisterDefaultSettings();

			// Bootstrap.Run();

			registerForRemoteNotifications();

			return true;
		}

		void HandleParsePushNotificationReceived (object sender, ParsePushNotificationEventArgs e)
		{

		}

		void registerForRemoteNotifications ()
		{
			// Register for Push Notitications
			UIUserNotificationType notificationTypes = (UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound);

			var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(notificationTypes, new NSSet (new string[] { }));

			UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);
			UIApplication.SharedApplication.RegisterForRemoteNotifications();

			// Handle Push Notifications
			ParsePush.ParsePushNotificationReceived += HandleParsePushNotificationReceived;
		}

		public override void DidRegisterUserNotificationSettings (UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			application.RegisterForRemoteNotifications();
		}

		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			ParseInstallation installation = ParseInstallation.CurrentInstallation;

			installation.SetDeviceTokenFromData(deviceToken);

			installation.SaveAsync();
		}

		public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
		{
			// We need this to fire userInfo into ParsePushNotificationReceived.
			ParsePush.HandlePush(userInfo);
		}
	}
}