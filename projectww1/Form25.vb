Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form25
    ' Database connection
    Dim connection As New SqlConnection("Server=HOME\SQLEXPRESS;Database=wheelwiseproj1;Integrated Security=True")

    ' Form Load - Load bike models, accessories, and payment modes
    Private Sub Form25_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBikeModels()
        LoadAccessories()
        LoadPaymentMode()
    End Sub

    ' Load bike models into ComboBox
    Private Sub LoadBikeModels()
        Dim query As String = "SELECT bikeid, modelname FROM bikes_1"
        Dim adapter As New SqlDataAdapter(query, connection)
        Dim table As New DataTable()

        connection.Open()
        adapter.Fill(table)
        connection.Close()

        ComboBox1.DataSource = table
        ComboBox1.DisplayMember = "modelname"
        ComboBox1.ValueMember = "bikeid" ' Store bike ID
    End Sub

    ' Load accessories into ComboBox
    Private Sub LoadAccessories()
        Dim query As String = "SELECT accessoriesid, name FROM accessories_1"
        Dim adapter As New SqlDataAdapter(query, connection)
        Dim table As New DataTable()

        connection.Open()
        adapter.Fill(table)
        connection.Close()

        ComboBox2.DataSource = table
        ComboBox2.DisplayMember = "name"
        ComboBox2.ValueMember = "accessoriesid" ' Store accessory ID
    End Sub

    ' Load payment modes into ComboBox
    Private Sub LoadPaymentMode()
        ComboBox3.Items.Clear()
        ComboBox3.Items.Add("Cash")
        ComboBox3.Items.Add("Card")
    End Sub

    ' Fetch Bike Price when a Model is selected
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ' Ensure SelectedValue is valid before proceeding
        If ComboBox1.SelectedValue Is Nothing OrElse Not IsNumeric(ComboBox1.SelectedValue) Then
            TextBox1.Text = "0" ' Set default price if selection is invalid
            Exit Sub
        End If

        ' Convert selected value to Integer safely
        Dim selectedBikeID As Integer = Convert.ToInt32(ComboBox1.SelectedValue)
        Dim query As String = "SELECT price FROM bikes_1 WHERE bikeid = @bikeid"

        Try
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@bikeid", selectedBikeID)

                connection.Open()
                Dim price As Object = cmd.ExecuteScalar()
                connection.Close()

                ' Set price if not null, otherwise default to 0
                TextBox1.Text = If(price IsNot Nothing AndAlso IsNumeric(price), price.ToString(), "0")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching bike price: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        CalculateTotal() ' Update total price
    End Sub

    ' Fetch Accessory Price when an Accessory is selected
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        ' Ensure SelectedValue is valid before proceeding
        If ComboBox2.SelectedValue Is Nothing OrElse Not IsNumeric(ComboBox2.SelectedValue) Then
            TextBox2.Text = "0" ' Set default price if selection is invalid
            Exit Sub
        End If

        ' Convert selected value to Integer safely
        Dim selectedAccessoryID As Integer = Convert.ToInt32(ComboBox2.SelectedValue)
        Dim query As String = "SELECT price_of_accessories FROM accessories_1 WHERE accessoriesid = @accessoriesid"

        Try
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@accessoriesid", selectedAccessoryID)

                connection.Open()
                Dim price As Object = cmd.ExecuteScalar()
                connection.Close()

                ' Set price if not null, otherwise default to 0
                TextBox2.Text = If(price IsNot Nothing AndAlso IsNumeric(price), price.ToString(), "0")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching accessory price: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        CalculateTotal() ' Update total price
    End Sub


    ' Calculate total price (Bike + Accessory)
    Private Sub CalculateTotal()
        Dim bikePrice, accessoryPrice As Decimal

        ' Convert TextBox values to Decimal (handle empty values)
        Decimal.TryParse(TextBox1.Text, bikePrice)
        Decimal.TryParse(TextBox2.Text, accessoryPrice)

        Dim total As Decimal = bikePrice + accessoryPrice
        TextBox4.Text = total.ToString("0.00") ' Display total price in TextBox4
    End Sub

    ' Fetch current user's Customer ID
    Public Function GetCustomerID(customerName As String) As Integer
        Dim customerID As Integer = -1 ' Default value (-1 means not found)

        ' Trim whitespace to avoid input issues
        customerName = customerName.Trim()

        ' Debugging Step: Show the name being searched
        MessageBox.Show("Searching for customer: " & customerName)

        ' Ensure customer name is not empty
        If String.IsNullOrWhiteSpace(customerName) Then
            MessageBox.Show("Please enter a valid customer name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return customerID
        End If

        ' Correct SQL Query
        Dim query As String = "SELECT * FROM customer_1 WHERE username = @name;"

        Try
            Using connection As New SqlConnection("Server=HOME\SQLEXPRESS;Database=wheelwiseproj1;Integrated Security=True")
                Using command As New SqlCommand(query, connection)
                    ' Use parameterized query
                    command.Parameters.AddWithValue("@name", customerName)

                    connection.Open()
                    Dim result As Object = command.ExecuteScalar()
                    connection.Close()

                    ' Debugging: Show query result
                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        customerID = Convert.ToInt32(result)
                        MessageBox.Show("Customer found! ID: " & customerID)
                    Else
                        MessageBox.Show("No customer found with the name: " & customerName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return customerID
    End Function


    ' Generate Random Payment ID
    Private Function GenerateRandomPaymentID() As Integer
        Dim rnd As New Random()
        Return rnd.Next(10000, 99999) ' Generate 5-digit random ID
    End Function

    ' Show card details textbox only if "Card" is selected
    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        TextBox3.Visible = (ComboBox3.Text = "Card")

        If ComboBox3.Text <> "Card" Then
            TextBox3.Text = "" ' Clear card details when cash is selected
        End If
    End Sub

    ' Process Payment
    ' Process Payment
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Ensure required fields are selected
        If ComboBox1.SelectedIndex = -1 OrElse TextBox1.Text = "" OrElse ComboBox3.SelectedIndex = -1 Then
            MessageBox.Show("Please select all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Fetch values from UI
        Dim enteredName As String = TextBox5.Text ' Get name from TextBox5
        Dim customerID As Integer = GetCustomerID(enteredName) ' Get customer ID from DB
        If customerID = -1 Then Exit Sub ' Stop if customer not found

        Dim PaymentID As Integer = GenerateRandomPaymentID()
        Dim BikeID As Integer = Convert.ToInt32(ComboBox1.SelectedValue)
        Dim AccessoriesID As Integer = Convert.ToInt32(ComboBox2.SelectedValue)
        Dim Modelname As String = ComboBox1.Text
        Dim Price As Decimal = Convert.ToDecimal(TextBox1.Text)
        Dim Name As String = If(ComboBox2.Text <> "", ComboBox2.Text, "None")
        Dim price_of_accessories As Decimal = If(TextBox2.Text <> "", Convert.ToDecimal(TextBox2.Text), 0)
        Dim paymentMode As String = ComboBox3.Text
        Dim cardDetails As String = If(paymentMode = "Card", TextBox3.Text, "N/A")
        Dim totalAmount As Decimal = Convert.ToDecimal(TextBox4.Text)

        ' Insert into payment_1 table
        Dim query As String = "INSERT INTO payment_1 (paymentid, customerid, bikeid, accessoriesid, amount, name, paymentmode) " &
                              "VALUES (@paymentid, @customerid, @bikeid, @accessoriesid, @amount, @name, @paymentmode)"

        Try
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@paymentid", PaymentID)
                cmd.Parameters.AddWithValue("@customerid", customerID)
                cmd.Parameters.AddWithValue("@bikeid", BikeID)
                cmd.Parameters.AddWithValue("@accessoriesid", AccessoriesID)
                cmd.Parameters.AddWithValue("@amount", totalAmount)
                cmd.Parameters.AddWithValue("@name", Name)
                cmd.Parameters.AddWithValue("@paymentmode", paymentMode)

                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()

                ' Show confirmation
                MessageBox.Show("Payment successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error saving payment: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form12.Show()
    End Sub
End Class

