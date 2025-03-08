Imports System.Data.SqlClient

Public Class Form12
    ' Database connection
    Dim connection As New SqlConnection("Server=HOME\SQLEXPRESS;Database=wheelwiseproj1;Integrated Security=True")

    ' Generate Random Feedback ID
    Private Function GenerateRandomFeedbackID() As Integer
        Dim rnd As New Random()
        Return rnd.Next(10000, 99999) ' Generate a 5-digit random feedback ID
    End Function

    ' Fetch CustomerID, BikeID, and AccessoriesID from Payment_1 Table
    Private Function GetCustomerBikeAccessoriesID(username As String) As (Integer, Integer, Integer)
        Dim query As String = "SELECT customerid, bikeid, accessoriesid FROM payment_1 WHERE customerid = (SELECT customerid FROM customer_1 WHERE LOWER(username) = LOWER(@username))"

        Dim customerID As Integer = -1
        Dim bikeID As Integer = -1
        Dim accessoriesID As Integer = -1

        Try
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@username", username)

                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        customerID = If(IsDBNull(reader("customerid")), -1, Convert.ToInt32(reader("customerid")))
                        bikeID = If(IsDBNull(reader("bikeid")), -1, Convert.ToInt32(reader("bikeid")))
                        accessoriesID = If(IsDBNull(reader("accessoriesid")), -1, Convert.ToInt32(reader("accessoriesid")))
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If connection.State = ConnectionState.Open Then connection.Close()
        End Try

        Return (customerID, bikeID, accessoriesID)
    End Function

    ' Submit Feedback
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Ensure required fields are entered
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) Then
            MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Fetch Customer, Bike, and Accessories ID from Payment_1 Table
        Dim ids = GetCustomerBikeAccessoriesID(TextBox1.Text)
        Dim customerID As Integer = ids.Item1
        Dim bikeID As Integer = ids.Item2
        Dim accessoriesID As Integer = ids.Item3

        ' Validate retrieved IDs
        If customerID = -1 Then
            MessageBox.Show("Customer not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If bikeID = -1 Then
            MessageBox.Show("No bike found for this customer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If accessoriesID = -1 Then
            MessageBox.Show("No accessories found for this customer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Generate Feedback ID
        Dim feedbackID As Integer = GenerateRandomFeedbackID()
        Dim rating As Integer = TextBox2.Text
        Dim reviews As String = TextBox3.Text

        ' Validate rating input
        If Not Integer.TryParse(TextBox2.Text, rating) OrElse rating < 1 OrElse rating > 5 Then
            MessageBox.Show("Please enter a valid rating (1-5).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Insert into feedback_1 table
        Dim query As String = "INSERT INTO feedback_1 (feedbackid, customerid, bikeid, accessoriesid, rating, reviews) " &
                              "VALUES (@feedbackid, @customerid, @bikeid, @accessoriesid, @rating, @reviews)"

        Try
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@feedbackid", feedbackID)
                cmd.Parameters.AddWithValue("@customerid", customerID)
                cmd.Parameters.AddWithValue("@bikeid", bikeID)
                cmd.Parameters.AddWithValue("@accessoriesid", accessoriesID)
                cmd.Parameters.AddWithValue("@rating", rating)
                cmd.Parameters.AddWithValue("@reviews", reviews)

                connection.Open()
                cmd.ExecuteNonQuery()
            End Using

            ' Show success message
            MessageBox.Show("Feedback submitted successfully Thank you 😊", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error saving feedback: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If connection.State = ConnectionState.Open Then connection.Close()
        End Try
    End Sub
End Class

