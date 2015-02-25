package pt.notificationhubssample;

import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;
import com.google.gson.Gson;

import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.MobileServiceTable;
import com.microsoft.windowsazure.mobileservices.Registration;
import com.microsoft.windowsazure.mobileservices.RegistrationCallback;
import com.microsoft.windowsazure.mobileservices.ServiceFilterResponse;
import com.microsoft.windowsazure.mobileservices.TableOperationCallback;
import com.microsoft.windowsazure.mobileservices.UnregisterCallback;
import com.microsoft.windowsazure.notifications.NotificationsManager;

import java.net.MalformedURLException;


public class MainActivity extends ActionBarActivity {

    private Button insertCarBtn;
    public MobileServiceClient mClient;
    public MobileServiceTable carTable;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        configureAzure();
        configureNotifications();


        // Car button
        insertCarBtn = (Button) findViewById(R.id.insertCarButton);
        insertCarBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                insertAzure();
            }
        });

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    private void configureAzure() {
        try {
            mClient = new MobileServiceClient(
                    "<the endpoint>",
                    "<the key>",
                    MainActivity.this);
        } catch (MalformedURLException e) {
            Log.v("MobileServices", "Creating mobile service client error");
        }

        carTable = mClient.getTable("Car", Car.class);

    }

    private void insertAzure() {
        Car newCar = new Car();
        newCar.name = "Red Car";
        newCar.isElectric = false;

        carTable.insert(newCar, new TableOperationCallback<Car>() {
            @Override
            public void onCompleted(Car entity, Exception exception, ServiceFilterResponse response) {
                if (exception == null) {
                    Toast.makeText(MainActivity.this, "Car sent", Toast.LENGTH_LONG).show();
                } else {
                    Log.e("Azure", exception.getLocalizedMessage());
                }
            }
        });
    }

    private void configureNotifications() {
        NotificationsManager.handleNotifications(this, "<Sender ID>", PushNotificationHandler.class);
    }

    /**
     * Registers mobile services client to receive GCM push notifications
     * @param gcmRegistrationId The Google Cloud Messaging session Id returned
     * by the call to GoogleCloudMessaging.register in NotificationsManager.handleNotifications
     */
    public void registerForPush(String gcmRegistrationId) {
        String[] tags = new String[] {};

        // Register device for push notifications
        mClient.getPush().register(gcmRegistrationId, tags, new RegistrationCallback() {
            @Override
            public void onRegister(Registration registration, Exception exception) {
                if (exception != null) {
                    // handle exception
                } else {
                    Log.i("notification", "Registration ID: " + registration.getRegistrationId());
                }
            }
        });
    }
}
