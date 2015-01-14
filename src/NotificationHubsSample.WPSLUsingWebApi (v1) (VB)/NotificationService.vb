Imports System.Diagnostics
Imports Microsoft.Phone.Notification
Imports System.Text

''' Define the NotificationService.
''' </summary>
Public Class NotificationService
    ''' <summary>
    ''' Gets or sets the channel URI.
    ''' </summary>
    ''' <value>The channel URI.</value>
    Public Property ChannelUri() As String
        Get
            Return m_ChannelUri
        End Get
        Set(value As String)
            m_ChannelUri = Value
        End Set
    End Property
    Private m_ChannelUri As String

    ''' <summary>
    ''' Requests this channel uri.
    ''' </summary>
    Public Sub Request()
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
    Private Sub Channel_ChannelUriUpdated(sender As Object, e As NotificationChannelUriEventArgs)
        ChannelUri = e.ChannelUri.ToString()
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
