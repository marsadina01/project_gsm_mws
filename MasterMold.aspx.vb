
Imports System.Data
Imports System
Imports System.Security.Cryptography
Imports System.Data.SqlClient

Partial Class MasterMold
    Inherits System.Web.UI.Page

    Dim connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("name") Is Nothing Then
            Response.Redirect("default.aspx")
        End If
        'If Session("role") <> "admin" Then
        '    Response.Redirect("login.aspx")
        'End If
        If Not IsPostBack Then

            data()
        End If
    End Sub

    Public Sub data()
        Dim MatComm As New SqlCommand
        Dim dv As DataView
        Dim datetime As New Date
        Dim dtab As New DataTable
        Dim oradap As New SqlDataAdapter

        Dim conn As New SqlConnection(connStr)
        conn.Open()

        Dim MatSql As String = "select * from db_purchasing.dbo.tlkp_mold where mold_status = 1"

        MatComm.Connection = conn
        MatComm.CommandText = MatSql
        oradap.SelectCommand = MatComm
        oradap.Fill(dtab)
        'gvdata.DataSource = dtab
        dv = dtab.DefaultView

        'dtab.Columns.Add("outstanding")

        If dtab.Rows.Count > 0 Then

            Repeater1.DataSource = dv
            Me.Repeater1.DataBind()
            'gvdata.DataBind()
        Else
            Repeater1.DataSource = Me.Get_EmptyDataTable()
            Repeater1.DataBind()
        End If

        conn.Close()
    End Sub

    Public Function Get_EmptyDataTable() As DataTable
        Dim dtEmpty As New DataTable()
        'Here ensure that you have added all the column available in your gridview
        dtEmpty.Columns.Add("mold_id", GetType(String))
        dtEmpty.Columns.Add("mold_nama", GetType(String))
        dtEmpty.Columns.Add("mold_tipe", GetType(String))

        Dim datatRow As DataRow = dtEmpty.NewRow()


        'Inserting a new row,datatable .newrow creates a blank row
        dtEmpty.Rows.Add(datatRow)
        'adding row to the datatable
        Return dtEmpty
    End Function

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim successMessage As String = "Data berhasil disimpan"
        Dim errorMessage As String = "Terjadi kesalahan, data tidak tersimpan"
        Dim url As String = "MasterMold.aspx"

        Dim con As New SqlConnection(connStr)
        Dim strQuery As String = "INSERT INTO db_purchasing.dbo.tlkp_mold(mold_nama,mold_tipe,mold_status,mold_createdate,mold_createby) values ('" & txtname.Text & "', '" & ddlType.SelectedValue & "',1,GETDATE(),'" & Session("npk") & "')"

        Dim cmd As New SqlCommand(strQuery)


        cmd.CommandType = CommandType.Text
        cmd.Connection = con

        Try
            con.Open()
            cmd.ExecuteNonQuery()
            txtname.Text = ""
            ddlType.SelectedIndex = ""
            data()

            ' Tampilkan pesan sukses
            Dim successScript As String = "window.onload = function(){ alert('" & successMessage & "'); window.location = '" & url & "'; }"
            ClientScript.RegisterStartupScript(Me.GetType(), "Success", successScript, True)

        Catch ex As Exception
            ' Jika terjadi kesalahan, tampilkan pesan error
            Dim errorScript As String = "window.onload = function(){ alert('" & errorMessage & "'); window.location = '" & url & "'; }"
            ClientScript.RegisterStartupScript(Me.GetType(), "Error", errorScript, True)
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Sub

    Private Sub ToggleElements(ByVal item As RepeaterItem, ByVal isEdit As Boolean)
        'Toggle Buttons.
        item.FindControl("lbubah").Visible = Not isEdit
        item.FindControl("lbsimpan").Visible = isEdit
        item.FindControl("lbcancel").Visible = isEdit

        'Toggle Labels.
        item.FindControl("lblsite_nama").Visible = Not isEdit
        item.FindControl("lblsite_tipe").Visible = Not isEdit

        'Toggle TextBoxes and DropDownList.
        item.FindControl("txtsite_nama").Visible = isEdit
        item.FindControl("ddlsite_tipe").Visible = isEdit
    End Sub

    Protected Sub OnEdit(ByVal sender As Object, ByVal e As EventArgs)
        'Find the reference of the Repeater Item.
        Dim item As RepeaterItem = TryCast(TryCast(sender, LinkButton).NamingContainer, RepeaterItem)
        Me.ToggleElements(item, True)
    End Sub

    Protected Sub OnUpdate(ByVal sender As Object, ByVal e As EventArgs)
        Dim item As RepeaterItem = TryCast(TryCast(sender, LinkButton).Parent, RepeaterItem)

        Dim lblid As Label = DirectCast(item.FindControl("lblid"), Label)
        Dim txtsite_nama As TextBox = DirectCast(item.FindControl("txtsite_nama"), TextBox)
        Dim ddlsite_tipe As DropDownList = DirectCast(item.FindControl("ddlsite_tipe"), DropDownList)

        Dim con As New SqlConnection(connStr)
        Dim MySQLQuery As String = "Update db_purchasing.dbo.tlkp_mold SET mold_nama='" & Trim(txtsite_nama.Text) & "',mold_tipe=" & ddlsite_tipe.SelectedValue & ", mold_modifdate=GETDATE(), mold_modifby='" & Session("npk") & "' where mold_id=" & Trim(lblid.Text)
        Dim cmd As New SqlCommand(MySQLQuery)

        cmd.CommandType = CommandType.Text
        cmd.Connection = con

        Try
            con.Open()
            cmd.ExecuteNonQuery()


        Catch ex As Exception
            Response.Write(ex.Message)
            'ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", Script, True)
        Finally
            con.Close()
            con.Dispose()
        End Try

        data()

    End Sub

    Protected Sub OnCancel(ByVal sender As Object, ByVal e As EventArgs)
        'Find the reference of the Repeater Item.
        Dim item As RepeaterItem = TryCast(TryCast(sender, LinkButton).Parent, RepeaterItem)
        Me.ToggleElements(item, False)

    End Sub

    Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim message As String = "Sorry data can not be deleted because it is related to another"
        Dim url As String = "MasterMold.aspx"
        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"

        Dim item As RepeaterItem = TryCast(TryCast(sender, LinkButton).Parent, RepeaterItem)


        Dim id As Integer = Integer.Parse(TryCast(item.FindControl("lblid"), Label).Text)


        Dim con As New SqlConnection(connStr)

        Dim MySQLQuery As String = "Update db_purchasing.dbo.tlkp_mold SET mold_status='0', mold_modifDate=GETDATE(), mold_modifBy='" & Session("npk") & "' where mold_id=" & id
        Dim cmd As New SqlCommand(MySQLQuery)

        cmd.CommandType = CommandType.Text
        cmd.Connection = con

        Try
            con.Open()
            cmd.ExecuteNonQuery()


        Catch ex As Exception
            Response.Write(ex.Message)
            'ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", Script, True)
        Finally
            con.Close()
            con.Dispose()
        End Try


        data()
    End Sub

    Public Shared Function DecryptString(ByVal encryptedString As String) As String
        Dim DES As New TripleDESCryptoServiceProvider
        Dim mKey As String = "PasswordKey"
        DES.Key = MD5Hash(mKey)
        DES.Mode = CipherMode.ECB
        Dim Buffer As Byte() = Convert.FromBase64String(encryptedString)
        Return ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length))
    End Function

    Public Shared Function MD5Hash(ByVal value As String) As Byte()
        Dim MD5 As New MD5CryptoServiceProvider
        Return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value))
    End Function

End Class
