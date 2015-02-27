package pt.notificationhubssample;

import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import com.microsoft.windowsazure.notifications.NotificationsHandler;

import java.util.Date;

/**
 * Created by Carlos Martins NB20466 on 25/02/2015.
 */
public class PushNotificationHandler extends NotificationsHandler {

    private MainActivity baseActivity;

    public PushNotificationHandler() {
        super();
    }

    @Override
    public void onRegistered(Context context, String gcmRegistrationId) {
        super.onRegistered(context, gcmRegistrationId);
        baseActivity = (MainActivity) context;
        baseActivity.registerForPush(gcmRegistrationId);
    }

    @Override
    public void onReceive(Context context, Bundle bundle) {
        String receivedMessage = bundle.getString("message");
        Log.i("receivedmsg", receivedMessage);

        NotificationManager mNotificationManager = (NotificationManager) baseActivity.getSystemService(Context.NOTIFICATION_SERVICE);

        Intent activityIntent = new Intent(baseActivity, MainActivity.class);
        PendingIntent contentIntent = PendingIntent.getActivity(baseActivity, 0, activityIntent, PendingIntent.FLAG_ONE_SHOT);

        NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(baseActivity)
                .setSmallIcon(R.drawable.ic_launcher)
                .setAutoCancel(true)
                .setContentTitle("Notification Hubs Sample")
                .setStyle(new NotificationCompat.BigTextStyle()
                        .bigText(receivedMessage))
                .setContentText(receivedMessage);

        mBuilder.setContentIntent(contentIntent);

        // Generates an unique id for android to display multiple notifications
        int notificationId = (int) (new Date().getTime()/1000);
        mNotificationManager.notify(notificationId, mBuilder.build());
    }
}
