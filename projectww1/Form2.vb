Imports System.Data.SqlClient

Public Class signup
    Private connectionString As String = "Data Source=HOME\SQLEXPRESS;Initial Catalog=wheelwiseproj1;Integrated Security=True"

    ' ✅ Generate a Random CustomerID
    Private Function GenerateCustomerID() As Integer
        Dim random As New Random()
        Return random.Next(100000, 999999) ' Generates a 6-digit ID
    End Function

    ' ✅ INSERT CUSTOMER INTO DATABASE
    Private Sub button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim customerID As Integer = GenerateCustomerID()
        Dim username As String = TextBox1.Text.Trim()
        Dim emailid As String = TextBox2.Text.Trim()
        Dim phoneno As String = TextBox3.Text.Trim()
        Dim address As String = TextBox4.Text.Trim()
        Dim password As String = TextBox5.Text.Trim()
        Dim confirmPassword As String = TextBox6.Text.Trim()

        ' ✅ Check if passwords match
        If password <> confirmPassword Then
            MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' ✅ Check required fields
        If String.IsNullOrEmpty(name) OrElse String.IsNullOrEmpty(emailid) Then
            MessageBox.Show("Please enter name and email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim query As String = "INSERT INTO Customer_1 (CustomerID, username, Emailid, phoneno, address, password) VALUES (@CustomerID, @username, @Emailid, @phoneno, @address, @password)"

        Try
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@CustomerID", customerID)
                    cmd.Parameters.AddWithValue("@username", username)
                    cmd.Parameters.AddWithValue("@Emailid", emailid)
                    cmd.Parameters.AddWithValue("@phoneno", phoneno)
                    cmd.Parameters.AddWithValue("@address", address)
                    cmd.Parameters.AddWithValue("@password", password)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Customer Registered! ID: " & customerID, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            customer_dashboard.Show()
        Catch ex As Exception
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub signup_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class