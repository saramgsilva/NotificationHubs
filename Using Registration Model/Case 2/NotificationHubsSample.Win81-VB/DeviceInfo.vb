''' <summary>
''' Define the DeviceInfo.
''' </summary>
Public Class DeviceInfo
    ''' <summary>
    ''' Gets or sets the registration identifier.
    ''' </summary>
    ''' <value>The registration identifier.</value>
    Public Property RegistrationId() As String
        Get
            Return m_RegistrationId
        End Get
        Set(value As String)
            m_RegistrationId = Value
        End Set
    End Property
    Private m_RegistrationId As String

    ''' <summary>
    ''' Gets or sets the platform.
    ''' </summary>
    ''' <value>The platform.</value>
    Public Property Platform() As String
        Get
            Return m_Platform
        End Get
        Set(value As String)
            m_Platform = Value
        End Set
    End Property
    Private m_Platform As String

    ''' <summary>
    ''' Gets or sets the handle.
    ''' </summary>
    ''' <value>The handle.</value>
    Public Property Handle() As String
        Get
            Return m_Handle
        End Get
        Set(value As String)
            m_Handle = Value
        End Set
    End Property
    Private m_Handle As String

    ''' <summary>
    ''' Gets or sets the tags.
    ''' </summary>
    ''' <value>The tags.</value>
    Public Property Tags() As IEnumerable(Of String)
        Get
            Return m_Tags
        End Get
        Set(value As IEnumerable(Of String))
            m_Tags = Value
        End Set
    End Property
    Private m_Tags As IEnumerable(Of String)
End Class