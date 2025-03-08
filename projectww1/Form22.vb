Imports System.Data.SqlClient

Public Class Form22
    Dim connectionString As String = "Data Source=HOME\SQLEXPRESS;Initial Catalog=wheelwiseproj1;Integrated Security=True"
    Dim conn As New SqlConnection(connectionString)
    Dim adapter As SqlDataAdapter
    Dim dt As New DataTable()

    Private Sub admin_dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        Try
            conn.Open()
            Dim query As String = "SELECT * FROM feedback_1"
            adapter = New SqlDataAdapter(query, conn)
            dt.Clear()
            adapter.Fill(dt)

            ' Load data into DataGridView inside TabControl
            DataGridView1.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim commandBuilder As New SqlCommandBuilder(adapter)
            adapter.Update(dt)
            MessageBox.Show("Data updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error updating data: " & ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        form13.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        form23.show()
        Me.Hide()
    End Sub
End Class