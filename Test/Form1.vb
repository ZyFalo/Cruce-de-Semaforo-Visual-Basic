Public Class Form1
    Private listaCarros As New List(Of Carro)
    ' Se crea el semáforo con estado inicial = 1 (por ejemplo)
    Private trafficLight As New TrafficLight(1)
    Private intersectionPictureBox As PictureBox ' PictureBox que representa la intersección

    ' Se asume que en el formulario ya existen dos PictureBox llamados semaforo1 y semaforo2

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configurar la intersección (ej: PictureBox llamado "intersection")
        intersectionPictureBox = intersection
        Dim intersectionBounds As New Rectangle(
            intersectionPictureBox.Left,
            intersectionPictureBox.Top,
            intersectionPictureBox.Width,
            intersectionPictureBox.Height
        )

        ' Inicializar carros con referencia al semáforo y la intersección
        listaCarros.Add(New Carro(CarHor1, 4, "Horizontal", Me.ClientSize.Width, Me.ClientSize.Height, trafficLight, intersectionBounds))
        listaCarros.Add(New Carro(CarHor2, 3, "Horizontal", Me.ClientSize.Width, Me.ClientSize.Height, trafficLight, intersectionBounds))
        listaCarros.Add(New Carro(CarHor3, 3, "Horizontal", Me.ClientSize.Width, Me.ClientSize.Height, trafficLight, intersectionBounds))
        listaCarros.Add(New Carro(CarHor4, 3, "Horizontal", Me.ClientSize.Width, Me.ClientSize.Height, trafficLight, intersectionBounds))

        listaCarros.Add(New Carro(CarVer1, 3, "Vertical", Me.ClientSize.Width, Me.ClientSize.Height, trafficLight, intersectionBounds))
        listaCarros.Add(New Carro(CarVer2, 3, "Vertical", Me.ClientSize.Width, Me.ClientSize.Height, trafficLight, intersectionBounds))
        listaCarros.Add(New Carro(CarVer3, 2, "Vertical", Me.ClientSize.Width, Me.ClientSize.Height, trafficLight, intersectionBounds))
        listaCarros.Add(New Carro(CarVer4, 2, "Vertical", Me.ClientSize.Width, Me.ClientSize.Height, trafficLight, intersectionBounds))

        ' Asignar la lista completa de carros a cada objeto, para poder detectar colisiones
        For Each carro As Carro In listaCarros
            carro.AllCars = listaCarros
        Next

        ' Iniciar hilos para cada carro
        For Each carro In listaCarros
            carro.Iniciar()
        Next

        ' Actualizar visualmente el semáforo al iniciar
        UpdateTrafficLightDisplay()

        ' Iniciar temporizador para cambiar estados del semáforo
        Dim timer As New Timer()
        timer.Interval = 5000 ' 5 segundos
        AddHandler timer.Tick, AddressOf CambiarEstadoSemaforo
        timer.Start()
    End Sub

    Private Sub CambiarEstadoSemaforo(sender As Object, e As EventArgs)
        ' Cambiar el estado del semáforo: si es 1, pasa a 2; si es 2, pasa a 1.
        trafficLight.CurrentState = If(trafficLight.CurrentState = 1, 2, 1)
        UpdateTrafficLightDisplay()
    End Sub

    ' Método para actualizar el color de los PictureBox según el estado del semáforo
    Private Sub UpdateTrafficLightDisplay()
        If trafficLight.CurrentState = 1 Then
            semaforo1.BackColor = Color.Green
            semaforo2.BackColor = Color.Red
        ElseIf trafficLight.CurrentState = 2 Then
            semaforo1.BackColor = Color.Red
            semaforo2.BackColor = Color.Green
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class
