' The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641
Imports Windows.UI.Popups

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
    Protected Overrides Sub OnNavigatedTo(e As Navigation.NavigationEventArgs)
        ' TODO: Prepare the page for display here.

        ' TODO: If your application contains multiple pages, ensure that you are
        ' handling the hardware Back button by registering for the
        ' Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
        ' If you are using the NavigationHelper provided by some templates,
        ' this event is handled for you.
    End Sub

    Private Async Sub ButtonBase_OnClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim carToInsert = New Car()
        carToInsert.Name = "Renault 4l"
        carToInsert.IsElectric = False

        Dim table = App.NotificationHubsSampleAMSClient.GetTable(Of Car)()
        Await table.InsertAsync(carToInsert)

        Dim dialog = New MessageDialog("Inserted.")
        dialog.Commands.Add(New UICommand("OK"))
        Await dialog.ShowAsync()
    End Sub
End Class
