Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Cryptography

Partial Class DetailWorkOrder
    Inherits System.Web.UI.Page

    Dim connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("name") Is Nothing Then
            Response.Redirect("default.aspx")
        End If

        If Not IsPostBack Then
            If Request.QueryString("wor_no") IsNot Nothing Then
                Dim worNo As String = Request.QueryString("wor_no")
                LoadWorkOrderDetails()
                LoadMachineDropdown()
                LoadMoldToolDropdown()
                LoadUploadedFiles()
            Else
                Response.Write("<script>alert('Nomor Work Order tidak ditemukan!'); window.location='ViewWorkOrder.aspx';</script>")
            End If
        End If
    End Sub
    Private Sub LoadMachineDropdown()
        Dim query As String = "SELECT mach_id, mach_name FROM tlkp_machine"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    ddlmachine.DataSource = reader
                    ddlmachine.DataTextField = "mach_name"  ' Teks yang ditampilkan
                    ddlmachine.DataValueField = "mach_id"  ' Nilai yang akan dipilih
                    ddlmachine.DataBind()
                Catch ex As Exception
                    Response.Write("<script>alert('Error: " & ex.Message & "');</script>")
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Sub

    Private Sub LoadMoldToolDropdown()
        Dim query As String = "SELECT mold_id, mold_nama FROM tlkp_mold"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    ddlMoldTool.DataSource = reader
                    ddlMoldTool.DataTextField = "mold_nama"  ' Nama yang ditampilkan
                    ddlMoldTool.DataValueField = "mold_id"  ' Nilai yang akan disimpan
                    ddlMoldTool.DataBind()
                Catch ex As Exception
                    Response.Write("<script>alert('Error: " & ex.Message & "');</script>")
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Sub


    ' Fungsi untuk mengambil dan menampilkan detail Work Order
    Private Sub LoadWorkOrderDetails()
        Dim worNo As String = Request.QueryString("wor_no")

        If String.IsNullOrEmpty(worNo) Then
            Response.Write("<script>alert('Work Order Number tidak ditemukan!');</script>")
            Exit Sub
        End If

        Dim query As String = "SELECT u.spl_nama, w.wor_damage, w.wor_machine, w.wor_mold_tool, w.wor_repairby, w.wor_lampiran, w.wor_addnote, w.wor_status, w.wor_createby, w.wor_createdate FROM t_workorder w LEFT JOIN tlkp_supplier u ON w.wor_supplier = u.spl_id WHERE w.wor_no = @wor_no"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@wor_no", worNo)
                Try
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            txtrequestor.Text = If(IsDBNull(reader("spl_nama")), "", reader("spl_nama").ToString())
                            txtkerusakan.Text = If(IsDBNull(reader("wor_damage")), "", reader("wor_damage").ToString())
                            ddlmachine.SelectedValue = If(IsDBNull(reader("wor_machine")), "", reader("wor_machine").ToString())
                            ddlMoldTool.SelectedValue = If(IsDBNull(reader("wor_mold_tool")), "", reader("wor_mold_tool").ToString())
                            'Response.Write("<script>alert('Machine: " & reader("wor_machine").ToString() & "');</script>")
                            'Response.Write("<script>alert('Mold Tool: " & reader("wor_mold_tool").ToString() & "');</script>")


                            ' Handle checkbox untuk wor_repairby
                            Dim repairBy As String = If(IsDBNull(reader("wor_repairby")), "", reader("wor_repairby").ToString())

                            If repairBy = "GS" Then
                                chkRepairBy.Checked = True ' Jika GS, checkbox dicentang
                            Else
                                chkRepairBy.Checked = False ' Jika Supplier atau kosong, tidak dicentang
                            End If

                            'txtLampiran.Text = If(IsDBNull(reader("wor_lampiran")), "", reader("wor_lampiran").ToString())
                            txtketerangan.Text = If(IsDBNull(reader("wor_addnote")), "", reader("wor_addnote").ToString())
                        Else
                            Response.Write("<script>alert('Data Work Order tidak ditemukan!');</script>")
                        End If
                    End Using
                Catch ex As Exception
                    Response.Write("<script>alert('Error: " & ex.Message & "');</script>")
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Sub

    Private Sub LoadUploadedFiles()
        Dim filePath As String = "~/Uploads/"
        Dim fileName As String = ""
        Dim worNo As String = Request.QueryString("wor_no")


        Dim queryFile As String = "SELECT wor_lampiran FROM t_workorder WHERE wor_no = @wor_no"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(queryFile, conn)
                cmd.Parameters.AddWithValue("@wor_no", worNo)
                Try
                    conn.Open()
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                        fileName = result.ToString()
                    End If
                Catch ex As Exception
                    Response.Write("<script>alert('Error: " & ex.Message & "');</script>")
                Finally
                    conn.Close()
                End Try
            End Using
        End Using

        If Not String.IsNullOrEmpty(fileName) Then
            lnkLampiran.Visible = True
            lnkLampiran.NavigateUrl = filePath & fileName
            lnkLampiran.Text = "📄 Download Lampiran"
        Else
            lnkLampiran.Visible = False
        End If

    End Sub


    ' Fungsi untuk menangani tombol Approve
    Protected Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        Dim worNo As String = Request.QueryString("wor_no") ' Ambil Work Order No dari URL
        Dim npk As String = Session("npk") ' Ambil NPK dari session login
        Dim approveDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        If String.IsNullOrEmpty(worNo) Or String.IsNullOrEmpty(npk) Then
            Response.Write("<script>alert('Data tidak valid!');</script>")
            Exit Sub
        End If

        Dim connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()

            Try
                ' Cek apakah sudah pernah approve sebelumnya
                Dim queryCheck As String = "SELECT COUNT(*) FROM t_detailworkorder WHERE dt_wor_no = @wor_no AND dt_approve1by = @npk"
                Using cmdCheck As New SqlCommand(queryCheck, conn, transaction)
                    cmdCheck.Parameters.AddWithValue("@wor_no", worNo)
                    cmdCheck.Parameters.AddWithValue("@npk", npk)
                    Dim count As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

                    ' Jika belum ada approval dari user ini, lanjut insert
                    If count = 0 Then
                        Dim queryInsert As String = "INSERT INTO t_detailworkorder (dt_wor_no, dt_approve1by, dt_approve1date) VALUES(@wor_no, @npk, @approveDate)"
                        Using cmdInsert As New SqlCommand(queryInsert, conn, transaction)
                            cmdInsert.Parameters.AddWithValue("@wor_no", worNo)
                            cmdInsert.Parameters.AddWithValue("@npk", npk)
                            cmdInsert.Parameters.AddWithValue("@approveDate", approveDate)
                            cmdInsert.ExecuteNonQuery()
                        End Using
                    End If
                End Using

                ' Update Status Work Order
                Dim queryUpdate As String = "UPDATE t_workorder SET wor_status = 2 WHERE wor_no = @wor_no"
                Using cmdUpdate As New SqlCommand(queryUpdate, conn, transaction)
                    cmdUpdate.Parameters.AddWithValue("@wor_no", worNo)
                    cmdUpdate.ExecuteNonQuery()
                End Using

                transaction.Commit()
                Dim script As String = "<script>" & Environment.NewLine &
                       "Swal.fire({" & Environment.NewLine &
                       "    icon: 'success'," & Environment.NewLine &
                       "    title: 'Success!'," & Environment.NewLine &
                       "    text: 'Anda telah menyetujui request ini'," & Environment.NewLine &
                       "    confirmButtonColor: '#28a745'," & Environment.NewLine &
                       "    allowOutsideClick: false" & Environment.NewLine &
                       "}).then((result) => {" & Environment.NewLine &
                       "    if (result.isConfirmed) {" & Environment.NewLine &
                       "        window.location='ViewWorkOrder.aspx';" & Environment.NewLine &
                       "    }" & Environment.NewLine &
                       "});" & Environment.NewLine &
                       "</script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "approvalSuccess", script)

            Catch ex As Exception
                transaction.Rollback()
                Response.Write("<script>alert('Error: " & ex.Message.Replace("'", "\'") & "');</script>")
            Finally
                conn.Close()
            End Try
        End Using
    End Sub


    ' Fungsi untuk menangani tombol Reject
    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim rejectReason As String = txtRejectReason.Text ' Ambil alasan reject dari textarea
        If String.IsNullOrEmpty(rejectReason) Then
            Response.Write("<script>alert('Alasan Reject harus diisi!');</script>")
            Exit Sub
        End If

        Dim worNo As String = Request.QueryString("wor_no")
        Dim npk As String = Session("npk") ' NPK user login
        Dim rejectDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        If String.IsNullOrEmpty(worNo) Or String.IsNullOrEmpty(npk) Then
            Response.Write("<script>alert('Data tidak valid!');</script>")
            Exit Sub
        End If

        Dim connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()

            Try
                ' Cek status approval dari tabel utama
                Dim queryCheckStatus As String = "SELECT wor_status FROM t_workorder WHERE wor_no = @wor_no"
                Dim status As Integer

                Using cmdCheckStatus As New SqlCommand(queryCheckStatus, conn, transaction)
                    cmdCheckStatus.Parameters.AddWithValue("@wor_no", worNo)
                    Dim result As Object = cmdCheckStatus.ExecuteScalar()
                    status = If(result IsNot Nothing, Convert.ToInt32(result), -1)
                End Using

                ' Jika status sudah 2 (Approved), tidak bisa reject
                If status = 2 Then
                    Response.Write("<script>alert('Work Order sudah disetujui dan tidak dapat ditolak!');</script>")
                    transaction.Rollback()
                    conn.Close()
                    Exit Sub
                ElseIf status <> 1 Then
                    Response.Write("<script>alert('Status tidak valid untuk ditolak!');</script>")
                    transaction.Rollback()
                    conn.Close()
                    Exit Sub
                End If

                ' Simpan data ke tabel t_detailworkorder
                Dim queryInsert As String = "INSERT INTO t_detailworkorder (dt_wor_no, dt_alasanreject, dt_rejectby) VALUES(@wor_no, @reason, @npk)"
                Using cmdInsert As New SqlCommand(queryInsert, conn, transaction)
                    cmdInsert.Parameters.AddWithValue("@wor_no", worNo)
                    cmdInsert.Parameters.AddWithValue("@reason", rejectReason)
                    cmdInsert.Parameters.AddWithValue("@npk", npk)
                    cmdInsert.ExecuteNonQuery()
                End Using

                ' Commit transaksi
                transaction.Commit()

                ' Tampilkan SweetAlert jika berhasil
                Dim script As String = "<script>" & Environment.NewLine &
               "Swal.fire({" & Environment.NewLine &
               "    icon: 'error'," & Environment.NewLine &
               "    title: 'Rejected!'," & Environment.NewLine &
               "    text: 'Work Order telah ditolak'," & Environment.NewLine &
               "    confirmButtonColor: '#d33'," & Environment.NewLine &
               "    allowOutsideClick: false" & Environment.NewLine &
               "}).then((result) => {" & Environment.NewLine &
               "    if (result.isConfirmed) {" & Environment.NewLine &
               "        window.location='ViewWorkOrder.aspx';" & Environment.NewLine &
               "    }" & Environment.NewLine &
               "});" & Environment.NewLine &
               "</script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "rejectSuccess", script)

            Catch ex As Exception
                transaction.Rollback()
                Response.Write("<script>alert('Error: " & ex.Message.Replace("'", "\'") & "');</script>")
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    ' Fungsi untuk mendekripsi string koneksi database
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