Imports System.Data.SqlClient

Public Class Form9
    Private connectionString As String = "Data Source=HOME\SQLEXPRESS;Initial Catalog=wheelwiseproj1;Integrated Security=True"

    ' Button to fetch details for BikeID 1
    Private Sub button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        FetchaccessoriesDetails(701) ' Fetch details for Bike 1
    End Sub

    ' Button to fetch details for BikeID 2
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        FetchaccessoriesDetails(702) ' Fetch details for Bike 2
    End Sub

    ' Button to fetch details for BikeID 3
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        FetchaccessoriesDetails(703) ' Fetch details for Bike 3
    End Sub

    ' Button to fetch details for BikeID 4
    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        FetchaccessoriesDetails(704) ' Fetch details for Bike 4
    End Sub
    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        FetchaccessoriesDetails(705) ' Fetch details for Bike 4
    End Sub
    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        FetchaccessoriesDetails(706) ' Fetch details for Bike 4
    End Sub
    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        FetchaccessoriesDetails(707) ' Fetch details for Bike 4
    End Sub
    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        FetchaccessoriesDetails(708) ' Fetch details for Bike 4
    End Sub

    ' Function to Fetch Bike Details by BikeID
    Private Sub FetchaccessoriesDetails(accessoriesID As Integer)
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT * FROM accessories_1 WHERE accessoriesID = @accessoriesID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@accessoriesID", accessoriesID)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim Name As String = reader("Name").ToString()
                            Dim price_of_accessories As Decimal = Convert.ToDecimal(reader("Price_of_accessories"))
                            Dim features As String = reader("features").ToString()
                            Dim showroomid As String = reader("showroomid").ToString()

                            ' Display the fetched bike details
                            MessageBox.Show($"accessories ID: {accessoriesID}" & vbCrLf &
                                                $"name: {Name}" & vbCrLf &
                                                $"Price_of_accessories: {price_of_accessories:C}" & vbCrLf &
                                                $"features: {features}" & vbCrLf &
                                                $"showroomid: {showroomid}" & vbCrLf &
                                                "accessories Details")
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

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form25.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form25.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form25.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form25.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form25.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Form25.Show()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form25.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form25.Show()
    End Sub
End Class