Imports System.Threading

Public Class Carro
    Public Property PictureBox As PictureBox
    Public Property Velocidad As Integer
    Public Property Direccion As String
    Public Property PosicionInicial As Point
    Public Property FormWidth As Integer
    Public Property FormHeight As Integer
    Public Property IntersectionBounds As Rectangle ' Área de la intersección
    Public Property TrafficLight As TrafficLight ' Referencia al semáforo global
    Public Property AllCars As List(Of Carro) ' Lista de todos los carros
    Public Property IsInIntersection As Boolean = False

    Private _hilo As Thread
    Private _isRunning As Boolean

    Public Sub New(
        pb As PictureBox,
        velocidad As Integer,
        direccion As String,
        formWidth As Integer,
        formHeight As Integer,
        trafficLight As TrafficLight,
        intersectionBounds As Rectangle
    )
        Me.PictureBox = pb
        Me.Velocidad = velocidad
        Me.Direccion = direccion
        Me.PosicionInicial = pb.Location
        Me.FormWidth = formWidth
        Me.FormHeight = formHeight
        Me.TrafficLight = trafficLight
        Me.IntersectionBounds = intersectionBounds
    End Sub

    Public Sub Iniciar()
        _isRunning = True
        _hilo = New Thread(AddressOf Mover)
        _hilo.IsBackground = True
        _hilo.Start()
    End Sub

    Public Sub Detener()
        _isRunning = False
        If _hilo IsNot Nothing AndAlso _hilo.IsAlive Then
            _hilo.Join()
        End If
    End Sub

    Private Sub Mover()
        While _isRunning
            If Direccion = "Horizontal" Then
                MoverHorizontal()
            Else
                MoverVertical()
            End If
            Thread.Sleep(20) ' Controla la velocidad de actualización
        End While
    End Sub

    ' Función para detectar colisiones usando los bounds de los PictureBox
    Private Function CollisionAhead(newRect As Rectangle) As Boolean
        If AllCars Is Nothing Then Return False
        For Each otherCar As Carro In AllCars
            If otherCar IsNot Me Then
                If newRect.IntersectsWith(otherCar.PictureBox.Bounds) Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Private Sub MoverHorizontal()
        Dim nuevaPosicion As Integer = PictureBox.Left + Velocidad
        Dim newRect As New Rectangle(nuevaPosicion, PictureBox.Top, PictureBox.Width, PictureBox.Height)

        ' Verificar semáforo en intersección (estado 1 para horizontal)
        If TrafficLight.CurrentState = 1 AndAlso IntersectionBounds.IntersectsWith(newRect) Then
            Return ' Detener movimiento si el semáforo está en rojo para la vía horizontal
        End If

        ' Verificar colisión con otro vehículo
        If CollisionAhead(newRect) Then
            Return ' Si hay un vehículo adelante, se detiene el movimiento
        End If

        SyncLock PictureBox
            ' Verificar límite del formulario y reiniciar si es necesario
            If nuevaPosicion > FormWidth Then
                nuevaPosicion = -PictureBox.Width
            End If
        End SyncLock

        ' Actualizar UI de manera segura
        If PictureBox.InvokeRequired Then
            PictureBox.Invoke(Sub() PictureBox.Left = nuevaPosicion)
        Else
            PictureBox.Left = nuevaPosicion
        End If
    End Sub

    Private Sub MoverVertical()
        Dim nuevaPosicion As Integer = PictureBox.Top + Velocidad
        Dim newRect As New Rectangle(PictureBox.Left, nuevaPosicion, PictureBox.Width, PictureBox.Height)

        ' Verificar semáforo en intersección (estado 2 para vertical)
        If TrafficLight.CurrentState = 2 AndAlso IntersectionBounds.IntersectsWith(newRect) Then
            Return ' Detener movimiento si el semáforo está en rojo para la vía vertical
        End If

        ' Verificar colisión con otro vehículo
        If CollisionAhead(newRect) Then
            Return ' Si hay un vehículo adelante, se detiene el movimiento
        End If

        SyncLock PictureBox
            If nuevaPosicion > FormHeight Then
                nuevaPosicion = -PictureBox.Height
            End If
        End SyncLock

        If PictureBox.InvokeRequired Then
            PictureBox.Invoke(Sub() PictureBox.Top = nuevaPosicion)
        Else
            PictureBox.Top = nuevaPosicion
        End If
    End Sub

    Public Sub ReiniciarPosicion()
        If PictureBox.InvokeRequired Then
            PictureBox.Invoke(Sub() PictureBox.Location = PosicionInicial)
        Else
            PictureBox.Location = PosicionInicial
        End If
    End Sub
End Class