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
            Dim worNo As String = Request.QueryString("wor_no")

            ' Tentukan tombol berdasarkan wor_no
            If Not String.IsNullOrEmpty(worNo) Then
                SetActiveButton(worNo)
                SetMoldToolLabel(worNo)
            End If

            LoadWorkOrderDetails()
            LoadMachineDropdown()
            LoadMoldToolDropdown()
            LoadUploadedFiles()
            LoadTimeline()

            ' Cek status Work Order dari database
            Dim worStatus As Integer = GetWorkOrderStatus(worNo)

            If worStatus = 1 Then
                If Session("role") = "atsreq" Then
                    detail1.Visible = True
                    detail2.Visible = False
                    fclose.Visible = False
                    btnApprove.Visible = True
                    btnReject.Visible = True
                    btnCancel.Visible = False
                    btnClose.Visible = False
                Else
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = False
                    btnApprove.Visible = False
                    btnReject.Visible = False
                    btnCancel.Visible = True
                    btnClose.Visible = False
                End If

            ElseIf worStatus = 3 Then
                If Session("role") = "teknisiSup" Or Session("role") = "teknisiGS" Then
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = True
                    btnApprove.Visible = False
                    btnReject.Visible = False
                    btnCancel.Visible = True
                    btnClose.Visible = True
                Else
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = False
                    btnApprove.Visible = False
                    btnReject.Visible = False
                    btnCancel.Visible = True
                    btnClose.Visible = False
                End If

            ElseIf worStatus = 4 Then
                If Session("role") = "atstekSup" Or Session("role") = "atstekGS" Then
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = True
                    btnApprove.Visible = True
                    btnReject.Visible = True
                    btnCancel.Visible = False
                    btnClose.Visible = False
                Else
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = True
                    btnApprove.Visible = False
                    btnReject.Visible = False
                    btnCancel.Visible = True
                    btnClose.Visible = False
                End If

            Else
                detail1.Visible = False
                detail2.Visible = True
                fclose.Visible = True
                btnApprove.Visible = False
                btnReject.Visible = False
                btnCancel.Visible = True
                btnClose.Visible = False
            End If
        Else
            LoadWorkOrderDetails()
            LoadMachineDropdown()
            LoadMoldToolDropdown()
            LoadUploadedFiles()
            LoadTimeline()
        End If
    End Sub

    Private Function GetWorkOrderStatus(ByVal worNo As String) As Integer
        Dim status As Integer = 0

        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT wor_status FROM db_purchasing.dbo.t_workorder WHERE wor_no = @worNo"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@worNo", worNo)
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    status = Convert.ToInt32(result)
                End If
            End Using
        End Using

        Return status
    End Function

    Private Sub LoadMachineDropdown()
        Dim query As String = "SELECT id, mesin_nomor FROM db_maintenance.dbo.tlkp_mesin"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    ddlmachine.DataSource = reader
                    ddlmachine.DataTextField = "mesin_nomor"  ' Teks yang ditampilkan
                    ddlmachine.DataValueField = "id"  ' Nilai yang akan dipilih
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
        Dim query As String = "SELECT mold_id, mold_name FROM db_master_data.dbo.tlkp_mnt WHERE mold_status = 1"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    ddlMoldTool.DataSource = reader
                    ddlMoldTool.DataTextField = "mold_name"  ' Nama yang ditampilkan
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

        Dim query As String = "SELECT " +
                                "u.spl_nama, " +
                                "w.wor_damage, " +
                                "w.wor_machine, " +
                                "m.mesin_nomor, " +
                                "w.wor_mold_tool, " +
                                "t.mold_name, " +
                                "w.wor_repairby, " +
                                "w.wor_lampiran, " +
                                "w.wor_addnote, " +
                                "w.wor_status, " +
                                "w.wor_createby, " +
                                "w.wor_createdate, " +
                                "w.wor_stok, " +
                                "w.wor_total_order, " +
                                "w.wor_tglproduksi, " +
                                "w.wor_analisa, " +
                                "w.wor_perbaikan " +
                            "FROM db_purchasing.dbo.t_workorder w JOIN db_purchasing.dbo.tlkp_supplier u ON w.wor_supplier = u.spl_id " +
                                "JOIN db_maintenance.dbo.tlkp_mesin m ON w.wor_machine = m.id " +
                                "JOIN db_master_data.dbo.tlkp_mnt t ON w.wor_mold_tool = t.mold_id WHERE w.wor_no = @wor_no"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@wor_no", worNo)
                Try
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            txtrequestor.Text = If(IsDBNull(reader("spl_nama")), "", reader("spl_nama").ToString())
                            lblrequestor.Text = If(IsDBNull(reader("spl_nama")), "", reader("spl_nama").ToString())
                            lblmesin.Text = If(IsDBNull(reader("mesin_nomor")), "", reader("mesin_nomor").ToString())
                            lblmold.Text = If(IsDBNull(reader("mold_name")), "", reader("mold_name").ToString())
                            txtkerusakan.Text = If(IsDBNull(reader("wor_damage")), "", reader("wor_damage").ToString())
                            ddlmachine.SelectedValue = If(IsDBNull(reader("wor_machine")), "", reader("wor_machine").ToString())
                            ddlMoldTool.SelectedValue = If(IsDBNull(reader("wor_mold_tool")), "", reader("wor_mold_tool").ToString())

                            ' Handle checkbox untuk wor_repairby
                            Dim repairBy As String = If(IsDBNull(reader("wor_repairby")), "", reader("wor_repairby").ToString())

                            If repairBy = "GS" Then
                                chkRepairBy.Checked = True ' Jika GS, checkbox dicentang
                            Else
                                chkRepairBy.Checked = False ' Jika Supplier atau kosong, tidak dicentang
                            End If

                            txtketerangan.Text = If(IsDBNull(reader("wor_addnote")), "", reader("wor_addnote").ToString())
                            txtStok.Text = If(IsDBNull(reader("wor_stok")), "", reader("wor_stok").ToString())
                            txtTotalOrder.Text = If(IsDBNull(reader("wor_total_order")), "", reader("wor_total_order").ToString())
                            txtTglProduksi.Text = If(IsDBNull(reader("wor_tglproduksi")), "", reader("wor_tglproduksi").ToString())

                            ' Ambil Data Analisa Kejadian & Perbaikan
                            Dim analisa As String = If(IsDBNull(reader("wor_analisa")), "", reader("wor_analisa").ToString())
                            Dim perbaikan As String = If(IsDBNull(reader("wor_perbaikan")), "", reader("wor_perbaikan").ToString())

                            ' Cek role pengguna dari session
                            Dim userRole As String = Session("role")

                            ' Jika role adalah teknisi (teknisiSup atau teknisiGS)
                            Dim isTeknisi As Boolean = (userRole = "teknisiSup" Or userRole = "teknisiGS")

                            ' Logika Enable/Disable TextBox
                            If String.IsNullOrEmpty(analisa) Then
                                txtanalisa.Enabled = isTeknisi ' Hanya teknisi yang bisa edit
                            Else
                                txtanalisa.Text = analisa
                                txtanalisa.Enabled = False ' Sudah terisi, tidak bisa diedit
                            End If

                            If String.IsNullOrEmpty(perbaikan) Then
                                txtperbaikan.Enabled = isTeknisi ' Hanya teknisi yang bisa edit
                            Else
                                txtperbaikan.Text = perbaikan
                                txtperbaikan.Enabled = False ' Sudah terisi, tidak bisa diedit
                            End If
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
            Dim fileUrl As String = ResolveUrl(filePath & fileName)
            lnkLampiran.Visible = True
            lnkLampiran.NavigateUrl = "#" ' Prevent default link behavior
            lnkLampiran.Attributes.Add("onclick", "showLampiranModal('" & fileUrl & "'); return false;")
            lnkLampiran.Text = "📄 Lihat Lampiran"
        Else
            lnkLampiran.Visible = False
        End If
    End Sub

    Private Sub LoadTimeline()
        Dim worNo As String = Request.QueryString("wor_no")

        Dim query As String = "SELECT " +
                                    "d.dt_level, " +
                                    "d.dt_createdate, " +
                                    "u1.user_nama AS dt_createby " +
                                "FROM db_purchasing.dbo.t_detailworkorder d " +
                                "JOIN db_purchasing.dbo.t_workorder w ON d.dt_wor_no = w.wor_no " +
                                "JOIN db_purchasing.dbo.tlkp_user u1 ON d.dt_createby = u1.user_npk " +
                                "WHERE d.dt_wor_no = @wor_no " +
                                "UNION ALL " +
                                "SELECT " +
                                    "0 AS dt_level, " +
                                    "w.wor_createdate AS dt_createdate, " +
                                    "NULL AS dt_createby " +
                                "FROM db_purchasing.dbo.t_workorder w  " +
                                "WHERE w.wor_no = @wor_no " +
                                "ORDER BY dt_createdate DESC"

        Dim timelineHtml As New StringBuilder()
        Dim isFirst As Boolean = True ' Untuk menandai item terbaru

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@wor_no", worNo)
                Try
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            While reader.Read()
                                Dim level As Integer = reader("dt_level").ToString()
                                Dim dateStr As String = Convert.ToDateTime(reader("dt_createdate")).ToString("d MMM yyyy HH:mm")
                                Dim actor As String = reader("dt_createby").ToString()

                                Dim statusText As String = ""
                                Dim iconClass As String = "fas fa-circle" ' Default icon (bulat)
                                Dim latestClass As String = ""

                                Select Case level
                                    Case 1
                                        statusText = "Approved"
                                    Case 2
                                        statusText = "Responded to the WO"
                                    Case 3
                                        statusText = "Closed the WO"
                                    Case 4
                                        statusText = "Approved the repair"
                                    Case 0
                                        statusText = "Work Order created"
                                    Case -1
                                        statusText = "Canceled"
                                    Case -2
                                        statusText = "Canceled the repair"
                                End Select

                                ' Tandai item terbaru dengan class "latest"
                                If isFirst Then
                                    iconClass = "fas fa-check" ' Gunakan checklist untuk item terbaru
                                    latestClass = " latest"
                                    isFirst = False ' Setelah satu kali, nonaktifkan flag
                                End If

                                ' Tambahkan ke dalam HTML timeline
                                timelineHtml.Append("<li class='timeline-item" & latestClass & "'>")
                                timelineHtml.Append("<span class='timeline-icon" & latestClass & "'><i class='" & iconClass & "'></i></span>")
                                timelineHtml.Append("<span class='timeline-date" & latestClass & "'>" & dateStr & "</span> ")
                                timelineHtml.Append("<span class='timeline-text" & latestClass & "'>" & actor & " " & statusText & "</span>")
                                timelineHtml.Append("</li>")
                            End While
                        Else
                            timelineHtml.Append("<li class='timeline-item'><span class='timeline-text'>No timeline available.</span></li>")
                        End If
                    End Using
                Catch ex As Exception
                    Response.Write("<script>alert('Error: " & ex.Message & "');</script>")
                Finally
                    conn.Close()
                End Try
            End Using
        End Using

        ' Set the timeline content to a literal control in the ASPX page
        litTimeline.Text = timelineHtml.ToString()
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
                Dim queryCheck As String = "SELECT COUNT(*) FROM t_detailworkorder WHERE dt_wor_no = @wor_no AND dt_createby = @npk AND dt_level = 1"
                Using cmdCheck As New SqlCommand(queryCheck, conn, transaction)
                    cmdCheck.Parameters.AddWithValue("@wor_no", worNo)
                    cmdCheck.Parameters.AddWithValue("@npk", npk)
                    Dim count As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

                    ' Jika belum ada approval dari user ini, lanjut insert
                    If count = 0 Then
                        Dim queryInsert As String = "INSERT INTO t_detailworkorder (dt_wor_no, dt_createby, dt_createdate, dt_level) VALUES(@wor_no, @npk, @approveDate, 1)"
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
                Dim queryInsert As String = "INSERT INTO t_detailworkorder (dt_wor_no, dt_alasanreject, dt_createby, dt_createdate, dt_level) VALUES(@wor_no, @reason, @npk, GETDATE(), -1)"
                Using cmdInsert As New SqlCommand(queryInsert, conn, transaction)
                    cmdInsert.Parameters.AddWithValue("@wor_no", worNo)
                    cmdInsert.Parameters.AddWithValue("@reason", rejectReason)
                    cmdInsert.Parameters.AddWithValue("@npk", npk)
                    cmdInsert.ExecuteNonQuery()
                End Using

                ' Update Status Work Order
                Dim queryUpdate As String = "UPDATE t_workorder SET wor_status = 0 WHERE wor_no = @wor_no"
                Using cmdUpdate As New SqlCommand(queryUpdate, conn, transaction)
                    cmdUpdate.Parameters.AddWithValue("@wor_no", worNo)
                    cmdUpdate.ExecuteNonQuery()
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

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim worNo As String = Request.QueryString("wor_no") ' Ambil Work Order No dari URL
        Dim npk As String = Session("npk") ' Ambil NPK dari session login
        Dim approveDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim txtanalisa As TextBox = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("txtanalisa"), TextBox)
        Dim txtperbaikan As TextBox = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("txtperbaikan"), TextBox)

        ' Validasi: Pastikan analisa dan perbaikan diisi
        If String.IsNullOrEmpty(txtanalisa.Text.Trim()) OrElse String.IsNullOrEmpty(txtperbaikan.Text.Trim()) Then
            Response.Write("<script>alert('Analisa dan Perbaikan harus diisi sebelum menutup WO!');</script>")
            Exit Sub
        End If

        If String.IsNullOrEmpty(worNo) OrElse String.IsNullOrEmpty(npk) Then
            Response.Write("<script>alert('Data tidak valid!');</script>")
            Exit Sub
        End If

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()

            Try
                ' Cek apakah sudah pernah approve sebelumnya
                Dim queryCheck As String = "SELECT COUNT(*) FROM t_detailworkorder WHERE dt_wor_no = @wor_no AND dt_createby = @npk AND dt_level = 3"
                Using cmdCheck As New SqlCommand(queryCheck, conn, transaction)
                    cmdCheck.Parameters.AddWithValue("@wor_no", worNo)
                    cmdCheck.Parameters.AddWithValue("@npk", npk)
                    Dim count As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

                    ' Jika belum ada approval dari user ini, lanjut insert
                    If count = 0 Then
                        Dim queryInsert As String = "INSERT INTO t_detailworkorder (dt_wor_no, dt_createby, dt_createdate, dt_level) VALUES(@wor_no, @npk, @approveDate, 3)"
                        Using cmdInsert As New SqlCommand(queryInsert, conn, transaction)
                            cmdInsert.Parameters.AddWithValue("@wor_no", worNo)
                            cmdInsert.Parameters.AddWithValue("@npk", npk)
                            cmdInsert.Parameters.AddWithValue("@approveDate", approveDate)
                            cmdInsert.ExecuteNonQuery()
                        End Using
                    End If
                End Using

                ' Update Status Work Order
                Dim queryUpdate As String = "UPDATE t_workorder SET wor_status = 4, wor_analisa = @analisa, wor_perbaikan = @perbaikan WHERE wor_no = @wor_no"
                Using cmdUpdate As New SqlCommand(queryUpdate, conn, transaction)
                    cmdUpdate.Parameters.AddWithValue("@wor_no", worNo)
                    cmdUpdate.Parameters.AddWithValue("@analisa", txtanalisa.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@perbaikan", txtperbaikan.Text.Trim())
                    cmdUpdate.ExecuteNonQuery()
                End Using

                transaction.Commit()
                Dim script As String = "<script>" & Environment.NewLine &
                   "Swal.fire({" & Environment.NewLine &
                   "    icon: 'success'," & Environment.NewLine &
                   "    title: 'Success!'," & Environment.NewLine &
                   "    text: 'Anda telah menutup WO ini'," & Environment.NewLine &
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

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ViewWorkOrder.aspx")
    End Sub

    Private Sub SetActiveButton(ByVal worNo As String)
        If worNo.StartsWith("BM") Then
            btnMold.CssClass = "btn btn-outline-primary mx-2 custom-dark-btn active-btn"
            btnTool.CssClass = "btn btn-outline-primary mx-2 custom-dark-btn"
        ElseIf worNo.StartsWith("BT") Then
            btnTool.CssClass = "btn btn-outline-primary mx-2 custom-dark-btn active-btn"
            btnMold.CssClass = "btn btn-outline-primary mx-2 custom-dark-btn"
        End If
    End Sub

    Private Sub SetMoldToolLabel(ByVal worNo As String)
        If worNo.StartsWith("BM") Then
            lblMoldLabel.Text = "Mold"
        ElseIf worNo.StartsWith("BT") Then
            lblMoldLabel.Text = "Tool"
        Else
            lblMoldLabel.Text = "Mold/Tool"
        End If
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