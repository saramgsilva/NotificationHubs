''' <summary>
''' Define the Car.
''' </summary>
Public Class Car
    ''' <summary>
    ''' Gets or sets the name.
    ''' </summary>
    ''' <value>The name.</value>
    Public Property Name As String

    ''' <summary>
    ''' Gets or sets a value indicating whether this instance is electric.
    ''' </summary>
    ''' <value><c>true</c> if this instance is electric; otherwise, <c>false</c>.</value>
    Public Property IsElectric As Boolean

    ''' <summary>
    ''' Gets or sets the created at.
    ''' </summary>
    ''' <value>The created at.</value>
    Public Property CreatedAt As System.Nullable(Of DateTimeOffset)

    ''' <summary>
    ''' Gets or sets a value indicating whether this <see cref="Car"/> is deleted.
    ''' </summary>
    ''' <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
    Public Property Deleted As Boolean

    ''' <summary>
    ''' Gets or sets the identifier.
    ''' </summary>
    ''' <value>The identifier.</value>
    Public Property Id As String

    ''' <summary>
    ''' Gets or sets the updated at.
    ''' </summary>
    ''' <value>The updated at.</value>
    Public Property UpdatedAt As System.Nullable(Of DateTimeOffset)

    ''' <summary>
    ''' Gets or sets the version.
    ''' </summary>
    ''' <value>The version.</value>
    Public Property Version As Byte()
End Class
