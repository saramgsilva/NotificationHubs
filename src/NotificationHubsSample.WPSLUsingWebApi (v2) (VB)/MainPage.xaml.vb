Imports System.Net.Http
Imports Newtonsoft.Json
Imports Microsoft.Phone.Controls
Imports System.Text

Partial Public Class MainPage
    Inherits PhoneApplicationPage

    Private Const RegisterUrl As String = "https://notificationhubssamplewebapiv2.azurewebsites.net/api/NotificationHub/Register"

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MainPage"/> class.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        SupportedOrientations = SupportedPageOrientation.Portrait Or SupportedPageOrientation.Landscape
        DataContext = Me
    End Sub

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
           
            ' Define a registration to pass in the body of the POST request.
            Dim deviceInfo = New DeviceInfo()
            deviceInfo.Platform = "mpns"
            deviceInfo.Handle = App.NotificationService.ChannelUri
            deviceInfo.RegistrationId = registrationId
            deviceInfo.Tags = New List(Of String)()


            ' Create a client to send the HTTP registration request.
            Dim client = New HttpClient()
            Dim request = New HttpRequestMessage(HttpMethod.Post, New Uri(RegisterUrl))

            ' Add the Authorization header with the basic login credentials.
            Dim auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(User.Text + ":" + Password.Password))
            request.Headers.Add("Authorization", auth)
            request.Content = New StringContent(JsonConvert.SerializeObject(deviceInfo), Encoding.UTF8, "application/json")


            ' Send the registration request.
            Dim response = Await client.SendAsync(request)

            ' Get and display the response
            message = Await response.Content.ReadAsStringAsync()
        Catch ex As Exception
            message = ex.Message
        End Try

        MessageBox.Show(message)
    End Sub

End Class