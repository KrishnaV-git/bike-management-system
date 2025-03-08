Imports System.Data.SqlClient

Public Class naked
    Private Sub button1_click(sender As Object, e As EventArgs) Handles Button1.Click
        customer_dashboard.Show()
        Me.Hide()
    End Sub
    Private connectionString As String = "Data Source=HOME\SQLEXPRESS;Initial Catalog=wheelwiseproj1;Integrated Security=True"

    ' Button to fetch details for BikeID 1
    Private Sub button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        FetchBikeDetails(9) ' Fetch details for Bike 1
    End Sub

    ' Button to fetch details for BikeID 2
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        FetchBikeDetails(10) ' Fetch details for Bike 2
    End Sub

    ' Button to fetch details for BikeID 3
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        FetchBikeDetails(11) ' Fetch details for Bike 3
    End Sub

    ' Button to fetch details for BikeID 4
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        FetchBikeDetails(12) ' Fetch details for Bike 4
    End Sub

    ' Function to Fetch Bike Details by BikeID
    Private Sub FetchBikeDetails(bikeID As Integer)
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT ModelName, Price, EngineSpec, Type, Manufacturer FROM bikes_1 WHERE BikeID = @BikeID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@BikeID", bikeID)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim modelName As String = reader("ModelName").ToString()
                            Dim price As Decimal = Convert.ToDecimal(reader("Price"))
                            Dim engineSpec As String = reader("EngineSpec").ToString()
                            Dim bikeType As String = reader("Type").ToString()
                            Dim manufacturer As String = reader("Manufacturer").ToString()

                            ' Display the fetched bike details
                            MessageBox.Show($"Bike ID: {bikeID}" & vbCrLf &
                                                $"Model Name: {modelName}" & vbCrLf &
                                                $"Price: {price:C}" & vbCrLf &
                                                $"Engine Spec: {engineSpec}" & vbCrLf &
                                                $"Type: {bikeType}" & vbCrLf &
                                                $"Manufacturer: {manufacturer}",
                                                "Bike Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("No details found for the selected Bike ID.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Form8.Show()
        Me.Hide()
    End Sub
End Class