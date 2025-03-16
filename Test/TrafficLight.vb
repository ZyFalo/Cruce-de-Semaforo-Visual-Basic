Public Class TrafficLight
    Private _currentState As Integer
    Private ReadOnly _lockObj As New Object()

    ' Constructor que asigna un estado inicial
    Public Sub New(Optional initialState As Integer = 1)
        _currentState = initialState
    End Sub

    Public Property CurrentState As Integer
        Get
            SyncLock _lockObj
                Return _currentState
            End SyncLock
        End Get
        Set(value As Integer)
            SyncLock _lockObj
                _currentState = value
            End SyncLock
        End Set
    End Property
End Class
