Imports System.Diagnostics
Imports Microsoft.WindowsAzure.Messaging
Imports Microsoft.Phone.Notification
Imports System.Text

''' <summary>
''' Define the NotificationService.
''' </summary>
Public Class NotificationService
    ''' <summary>
    ''' Creates the or update notifications asynchronous.
    ''' </summary>
    Public Sub CreateOrUpdateNotificationsAsync()
        Dim channel = HttpNotificationChannel.Find("MyPushChannel")

        If channel Is Nothing Then
            channel = New HttpNotificationChannel("MyPushChannel")
            channel.Open()
            channel.BindToShellToast()

            AddHandler channel.ErrorOccurred, AddressOf Channel_ErrorOccurred
            AddHandler channel.HttpNotificationReceived, AddressOf Channel_HttpNotificationReceived
            AddHandler channel.ShellToastNotificationReceived, AddressOf Channel_ShellToastNotificationReceived
            AddHandler channel.ChannelUriUpdated, AddressOf Channel_ChannelUriUpdated
        Else
            AddHandler channel.ErrorOccurred, AddressOf Channel_ErrorOccurred
            AddHandler channel.HttpNotificationReceived, AddressOf Channel_HttpNotificationReceived
            AddHandler channel.ShellToastNotificationReceived, AddressOf Channel_ShellToastNotificationReceived
            AddHandler channel.ChannelUriUpdated, AddressOf Channel_ChannelUriUpdated

            Debug.WriteLine(channel.ChannelUri.ToString())
        End If
    End Sub

    ''' <summary>
    ''' Handles the ChannelUriUpdated event of the Channel control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="NotificationChannelUriEventArgs"/> instance containing the event data.</param>
    Private Async Sub Channel_ChannelUriUpdated(sender As Object, e As NotificationChannelUriEventArgs)
        Try
            Dim hub = New NotificationHub(Constants.HubName, Constants.ConnectionString)
            
            Debug.WriteLine(e.ChannelUri.ToString())

            ' register the channel in NH without tags
            'var result = await hub.RegisterNativeAsync(e.ChannelUri.ToString());

            Dim result = Await hub.RegisterNativeAsync(e.ChannelUri.ToString(), SettingsHelper.Tags)


            If result.RegistrationId IsNot Nothing Then
                Deployment.Current.Dispatcher.BeginInvoke(Function()
                                                              MessageBox.Show(result.RegistrationId)

                                                          End Function)
            End If
        Catch exception As Exception
            MessageBox.Show(exception.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Handles the ShellToastNotificationReceived event of the Channel control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="NotificationEventArgs"/> instance containing the event data.</param>
    Private Sub Channel_ShellToastNotificationReceived(sender As Object, e As NotificationEventArgs)
        Dim message As New StringBuilder()

        message.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString())

        ' Parse out the information that was part of the message.
        For Each key As String In e.Collection.Keys
            message.AppendFormat("{0}: {1}\n", key, e.Collection(key))

            If String.Compare(key, "wp:Param", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.CompareOptions.IgnoreCase) = 0 Then
            End If
        Next
        Deployment.Current.Dispatcher.BeginInvoke(Function()

                                                            MessageBox.Show(message.ToString())

                                                        End Function)
    End Sub

    ''' <summary>
    ''' Handles the ErrorOccurred event of the Channel control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="NotificationChannelErrorEventArgs"/> instance containing the event data.</param>
    Private Sub Channel_ErrorOccurred(sender As Object, e As NotificationChannelErrorEventArgs)
        Debug.WriteLine(e.Message)
    End Sub

    ''' <summary>
    ''' Handles the HttpNotificationReceived event of the Channel control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="HttpNotificationEventArgs"/> instance containing the event data.</param>
    Private Sub Channel_HttpNotificationReceived(sender As Object, e As HttpNotificationEventArgs)
        Debug.WriteLine(e.Notification)
    End Sub
End Class