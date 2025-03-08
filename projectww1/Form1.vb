Imports System.Data.SqlClient
Public Class login_form
    Dim connectionString As String = "Data Source=HOME\SQLEXPRESS;Initial Catalog=wheelwiseproj1;Integrated Security=True"
    Dim connection As New SqlConnection(connectionString)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox2.Items.Add("admin")
        ComboBox2.Items.Add("showroom")
        ComboBox2.Items.Add("customer")
        ComboBox2.SelectedIndex = 0 ' Default selection
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim role As String = ComboBox2.SelectedItem.ToString()
        Dim username As String = TextBox3.Text
        Dim password As String = TextBox4.Text
        ValidateUser(role, username, password)
    End Sub

    Private Sub ValidateUser(role As String, username As String, password As String)
        Try
            Dim query As String = ""
            Select Case role.ToLower()
                Case "admin"
                    query = "SELECT COUNT(*) FROM admin_1 WHERE username = @username AND password = @password"
                Case "showroom"
                    query = "SELECT COUNT(*) FROM showroom_1 WHERE username = @username AND password = @password"
                Case "customer"
                    query = "SELECT COUNT(*) FROM customer_1 WHERE username = @username AND password = @password"
            End Select

            If query <> "" Then
                Dim command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@username", username)
                command.Parameters.AddWithValue("@password", password)

                connection.Open()
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                connection.Close()

                If count > 0 Then
                    MessageBox.Show("Login Successful! Redirecting...")
                    ' Open the respective form based on role
                    Select Case role.ToLower()
                        Case "admin"
                            Dim adminForm As New form13()
                            form13.Show()
                        Case "showroom"
                            Dim showroomForm As New form13()
                            showroomForm.Show()
                        Case "customer"
                            Dim customerForm As New customer_dashboard()
                            customer_dashboard.Show()
                    End Select
                    Me.Hide()
                Else
                    MessageBox.Show("Wrong username or password!")
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If connection.State = ConnectionState.Open Then
                connection.Close()
            End If
        End Try
    End Sub
    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        signup.Show()
        Me.Hide()
    End Sub
End Class