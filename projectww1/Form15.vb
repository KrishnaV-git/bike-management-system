Imports System.Data.SqlClient

Public Class Form15
    Private Sub button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Database connection string (Modify as per your database)
        Dim connString As String = "Data Source=HOME\SQLEXPRESS;Initial Catalog=wheelwiseproj1;Integrated Security=True"
        Dim connection As New SqlConnection(connString)

        Try
            ' Open the connection
            connection.Open()

            ' SQL query to check if the user exists
            Dim query As String = "SELECT Password FROM customer_1 WHERE Username = @username"
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@username", TextBox1.Text)

            ' Execute query
            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.Read() Then
                ' Fetch password from database
                Dim storedPassword As String = reader("Password").ToString()

                ' Check if entered password matches
                If TextBox2.Text = storedPassword Then
                    MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    reader.Close()
                    connection.Close()

                    ' Open next form and close login form
                    Dim nextForm As New bikes()
                    customer_dashboard.Show()
                    Me.Hide()
                Else
                    MessageBox.Show("Invalid password! Exiting application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Application.Exit()
                End If
            Else
                MessageBox.Show("Username not found! Exiting application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.Exit()
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
    End Sub
End Class