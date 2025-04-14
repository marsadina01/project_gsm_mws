Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Cryptography

Partial Class DetailWorkOrder
    Inherits System.Web.UI.Page

    Dim connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())

    Private hasRejectReason As Boolean = False

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
            LoadRejectReason()


            ' Cek status Work Order dari database
            Dim worStatus As Integer = GetWorkOrderStatus(worNo)
            SetStatusBadge(worStatus)


            ' Tampilkan tombol Edit Lampiran hanya jika status 6 atau 7
            btnEditLampiran.Visible = (worStatus = 6 Or worStatus = 7)

            ' Default: semua tetap disable
            EnableFormControls(False)


            ' Jika statusnya 6 atau 7, atau jika ada dt_level -2 atau -3, aktifkan input
            If worStatus = 6 Or worStatus = 7 Then
                EnableFormControls(True)
                btnUpdate.Visible = True
            Else
                btnUpdate.Visible = False
            End If

            ' Tambahan logika untuk menyembunyikan btnUpdate kalau atsreq dan ada reject
            If Session("role") = "atsreq" AndAlso hasRejectReason Then
                btnUpdate.Visible = False
            End If


            If worStatus = 1 Then
                If Session("role") = "atsreq" Then
                    detail1.Visible = True
                    detail2.Visible = False
                    fclose.Visible = False
                    btnApprove.Visible = True
                    btnReject.Visible = True
                    btnBack.Visible = False
                    btnClose.Visible = False
                    btnUpdate.Visible = False
                ElseIf Session("role") = "requester" Then
                    detail1.Visible = True
                    detail2.Visible = False
                    fclose.Visible = False
                    btnApprove.Visible = False
                    btnReject.Visible = False
                    btnBack.Visible = False
                    btnClose.Visible = False
                    btnUpdate.Visible = True ' requester bisa update/upload
                Else
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = False
                    btnApprove.Visible = False
                    btnReject.Visible = False
                    btnBack.Visible = True
                    btnClose.Visible = False
                    btnUpdate.Visible = False
                End If

            ElseIf worStatus = 3 Then
                If Session("role") = "teknisiSup" Or Session("role") = "teknisiGS" Then
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = True
                    btnApprove.Visible = False
                    btnReject.Visible = False
                    btnBack.Visible = True
                    btnClose.Visible = True
                    btnUpdate.Visible = False
                    btnLampiran.Visible = True
                Else
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = False
                    btnApprove.Visible = False
                    btnReject.Visible = False
                    btnBack.Visible = True
                    btnClose.Visible = False
                    btnUpdate.Visible = False
                    btnLampiran.Visible = False
                End If

            ElseIf worStatus = 4 Then
                If Session("role") = "atstekSup" Or Session("role") = "atstekGS" Then
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = True
                    btnApprove.Visible = True
                    btnReject.Visible = True
                    btnBack.Visible = False
                    btnClose.Visible = False
                    btnUpdate.Visible = False
                Else
                    detail1.Visible = False
                    detail2.Visible = True
                    fclose.Visible = True
                    btnApprove.Visible = False
                    btnReject.Visible = False
                    btnBack.Visible = True
                    btnClose.Visible = False
                    btnUpdate.Visible = False
                End If

            Else
                detail1.Visible = False
                detail2.Visible = True
                fclose.Visible = True
                btnApprove.Visible = False
                btnReject.Visible = False
                btnBack.Visible = True
                btnClose.Visible = False
            End If
        Else
            LoadMachineDropdown()
            LoadMoldToolDropdown()
            LoadUploadedFiles()
            LoadTimeline()

        End If
    End Sub
    Private Sub EnableFormControls(enable As Boolean)
        ' Default disable semua kontrol
        txtrequestor.Enabled = False
        txtkerusakan.Enabled = False
        txtketerangan.Enabled = False
        chkRepairBy.Enabled = False
        txtStok.Enabled = False
        txtTotalOrder.Enabled = False
        txtTglProduksi.Enabled = False
        txtanalisa.Enabled = False
        txtperbaikan.Enabled = False

        ' Mengambil role pengguna dari session
        Dim role As String = Session("role")
        Dim worStatus As Integer = GetWorkOrderStatus(Request.QueryString("wor_no"))

        ' Mengaktifkan kontrol form sesuai dengan role dan status
        If role = "requester" Then
            ' Jika role adalah requester
            If worStatus = 7 Or worStatus = 1 Then
                ' Jika status 7, hanya izinkan pengeditan untuk requester
                txtkerusakan.Enabled = True
                txtketerangan.Enabled = True
                chkRepairBy.Enabled = True
                txtStok.Enabled = True
                txtTotalOrder.Enabled = True
                txtTglProduksi.Enabled = True
            End If
        ElseIf role = "teknisiSup" Or role = "teknisiGS" Then
            ' Jika role adalah teknisi
            If worStatus = 3 Or worStatus = 6 Then
                ' Jika status 6, hanya izinkan pengeditan untuk teknisi
                txtanalisa.Enabled = True
                txtperbaikan.Enabled = True
            End If
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

    ' Load Alasan Reject dari database
    Private Sub LoadRejectReason()
        Dim worNo As String = Request.QueryString("wor_no")
        hasRejectReason = False ' Default

        ' Pastikan worNo valid dan data ada di database
        If String.IsNullOrEmpty(worNo) Then
            Response.Write("<script>alert('Work Order Number tidak ditemukan!');</script>")
            Exit Sub
        End If

        ' Query untuk mengambil alasan reject berdasarkan worNo dan level -2 atau -3
        Dim query As String = "SELECT dt_alasanreject FROM t_detailworkorder WHERE dt_wor_no = @wor_no AND (dt_level = -2 OR dt_level = -3)"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@wor_no", worNo)
                Try
                    conn.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    If reader.HasRows Then
                        hasRejectReason = True ' ← Di sini ditandai kalau ada reject reason
                        While reader.Read()
                            Dim level As Integer = reader("dt_level")
                            Dim reason As String = If(IsDBNull(reader("dt_alasanreject")), "", reader("dt_alasanreject").ToString())

                            If level = -2 OrElse level = -3 Then
                                TetxtRejectReason.Text = reason
                            End If
                        End While
                    Else
                        TetxtRejectReason.Text = ""
                    End If
                Catch ex As Exception
                    'Response.Write("<script>alert('Error: " & ex.Message & "');</script>")
                Finally
                    conn.Close()
                End Try
            End Using
        End Using

        ' Kalau gak ada reject reason, sembunyikan textbox dan label
        If Not hasRejectReason Then
            TetxtRejectReason.Visible = False
            LabelRejectReason.Visible = False
        End If
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
                                        ' Cek apakah WO status-nya sudah 5
                                        Dim finalStatus As Integer = GetWorkOrderStatus(worNo)
                                        If finalStatus = 5 Then
                                            statusText = "Approved the repair"
                                        Else
                                            statusText = ""
                                        End If
                                    Case 0
                                        statusText = "Work Order created"
                                    Case -1
                                        statusText = "Cancelled"
                                    Case -2
                                        statusText = "Rejected"
                                    Case -3
                                        statusText = "Rejected the repair"
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
        Dim worNo As String = Request.QueryString("wor_no")
        Dim npk As String = Session("npk")
        Dim role As String = Session("role")
        Dim approveDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        If String.IsNullOrEmpty(worNo) OrElse String.IsNullOrEmpty(npk) Then
            Response.Write("<script>alert('Data tidak valid!');</script>")
            Exit Sub
        End If

        Dim currentStatus As Integer = GetWorkOrderStatus(worNo)
        Dim newStatus As Integer = 2 ' Default approve awal
        Dim levelLog As Integer = 1
        Dim messageTitle As String = "Success!"
        Dim messageText As String = "Anda telah menyetujui request ini"
        Dim messageIcon As String = "success"

        ' Penentuan status & level jika role atstek dan status sudah 4
        If currentStatus = 4 AndAlso (role = "atstekSup" Or role = "atstekGS") Then
            newStatus = 5
            levelLog = 4
            messageText = "Anda telah menyetujui hasil perbaikan dan Work Order telah selesai"
        ElseIf currentStatus = 1 Then
            ' Pastikan hanya role yang diizinkan bisa approve status awal
            If Not (role = "atsreq") Then
                Response.Write("<script>alert('Anda tidak memiliki akses untuk menyetujui permintaan ini.');</script>")
                Exit Sub
            End If
        End If

        ' Proteksi agar tidak turunkan status (misalnya status 5 jadi 2)
        If currentStatus >= newStatus Then
            Exit Sub
        End If

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()

            Try
                ' Cek apakah user ini sudah pernah approve dengan level ini
                Dim queryCheck As String = "SELECT COUNT(*) FROM t_detailworkorder WHERE dt_wor_no = @wor_no AND dt_createby = @npk AND dt_level = @levelLog"
                Using cmdCheck As New SqlCommand(queryCheck, conn, transaction)
                    cmdCheck.Parameters.AddWithValue("@wor_no", worNo)
                    cmdCheck.Parameters.AddWithValue("@npk", npk)
                    cmdCheck.Parameters.AddWithValue("@levelLog", levelLog)
                    Dim count As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

                    If count = 0 Then
                        ' Insert log approval
                        Dim queryInsert As String = "INSERT INTO t_detailworkorder (dt_wor_no, dt_createby, dt_createdate, dt_level) VALUES(@wor_no, @npk, @approveDate, @levelLog)"
                        Using cmdInsert As New SqlCommand(queryInsert, conn, transaction)
                            cmdInsert.Parameters.AddWithValue("@wor_no", worNo)
                            cmdInsert.Parameters.AddWithValue("@npk", npk)
                            cmdInsert.Parameters.AddWithValue("@approveDate", approveDate)
                            cmdInsert.Parameters.AddWithValue("@levelLog", levelLog)
                            cmdInsert.ExecuteNonQuery()
                        End Using
                    End If
                End Using

                ' Update status Work Order
                Dim queryUpdate As String

                If newStatus = 5 Then
                    queryUpdate = "UPDATE t_workorder SET wor_status = @newStatus, wor_finisheddate = @finishDate WHERE wor_no = @wor_no"
                Else
                    queryUpdate = "UPDATE t_workorder SET wor_status = @newStatus WHERE wor_no = @wor_no"
                End If

                Using cmdUpdate As New SqlCommand(queryUpdate, conn, transaction)
                    cmdUpdate.Parameters.AddWithValue("@wor_no", worNo)
                    cmdUpdate.Parameters.AddWithValue("@newStatus", newStatus)

                    If newStatus = 5 Then
                        cmdUpdate.Parameters.AddWithValue("@finishDate", DateTime.Now)
                    End If

                    cmdUpdate.ExecuteNonQuery()
                End Using


                transaction.Commit()

                ' SweetAlert success
                Dim script As String = "<script>" & Environment.NewLine &
                "Swal.fire({" & Environment.NewLine &
                "    icon: '" & messageIcon & "'," & Environment.NewLine &
                "    title: '" & messageTitle & "'," & Environment.NewLine &
                "    text: '" & messageText & "'," & Environment.NewLine &
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
    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReject.Click
        Dim worNo As String = Request.QueryString("wor_no")
        Dim npk As String = Session("npk")
        Dim role As String = Session("role")
        Dim rejectDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim rejectReason As String = hdnRejectReason.Value
        If String.IsNullOrEmpty(rejectReason) Then
            Response.Write("<script>alert('Alasan Reject harus diisi!');</script>")
            Exit Sub
        End If

        If String.IsNullOrEmpty(worNo) OrElse String.IsNullOrEmpty(npk) Then
            Response.Write("<script>alert('Data tidak valid!');</script>")
            Exit Sub
        End If

        Dim currentStatus As Integer = GetWorkOrderStatus(worNo)
        Dim newStatus As Integer = 0
        Dim levelLog As Integer = 0
        Dim messageTitle As String = "Rejected!"
        Dim messageText As String = "Work Order telah ditolak"
        Dim messageIcon As String = "error"

        ' Menentukan status dan level berdasarkan role dan status saat ini
        If currentStatus = 1 AndAlso role = "atsreq" Then
            newStatus = 7
            levelLog = -2
        ElseIf currentStatus = 4 AndAlso (role = "atstekSup" Or role = "atstekGS") Then
            newStatus = 6
            levelLog = -3
        Else
            Exit Sub
        End If

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim transaction As SqlTransaction = conn.BeginTransaction()

            Try
                ' Cek apakah user ini sudah pernah reject dengan level ini
                Dim queryCheck As String = "SELECT COUNT(*) FROM t_detailworkorder WHERE dt_wor_no = @wor_no AND dt_createby = @npk AND dt_level = @levelLog"
                Using cmdCheck As New SqlCommand(queryCheck, conn, transaction)
                    cmdCheck.Parameters.AddWithValue("@wor_no", worNo)
                    cmdCheck.Parameters.AddWithValue("@npk", npk)
                    cmdCheck.Parameters.AddWithValue("@levelLog", levelLog)
                    Dim count As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

                    If count = 0 Then
                        ' Insert log rejection
                        Dim queryInsert As String = "INSERT INTO t_detailworkorder (dt_wor_no, dt_alasanreject, dt_createby, dt_createdate, dt_level) VALUES(@wor_no, @reason, @npk, @rejectDate, @levelLog)"
                        Using cmdInsert As New SqlCommand(queryInsert, conn, transaction)
                            cmdInsert.Parameters.AddWithValue("@wor_no", worNo)
                            cmdInsert.Parameters.AddWithValue("@reason", rejectReason)
                            cmdInsert.Parameters.AddWithValue("@npk", npk)
                            cmdInsert.Parameters.AddWithValue("@rejectDate", rejectDate)
                            cmdInsert.Parameters.AddWithValue("@levelLog", levelLog)
                            cmdInsert.ExecuteNonQuery()
                        End Using
                    End If
                End Using

                ' Update status Work Order
                Dim queryUpdate As String = "UPDATE t_workorder SET wor_status = @newStatus WHERE wor_no = @wor_no"
                Using cmdUpdate As New SqlCommand(queryUpdate, conn, transaction)
                    cmdUpdate.Parameters.AddWithValue("@wor_no", worNo)
                    cmdUpdate.Parameters.AddWithValue("@newStatus", newStatus)
                    cmdUpdate.ExecuteNonQuery()
                End Using

                transaction.Commit()

                ' SweetAlert success
                Dim script As String = "<script>" &
               "Swal.fire({" &
               "    icon: 'error'," &
               "    title: 'Rejected!'," &
               "    text: 'Work Order telah ditolak'," &
               "    confirmButtonColor: '#d33'," &
               "    allowOutsideClick: false" &
               "}).then((result) => {" &
               "    if (result.isConfirmed) {" &
               "        window.location='ViewWorkOrder.aspx';" &
               "    }" &
               "});" &
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
        Dim worLmapiranwo As String = ""

        ' Jika ada file yang dipilih
        If fuLampiran.HasFile Then
            Dim folderPath As String = Server.MapPath("~/Uploads/")

            ' Buat folder jika belum ada
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            ' Ambil ekstensi file asli (huruf kecil untuk validasi)
            Dim fileExtension As String = Path.GetExtension(fuLampiran.FileName).ToLower()

            ' Daftar ekstensi yang diperbolehkan
            Dim allowedExtensions As String() = {".jpg", ".jpeg", ".png", ".pdf"}

            ' Cek apakah file yang diupload masuk dalam daftar yang diperbolehkan
            If allowedExtensions.Contains(fileExtension) Then
                ' Buat format nama file: MWS_YYYYMMDD_no_wor.ext
                Dim numWor As String = worNo.Replace("/", "_")
                Dim fileName As String = "MWS_" & DateTime.Now.ToString("yyyyMMdd") & "_" & numWor & fileExtension
                Dim filePath As String = Path.Combine(folderPath, fileName)
                Try
                    fuLampiran.SaveAs(filePath)
                Catch ex As Exception
                    Response.Write("<script>alert('Error saat menyimpan file: " & ex.Message & "');</script>")
                End Try

                ' Simpan file ke folder
                fuLampiran.SaveAs(filePath)

                ' Simpan nama file ke variabel untuk database
                worLmapiranwo = fileName
            Else
                ' Jika jenis file tidak diperbolehkan, tampilkan alert dan hentikan proses
                Response.Write("<script>alert('Format file tidak diperbolehkan! Hanya JPG, PNG, dan PDF yang bisa diupload.');</script>")
                Exit Sub
            End If

        End If

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
                Dim queryUpdate As String = "UPDATE t_workorder SET wor_status = 4, wor_analisa = @analisa, wor_perbaikan = @perbaikan, wor_lampiranwo = @lampiran WHERE wor_no = @wor_no"
                Using cmdUpdate As New SqlCommand(queryUpdate, conn, transaction)
                    cmdUpdate.Parameters.AddWithValue("@wor_no", worNo)
                    cmdUpdate.Parameters.AddWithValue("@analisa", txtanalisa.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@perbaikan", txtperbaikan.Text.Trim())
                    cmdUpdate.Parameters.AddWithValue("@lampiran", worLmapiranwo)
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

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ViewWorkOrder.aspx")
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        Dim worNo As String = Request.QueryString("wor_no")
        Dim npk As String = Session("npk")
        Dim role As String = Session("role")
        Dim updateDate As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "debug", "alert('wor_no: " & worNo & "\nrole: " & role & "\nnpk: " & npk & "');", True)

        'lblDebug.Text = "Kerusakan: " & txtkerusakan.Text & " | Note: " & txtketerangan.Text & " | Analisa: " & txtanalisa.Text & " | Perbaikan: " & txtperbaikan.Text
        Dim gejalaKerusakan As String = txtkerusakan.Text
        Dim addNote As String = txtketerangan.Text
        Dim repairBy As String = If(chkRepairBy.Checked, "GS", "Supplier")
        Dim analisa As String = txtanalisa.Text
        Dim perbaikan As String = txtperbaikan.Text
        Dim jumlahStok As Integer = Convert.ToInt32(txtStok.Text)
        Dim totalOrder As Integer = Convert.ToInt32(txtTotalOrder.Text)
        Dim tglProduksiDibutuhkan As String = txtTglProduksi.Text

        Dim currentStatus As Integer = GetWorkOrderStatus(worNo)
        Dim newStatus As Integer = 0
        Dim levelLog As Integer = 0

        ' Menentukan status dan level berdasarkan role dan status saat ini
        If (currentStatus = 1 Or currentStatus = 7) AndAlso role = "requester" Then
            newStatus = 1
            levelLog = 0
        ElseIf currentStatus = 6 AndAlso (role = "teknisiSup" Or role = "teknisiGS") Then
            newStatus = 4
            levelLog = 3
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Tidak memenuhi kondisi status dan role');", True)
            Exit Sub
        End If

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim tran As SqlTransaction = conn.BeginTransaction()

            Try
                ' Query untuk update data work order
                Dim updateQuery As String = "UPDATE t_workorder SET wor_damage = @kerusakan, wor_addnote = @note, wor_repairby = @repairBy, " &
                                        "wor_analisa = @analisa, wor_perbaikan = @perbaikan, wor_status = @status, wor_stok = @stok, " &
                                        "wor_total_order = @order, wor_tglproduksi = @tglProduksi WHERE wor_no = @worNo"
                Using cmdUpdate As New SqlCommand(updateQuery, conn, tran)
                    cmdUpdate.Parameters.AddWithValue("@kerusakan", gejalaKerusakan)
                    cmdUpdate.Parameters.AddWithValue("@note", addNote)
                    cmdUpdate.Parameters.AddWithValue("@repairBy", repairBy)
                    cmdUpdate.Parameters.AddWithValue("@analisa", analisa)
                    cmdUpdate.Parameters.AddWithValue("@perbaikan", perbaikan)
                    cmdUpdate.Parameters.AddWithValue("@status", newStatus)
                    cmdUpdate.Parameters.AddWithValue("@stok", jumlahStok)
                    cmdUpdate.Parameters.AddWithValue("@order", totalOrder)
                    cmdUpdate.Parameters.AddWithValue("@tglProduksi", tglProduksiDibutuhkan)
                    cmdUpdate.Parameters.AddWithValue("@worNo", worNo)
                    cmdUpdate.ExecuteNonQuery()
                End Using

                ' Insert ke t_detailworkorder untuk logging perubahan
                Dim insertQuery As String = "INSERT INTO t_detailworkorder (dt_wor_no, dt_createby, dt_createdate, dt_level) " &
                                        "VALUES (@worNo, @npk, @updateDate, @levelLog)"
                Using cmdInsert As New SqlCommand(insertQuery, conn, tran)
                    cmdInsert.Parameters.AddWithValue("@worNo", worNo)
                    cmdInsert.Parameters.AddWithValue("@npk", npk)
                    cmdInsert.Parameters.AddWithValue("@updateDate", updateDate)
                    cmdInsert.Parameters.AddWithValue("@levelLog", levelLog)
                    cmdInsert.ExecuteNonQuery()
                End Using

                tran.Commit()
                Dim script As String = "Swal.fire({" & Environment.NewLine &
               "    icon: 'success'," & Environment.NewLine &
               "    title: 'Success!'," & Environment.NewLine &
               "    text: 'Data request berhasil diupdate'," & Environment.NewLine &
               "    confirmButtonColor: '#28a745'," & Environment.NewLine &
               "    allowOutsideClick: false" & Environment.NewLine &
               "}).then((result) => {" & Environment.NewLine &
               "    if (result.isConfirmed) {" & Environment.NewLine &
               "        window.location='ViewWorkOrder.aspx';" & Environment.NewLine &
               "    }" & Environment.NewLine &
               "});"

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "updateSuccess", script, True)

            Catch ex As Exception
                tran.Rollback()
                Dim pesanError As String = ex.Message.Replace("'", "").Replace(Environment.NewLine, " ")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "Swal.fire('Error', 'Kesalahan: " & pesanError & "', 'error');", True)
                'lblDebug.Text = "ERROR: " & ex.Message
            End Try
        End Using
    End Sub

    Protected Sub btnUploadLampiranBaru_Click(sender As Object, e As EventArgs)
        If fuLampiranBaru.HasFile Then
            Dim allowedExtensions As String() = {".pdf", ".jpg", ".png"}
            Dim extension As String = Path.GetExtension(fuLampiranBaru.FileName).ToLower()

            If allowedExtensions.Contains(extension) Then
                Dim fileName As String = Path.GetFileName(fuLampiranBaru.FileName)
                Dim savePath As String = Server.MapPath("~/Uploads/") & fileName
                fuLampiranBaru.SaveAs(savePath)

                ' Update nama file di database
                Dim worNo As String = Request.QueryString("wor_no")
                Dim query As String = "UPDATE t_workorder SET wor_lampiran = @fileName WHERE wor_no = @wor_no"

                Using conn As New SqlConnection(connStr)
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@fileName", fileName)
                        cmd.Parameters.AddWithValue("@wor_no", worNo)

                        Try
                            conn.Open()
                            cmd.ExecuteNonQuery()
                            lblMessageBaru.ForeColor = System.Drawing.Color.Green
                            lblMessageBaru.Text = "Lampiran berhasil diperbarui."

                            ' Update iframe tanpa reload halaman
                            Dim script As String = "document.getElementById('lampiranFrame').src = '" & ResolveUrl("~/Uploads/" & fileName) & "';"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "refreshIframe", script, True)

                        Catch ex As Exception
                            lblMessageBaru.Text = "Gagal update file: " & ex.Message
                        Finally
                            conn.Close()
                        End Try
                    End Using
                End Using
            Else
                lblMessageBaru.Text = "File tidak valid. Hanya PDF, JPG, PNG."
            End If
        Else
            lblMessageBaru.Text = "Silakan pilih file terlebih dahulu."
        End If
    End Sub

    Private Sub SetStatusBadge(worStatus As Integer)
        Dim statusText As String = GetStatusText(worStatus)
        Dim statusStyle As String = GetStatusStyle(worStatus)

        ' Gunakan style inline agar tampil berwarna sesuai fungsi kamu
        Dim badgeHtml As String = "<span style=""" & statusStyle & " padding:4px 10px; border-radius:12px;"">" & statusText & "</span>"

        litBadgeStatus.Text = badgeHtml
    End Sub

    Public Function GetStatusText(ByVal statusID As Object) As String
        If IsDBNull(statusID) Then Return ""

        Select Case Convert.ToInt32(statusID)
            Case 1 : Return "Waiting Approval"
            Case 2 : Return "Need Response"
            Case 3 : Return "On Progress"
            Case 4 : Return "Waiting Approval by Kasie Technician"
            Case 5 : Return "Done"
            Case 6 : Return "Rejected by Kasie Technician"
            Case 7 : Return "Rejected by Kasie Req"
            Case 0 : Return "Canceled"
            Case Else : Return "Unknown"
        End Select
    End Function

    Public Function GetStatusStyle(ByVal statusID As Object) As String
        If IsDBNull(statusID) Then Return "background-color:#FFFFFF; color:#000000; font-weight:bold; text-align:center;"

        Select Case Convert.ToInt32(statusID)
            Case 1 : Return "background-color:#ffebd3; color:#ffa93f; font-weight:bold; text-align:center;" ' Oranye
            Case 2 : Return "background-color:#FED7D4; color:#f94131; font-weight:bold; text-align:center;" ' Merah
            Case 3 : Return "background-color:#fffbcc; color:#ffea08; font-weight:bold; text-align:center;" ' Kuning
            Case 4 : Return "background-color:#ffebd3; color:#2aa847; font-weight:bold; text-align:center;" ' Oranye, Hijau tua
            Case 5 : Return "background-color:#d4edda; color:#2aa847; font-weight:bold; text-align:center;" ' Hijau
            Case 6 : Return "background-color:#B0C4DE; color:#000080; font-weight:bold; text-align:center;" ' Biru abu-abu
            Case 7 : Return "background-color:#D3D3D3; color:#000000; font-weight:bold; text-align:center;" ' Abu-abu
            Case 0 : Return "background-color:#D3D3D3; color:#000000; font-weight:bold; text-align:center;" ' Abu-abu
            Case Else : Return "background-color:#FFFFFF; color:#000000; font-weight:bold; text-align:center;" ' Default (Putih)
        End Select
    End Function

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