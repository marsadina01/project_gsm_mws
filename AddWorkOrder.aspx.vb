Imports System.Data
Imports System
Imports System.Security.Cryptography
Imports System.Data.SqlClient

Partial Class AddWorkOrder
    Inherits System.Web.UI.Page

    Dim connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("name") Is Nothing Then
            Response.Redirect("default.aspx")
        End If

        If Not IsPostBack Then
            txtrequestor.Text = Session("namafull").ToString
            lblidreq.Text = Session("npk").ToString
            dataMoldTool("") ' Load dropdown kosong
        End If
    End Sub

    Protected Sub btnMold_Click(ByVal sender As Object, ByVal e As EventArgs)
        dataMoldTool("1")
    End Sub

    Protected Sub btnTool_Click(ByVal sender As Object, ByVal e As EventArgs)
        dataMoldTool("2")
    End Sub

    Private Sub dataMoldTool(ByVal type As String)
        Dim ddlMoldTool As DropDownList = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("ddlMoldTool"), DropDownList)
        If ddlMoldTool Is Nothing Then Exit Sub

        ' Reset dropdown & tambahkan item default
        ddlMoldTool.Items.Clear()
        ddlMoldTool.Items.Add(New ListItem("-- Pilih ID Mold / Tool --", "")) ' Tambahkan item default

        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT mold_id, mold_nama FROM db_purchasing.dbo.tlkp_mold WHERE mold_tipe = @Type AND mold_status = 1"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Type", type)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    ddlMoldTool.Items.Add(New ListItem(reader("mold_nama").ToString(), reader("mold_id").ToString()))
                End While
                conn.Close()
            End Using
        End Using

        ' Pastikan item pertama adalah placeholder, bukan data pertama dari database
        If ddlMoldTool.Items.Count > 1 Then
            ddlMoldTool.SelectedIndex = 0 ' Set ke caption "-- Pilih ID Mold / Tool --"
        End If
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