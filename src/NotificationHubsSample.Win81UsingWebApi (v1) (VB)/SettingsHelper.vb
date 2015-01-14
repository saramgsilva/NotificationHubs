Imports Windows.Storage

''' <summary>
''' Define the SettingsHelper.
''' </summary>
Public Class SettingsHelper
    Private Const ContainerName As String = "InstallationContainer"
    Private Const KeyName As String = "InstallationId"

    ''' <summary>
    ''' Gets the local storage container.
    ''' </summary>
    ''' <returns>ApplicationDataContainer.</returns>
    Private Shared Function GetLocalStorageContainer() As ApplicationDataContainer

        If Not ApplicationData.Current.LocalSettings.Containers.ContainsKey(ContainerName) Then
            ApplicationData.Current.LocalSettings.CreateContainer(ContainerName, ApplicationDataCreateDisposition.Always)
        End If
        Return ApplicationData.Current.LocalSettings.Containers(ContainerName)
    End Function

    ''' <summary>
    ''' Gets the installation identifier.
    ''' </summary>
    ''' <returns>The System.String.</returns>
    Public Shared Function GetInstallationId() As String
        Dim container = GetLocalStorageContainer()
        If Not container.Values.ContainsKey(KeyName) Then
            container.Values(KeyName) = Guid.NewGuid().ToString()
        End If
        Return DirectCast(container.Values(KeyName), String)
    End Function
End Class