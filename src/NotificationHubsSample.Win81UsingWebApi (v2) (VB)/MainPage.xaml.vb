' The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
Imports Windows.Networking.PushNotifications
Imports Windows.UI.Notifications
Imports Windows.UI.Popups
Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Text

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Private Const RegisterUrl As String = "https://notificationhubssamplewebapiv2.azurewebsites.net/api/NotificationHub/Register"

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MainPage"/> class.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        DataContext = Me
    End Sub

    ''' <summary>
    ''' Gets the margin for grid.
    ''' </summary>
    ''' <value>The margin for grid.</value>
    Public ReadOnly Property MarginForGrid() As Thickness
        Get
#If WINDOWS_APP Then
        Return New Thickness(120, 58, 120, 80)
#Else
#End If
            Return New Thickness(20, 58, 20, 80)
        End Get
    End Property

    ''' <summary>
    ''' This method gets both an installation ID and channel for push notifications and sends it, 
    ''' along with the device type, to the authenticated Web API method that creates a registration 
    ''' in Notification Hubs.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    Private Async Sub LoginClick(sender As Object, e As RoutedEventArgs)

        ' Get the info that we need to request registration.
        Dim registrationId = InstId.Text = SettingsHelper.GetRegistrationId()

        Dim message As String

        Try
            Dim channel = Await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync()
            AddHandler channel.PushNotificationReceived, AddressOf ChannelPushNotificationReceived

            ' Define a registration to pass in the body of the POST request.
            Dim deviceInfo = New DeviceInfo()
            deviceInfo.Platform = "wns"
            deviceInfo.Handle = channel.Uri
            deviceInfo.RegistrationId = registrationId
            deviceInfo.Tags = New List(Of String)()

            ' Create a client to send the HTTP registration request.
            Dim client = New HttpClient()
            Dim request = New HttpRequestMessage(HttpMethod.Post, New Uri(RegisterUrl))

            ' Add the Authorization header with the basic login credentials.
            Dim auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(User.Text + ":" + Password.Password))
            request.Headers.Add("Authorization", auth)
            request.Content = New StringContent(JsonConvert.SerializeObject(DeviceInfo), Encoding.UTF8, "application/json")


            ' Send the registration request.
            Dim response = Await client.SendAsync(request)

            ' Get and display the response
            message = Await response.Content.ReadAsStringAsync()
        Catch ex As Exception
            message = ex.Message
        End Try

        ' Display a message dialog.
        Dim dialog = New MessageDialog(message)
        dialog.Commands.Add(New UICommand("OK"))
        Await dialog.ShowAsync()
    End Sub

    ''' <summary>
    ''' Channels the push notification received.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="args">The <see cref="PushNotificationReceivedEventArgs"/> instance containing the event data.</param>
    Private Sub ChannelPushNotificationReceived(sender As PushNotificationChannel, args As PushNotificationReceivedEventArgs)
        Dim result = args.NotificationType.ToString()

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
