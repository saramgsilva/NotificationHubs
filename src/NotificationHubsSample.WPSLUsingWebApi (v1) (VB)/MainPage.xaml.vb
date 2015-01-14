Imports Windows.Networking.PushNotifications
Imports Windows.UI.Notifications
Imports System.Text
Imports System.Net.Http
Imports Newtonsoft.Json
Partial Public Class MainPage
    Inherits PhoneApplicationPage

    Private Const RegisterUrl As String = "https://NotificationHubsSampleWebApi.azurewebsites.net/api/register"

    ' In this case the templates are defined when is registered the decive
    Private Const RegisterUsingTemplatesUrl As String = "https://NotificationHubsSampleWebApi.azurewebsites.net/api/RegisterUsingTemplates"

    ' Constructor
    Public Sub New()
        InitializeComponent()

        SupportedOrientations = SupportedPageOrientation.Portrait Or SupportedPageOrientation.Landscape
        DataContext = Me
        ' Sample code to localize the ApplicationBar
        'BuildLocalizedApplicationBar()

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
    Private Async Sub Login_Click(sender As Object, e As RoutedEventArgs)
        ' Get the info that we need to request registration.
        Dim installationId = InstId.Text = SettingsHelper.GetInstallationId()

        Dim message As String

        Try
           
            ' Define a registration to pass in the body of the POST request.
            Dim registration = New Dictionary(Of String, String)() From {
                {"platform", "mpns"},
                {"instId", installationId},
                {"channelUri", App.NotificationService.ChannelUri}
            }

            ' Create a client to send the HTTP registration request.
            Dim client = New HttpClient()
            Dim request = New HttpRequestMessage(HttpMethod.Post, New Uri(RegisterUrl))

            ' Add the Authorization header with the basic login credentials.
            Dim auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Text + ":" + Password.Password))
            request.Headers.Add("Authorization", auth)
            request.Content = New StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json")


            ' Send the registration request.
            Dim response = Await client.SendAsync(request)

            ' Get and display the response, either the registration ID
            ' or an error message.
            message = Await response.Content.ReadAsStringAsync()
            message = String.Format("Registration ID: {0}", message)
        Catch ex As Exception
            message = ex.Message
        End Try

        MessageBox.Show(message)
     
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