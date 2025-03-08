Imports System.Data.SqlClient

Public Class Form10
    ' SQL Connection String
    Private connectionString As String = "Server=HOME\SQLEXPRESS;Database=wheelwiseproj1;Integrated Security=True"

    Private Sub TestRideForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBikesIntoComboBox()
        DateTimePicker1.Format = DateTimePickerFormat.Custom
        DateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss" ' Set format for date and time
    End Sub

    ' Load bike names into ComboBox from bikes_1 table
    Private Sub LoadBikesIntoComboBox()
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()
                Dim query As String = "SELECT modelname FROM bikes_1"
                Using cmd As New SqlCommand(query, con)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            ComboBox1.Items.Add(reader("modelname").ToString())
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading bikes: " & ex.Message)
        End Try
    End Sub

    ' Button Click Event to Schedule Test Ride
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("Please enter your username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If ComboBox1.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a bike.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get username from TextBox1
        Dim username As String = TextBox1.Text.Trim()

        ' Fetch Customer ID based on entered username
        Dim customerId As Integer = FetchCustomerId(username)
        If customerId = 0 Then
            MessageBox.Show("Customer not found. Please enter a valid username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Fetch Bike ID & Showroom ID based on selected bike
        Dim selectedBike As String = ComboBox1.SelectedItem.ToString()
        Dim bikeDetails As (Integer, Integer) = FetchBikeDetails(selectedBike)
        Dim bikeId As Integer = bikeDetails.Item1
        Dim showroomId As Integer = bikeDetails.Item2

        If bikeId = 0 Or showroomId = 0 Then
            MessageBox.Show("Error retrieving bike details. Please select a valid bike.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Generate Random Appointment ID
        Dim random As New Random()
        Dim appointmentId As Integer = random.Next(10000, 99999)

        ' Get Selected Date from DateTimePicker
        Dim appointmentschedule As String = DateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")

        ' Check if the customer already has a booking for the same bike
        If HasExistingBooking(customerId, bikeId) Then
            MessageBox.Show("You already have a test ride scheduled for this bike.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Store in test_ride table
        If StoreTestRide(appointmentId, customerId, bikeId, showroomId, appointmentschedule) Then
            MessageBox.Show($"Your test ride for {selectedBike} has been booked on {appointmentschedule}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Error booking test ride.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ' Function to Fetch Customer ID based on the entered username
    Private Function FetchCustomerId(username As String) As Integer
        Dim customerId As Integer = 0
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()
                Dim query As String = "SELECT customerid FROM customer_1 WHERE username = @Username"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@Username", username)
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        customerId = Convert.ToInt32(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching Customer ID: " & ex.Message)
        End Try

        ' Debugging Message
        MessageBox.Show("Fetched Customer ID: " & customerId)

        Return customerId
    End Function

    ' Function to Fetch Bike ID & Showroom ID
    Private Function FetchBikeDetails(bikeName As String) As (Integer, Integer)
        Dim bikeId As Integer = 0
        Dim showroomId As Integer = 0
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()
                Dim query As String = "SELECT bikeid, showroomid FROM bikes_1 WHERE modelname = @ModelName;"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@ModelName", bikeName)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            bikeId = Convert.ToInt32(reader("bikeid"))
                            showroomId = Convert.ToInt32(reader("showroomid"))
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching Bike details: " & ex.Message)
        End Try
        Return (bikeId, showroomId)
    End Function

    ' Function to Store Test Ride in Database
    Private Function StoreTestRide(appointmentId As Integer, customerId As Integer, bikeId As Integer, showroomId As Integer, appointmentschedule As String) As Boolean
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()
                Dim query As String = "INSERT INTO testride_1 (appointmentid, customerid, bikeid, showroomid, appointmentschedule) VALUES (@AppId, @CustId, @BikeId, @ShowroomId, @Appointmentschedule)"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@AppId", appointmentId)
                    cmd.Parameters.AddWithValue("@CustId", customerId)
                    cmd.Parameters.AddWithValue("@BikeId", bikeId)
                    cmd.Parameters.AddWithValue("@ShowroomId", showroomId)
                    cmd.Parameters.AddWithValue("@Appointmentschedule", appointmentschedule)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            Return True
        Catch ex As Exception
            MessageBox.Show("Error storing Test Ride: " & ex.Message)
            Return False
        End Try
    End Function

    ' Function to Check if Customer has Existing Booking for the Same Bike
    Private Function HasExistingBooking(customerId As Integer, bikeId As Integer) As Boolean
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()
                Dim query As String = "SELECT COUNT(*) FROM testride_1 WHERE customerid = @CustomerId AND bikeid = @BikeId"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@CustomerId", customerId)
                    cmd.Parameters.AddWithValue("@BikeId", bikeId)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking existing booking: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return True
        End Try
    End Function
End Class