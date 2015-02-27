Imports Windows.Networking.PushNotifications
Imports Windows.UI.Notifications
Imports Microsoft.WindowsAzure.Messaging
Imports Windows.UI.Popups

Public Class NotificationService
    ''' <summary>
    ''' The create or update notifications async.
    ''' </summary>
    Public Shared Async Sub CreateOrUpdateNotificationsAsync()
        Try
            ' get the channel for the app
            Dim pushNotificationChannel = Await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync()
            AddHandler pushNotificationChannel.PushNotificationReceived, AddressOf PushNotificationChannelPushNotificationReceived

            Dim hub = New NotificationHub(Constants.HubName, Constants.ConnectionString)
            
            ' register the channel in NH without tags
            'Dim result = Await hub.RegisterNativeAsync(pushNotificationChannel.Uri)

            Dim result = Await hub.RegisterNativeAsync(pushNotificationChannel.Uri, SettingsHelper.Tags)

            If result.RegistrationId IsNot Nothing Then
                Await ShowRegistrationIdAsync(result)
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Shows the registration identifier.
    ''' </summary>
    ''' <param name="result">The result.</param>
    ''' <returns>Task.</returns>
    Private Shared Async Function ShowRegistrationIdAsync(result As Registration) As Task
        Dim dialog = New MessageDialog("Registration successful: " + result.RegistrationId)
        dialog.Commands.Add(New UICommand("OK"))
        Await dialog.ShowAsync()
    End Function

    ''' <summary>
    ''' Pushes the notification channel push notification received.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="args">The <see cref="PushNotificationReceivedEventArgs"/> instance containing the event data.</param>
    Private Shared Sub PushNotificationChannelPushNotificationReceived(sender As PushNotificationChannel, args As PushNotificationReceivedEventArgs)
        Dim result As String = args.NotificationType.ToString()

        Select Case args.NotificationType
            Case PushNotificationType.Badge
                If args.BadgeNotification IsNot Nothing Then
                    result += ": " + args.BadgeNotification.Content.GetXml()
                    Dim updater = BadgeUpdateManager.CreateBadgeUpdaterForApplication()
                    updater.Update(New BadgeNotification(args.BadgeNotification.Content))
                End If
                Exit Select
            Case PushNotificationType.Raw
                If args.RawNotification IsNot Nothing Then
                    ' todo something here...
                    result += ": " + args.RawNotification.Content
                End If
                Exit Select
            Case PushNotificationType.Tile
                If args.TileNotification IsNot Nothing Then
                    result += ": " + args.TileNotification.Content.GetXml()
                    Dim updater = TileUpdateManager.CreateTileUpdaterForApplication()
                    updater.Update(New TileNotification(args.TileNotification.Content))
                End If
                Exit Select
            Case PushNotificationType.Toast
                If args.ToastNotification IsNot Nothing Then
                    ' todo something here...
                    result += ": " + args.ToastNotification.Content.GetXml()
                End If
                Exit Select
            Case Else
                Exit Select
        End Select

    End Sub
End Class
