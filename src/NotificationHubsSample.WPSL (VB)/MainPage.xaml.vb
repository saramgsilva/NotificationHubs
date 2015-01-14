Imports Microsoft.Phone.Controls

Partial Public Class MainPage
    Inherits PhoneApplicationPage

    ' Constructor
    Public Sub New()
        InitializeComponent()

        SupportedOrientations = SupportedPageOrientation.Portrait Or SupportedPageOrientation.Landscape

        ' Sample code to localize the ApplicationBar
        'BuildLocalizedApplicationBar()

    End Sub


    ''' <summary>
    ''' Invoked when this page is about to be displayed in a Frame.
    ''' </summary>
    ''' <param name="e">Event data that describes how this page was reached.
    ''' This parameter is typically used to configure the page.</param>
    Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
        Dim tags = SettingsHelper.Tags
        If tags.Count > 0 Then
            Button.Content = "Update"
        End If
        If tags.Contains("news") Then
            ToggleSwitchNews.IsChecked = True
        End If
        If tags.Contains("sports") Then
            ToggleSwitchSports.IsChecked = True
        End If
        If tags.Contains("music") Then
            ToggleSwitchMusic.IsChecked = True
        End If
    End Sub

    ''' <summary>
    ''' Handles the OnClick event of the ButtonBase control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    Private Sub Button_OnClick(sender As Object, e As RoutedEventArgs)
        UpdateTags()
        App.NotificationService.CreateOrUpdateNotificationsAsync()
    End Sub

    ''' <summary>
    ''' Updates the tags.
    ''' </summary>
    Private Sub UpdateTags()
        Dim tags = SettingsHelper.Tags
        If tags.Count > 0 Then
            Button.Content = "Update"
        End If
        If ToggleSwitchNews.IsChecked Then
            If Not tags.Contains("news") Then
                tags.Add("news")
            End If
        Else
            tags.Remove("news")
        End If
        If ToggleSwitchSports.IsChecked Then
            If Not tags.Contains("sports") Then
                tags.Add("sports")
            End If
        Else
            tags.Remove("sports")
        End If
        If ToggleSwitchMusic.IsChecked Then
            If Not tags.Contains("music") Then
                tags.Add("music")
            End If
        Else
            tags.Remove("music")
        End If
        SettingsHelper.Tags = tags
    End Sub


End Class