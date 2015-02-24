Imports Windows.Storage

''' <summary>
''' Define the SettingsHelper.
''' </summary>
Public NotInheritable Class SettingsHelper
    Private Sub New()
    End Sub
    Private Shared ReadOnly LocalSettings As ApplicationDataContainer
    Private Const TagsKey As String = "Tags"
    Private Const ChannelUriKey As String = "ChannelUri"
    Private Const RegistrationIdKey As String = "RegistrationId"

    ''' <summary>
    ''' Initializes static members of the <see cref="SettingsHelper"/> class.
    ''' </summary>
    Shared Sub New()
        LocalSettings = ApplicationData.Current.LocalSettings
    End Sub

    ''' <summary>
    ''' Gets or sets the tags.
    ''' </summary>
    ''' <value>The tags.</value>
    Public Shared Property Tags() As List(Of String)
        Get
            If LocalSettings.Values.ContainsKey(TagsKey) Then
                Dim tagsSaved = LocalSettings.Values(TagsKey).ToString()
                Return tagsSaved.Split(New Char() {","c}).ToList()
            End If
            Return New List(Of String)()
        End Get
        Set(value As List(Of String))
            If value Is Nothing OrElse value.Count = 0 Then
                LocalSettings.Values.Remove(TagsKey)
            Else
                LocalSettings.Values(TagsKey) = String.Join(",", value.ToArray())
            End If
        End Set
    End Property

    ''' <summary>
    ''' Removes all.
    ''' </summary>
    Public Shared Sub RemoveAll()
        LocalSettings.Values.Remove(ChannelUriKey)
        LocalSettings.Values.Remove(RegistrationIdKey)
    End Sub

    ''' <summary>
    ''' Gets or sets the channel URI.
    ''' </summary>
    ''' <value>The channel URI.</value>
    Public Shared Property ChannelUri() As String
        Get
            If LocalSettings.Values.ContainsKey(ChannelUriKey) Then
                Return LocalSettings.Values(ChannelUriKey).ToString()
            End If
            Return String.Empty
        End Get
        Set(value As String)
            LocalSettings.Values(ChannelUriKey) = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the registration identifier.
    ''' </summary>
    ''' <value>The registration identifier.</value>
    Public Shared Property RegistrationId() As String
        Get
            If LocalSettings.Values.ContainsKey(RegistrationIdKey) Then
                Return LocalSettings.Values(RegistrationIdKey).ToString()
            End If
            Return String.Empty
        End Get
        Set(value As String)
            LocalSettings.Values(RegistrationIdKey) = value
        End Set
    End Property
End Class