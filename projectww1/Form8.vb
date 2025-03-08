Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles
Public Class Form8
    ' SQL Connection String
    Private connectionString As String = "Server=HOME\SQLEXPRESS;Database=wheelwiseproj1;Integrated Security=True;"

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBikesIntoComboBoxes()
    End Sub

    ' Load bikes into ComboBoxes (Fixes duplicate issue)
    Private Sub LoadBikesIntoComboBoxes()
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()
                Dim query As String = "SELECT DISTINCT modelname FROM bikes_1"
                Using cmd As New SqlCommand(query, con)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ComboBox1.Items.Clear()
                        ComboBox2.Items.Clear()
                        While reader.Read()
                            Dim modelName As String = reader("ModelName").ToString()
                            ComboBox1.Items.Add(modelName)
                            ComboBox2.Items.Add(modelName)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading bikes: " & ex.Message)
        End Try
    End Sub

    ' Compare bikes and display details in a structured MessageBox
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ComboBox1.SelectedItem Is Nothing Or ComboBox2.SelectedItem Is Nothing Then
            MessageBox.Show("Please select two bikes to compare.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Fetch details for both bikes
        Dim bike1Details() As String = FetchBikeDetails(ComboBox1.SelectedItem.ToString())
        Dim bike2Details() As String = FetchBikeDetails(ComboBox2.SelectedItem.ToString())

        ' Properly formatted output with aligned columns
        Dim message As String = "----------------- BIKE 1 -----------------" & vbCrLf &
                            "Model: ".PadRight(15) & bike1Details(0) & vbCrLf &
                            "Price: ".PadRight(15) & bike1Details(1) & vbCrLf &
                            "Engine: ".PadRight(15) & bike2Details(2) & vbCrLf &
                            "Type: ".PadRight(15) & bike1Details(3) & vbCrLf &
                            "Manufacturer: ".PadRight(15) & bike1Details(4) & vbCrLf &
                            "------------------------------------------" & vbCrLf &
                            "----------------- BIKE 2 -----------------" & vbCrLf &
                            "Model: ".PadRight(15) & bike2Details(0) & vbCrLf &
                            "Price: ".PadRight(15) & bike2Details(1) & vbCrLf &
                            "Engine: ".PadRight(15) & bike2Details(2) & vbCrLf &
                            "Type: ".PadRight(15) & bike2Details(3) & vbCrLf &
                            "Manufacturer: ".PadRight(15) & bike2Details(4) & vbCrLf &
                            "------------------------------------------"
        MessageBox.Show(message, "Bike Comparison", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' Function to fetch bike details from database and return them as an array
    Private Function FetchBikeDetails(selectedBike As String) As String()
        Try
            Using con As New SqlConnection(connectionString)
                con.Open()
                Dim query As String = "SELECT ModelName, Price, EngineSpec, Type, Manufacturer FROM bikes_1 WHERE ModelName = @ModelName"
                Using cmd As New SqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@ModelName", selectedBike)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Return {reader("ModelName").ToString(),
                                    reader("Price").ToString(),
                                    reader("EngineSpec").ToString(),
                                    reader("Type").ToString(),
                                    reader("Manufacturer").ToString()}
                        Else
                            Return {"Data Not Found!", "", "", "", ""}
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Return {"Error fetching details!", "", "", "", ""}
        End Try
    End Function

    ' Truncate long text (Engine Spec) and add "..." if it's too long
    Private Function TruncateText(text As String, maxLength As Integer) As String
        If text.Length > maxLength Then
            Return text.Substring(0, maxLength) & "..."
        Else
            Return text
        End If
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        customer_dashboard.Show()
        Me.Hide()
    End Sub
End Class
