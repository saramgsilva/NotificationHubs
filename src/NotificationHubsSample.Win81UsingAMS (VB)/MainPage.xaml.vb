' The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
Imports Windows.UI.Popups

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

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
