' The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

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
            ToggleSwitchNews.IsOn = True
        End If
        If tags.Contains("sports") Then
            ToggleSwitchSports.IsOn = True
        End If
        If tags.Contains("music") Then
            ToggleSwitchMusic.IsOn = True
        End If
    End Sub

    ''' <summary>
    ''' Handles the OnClick event of the ButtonBase control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    Private Sub Button_OnClick(sender As Object, e As RoutedEventArgs)
        UpdateTags()
        NotificationService.CreateOrUpdateNotificationsAsync()
    End Sub

    ''' <summary>
    ''' Updates the tags.
    ''' </summary>
    Private Sub UpdateTags()
        Dim tags = SettingsHelper.Tags
        If tags.Count > 0 Then
            Button.Content = "Update"
        End If
        If ToggleSwitchNews.IsOn Then
            If Not tags.Contains("news") Then
                tags.Add("news")
            End If
        Else
            tags.Remove("news")
        End If
        If ToggleSwitchSports.IsOn Then
            If Not tags.Contains("sports") Then
                tags.Add("sports")
            End If
        Else
            tags.Remove("sports")
        End If
        If ToggleSwitchMusic.IsOn Then
            If Not tags.Contains("music") Then
                tags.Add("music")
            End If
        Else
            tags.Remove("music")
        End If
        SettingsHelper.Tags = tags
    End Sub

End Class
