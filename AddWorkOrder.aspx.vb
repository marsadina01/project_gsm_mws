Imports System.Data
Imports System.Security.Cryptography
Imports System.Data.SqlClient
Imports System.IO

Partial Class AddWorkOrder
    Inherits System.Web.UI.Page


    Dim connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())
    Dim selectedType As String = "" ' Untuk menyimpan pilihan Mold atau Tool

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("name") Is Nothing Then
            Response.Redirect("default.aspx")
        End If

        If Not IsPostBack Then
            txtrequestor.Text = Session("namafull").ToString()
            ViewState("namafull") = txtrequestor.Text
            lblidreq.Text = Session("npk").ToString()
            LoadMachines()

            ' Pastikan tombol yang terakhir dipilih tetap aktif setelah reload
            Dim selectedButton As String = If(ViewState("selectedButton"), "")

            If selectedButton = "Mold" Then
                btnMold.CssClass = "btn custom-dark-btn active-btn mx-2"
                btnTool.CssClass = "btn custom-dark-btn mx-2"
            ElseIf selectedButton = "Tool" Then
                btnTool.CssClass = "btn custom-dark-btn active-btn mx-2"
                btnMold.CssClass = "btn custom-dark-btn mx-2"
            Else
                ' Jika tidak ada yang diklik, gunakan default
                btnMold.CssClass = "btn custom-dark-btn mx-2"
                btnTool.CssClass = "btn custom-dark-btn mx-2"
            End If
        Else
            ' Ambil kembali dari ViewState saat postback
            If ViewState("namafull") IsNot Nothing Then
                txtrequestor.Text = ViewState("namafull").ToString()
            End If
        End If
    End Sub


    ' Fungsi untuk mengisi dropdown nama mesin
    Private Sub LoadMachines()
        ddlmachine.Items.Clear()
        ddlmachine.Items.Add(New ListItem("-- Pilih No Mesin --", ""))

        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT id, mesin_nomor FROM db_maintenance.dbo.tlkp_mesin"
            Using cmd As New SqlCommand(query, conn)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    ddlmachine.Items.Add(New ListItem(reader("mesin_nomor").ToString(), reader("id").ToString()))
                End While
                conn.Close()
            End Using
        End Using
    End Sub

    Private Sub dataMoldTool(ByVal type As String, ByVal machineID As String)
        Dim ddlMoldTool As DropDownList = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("ddlMoldTool"), DropDownList)
        If ddlMoldTool Is Nothing Then Exit Sub

        ddlMoldTool.Items.Clear()
        ddlMoldTool.Items.Add(New ListItem("-- Pilih ID Mold / Tool --", ""))

        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT mold_id, mold_name FROM db_master_data.dbo.tlkp_mnt WHERE mold_type = @Type AND mold_status = 1 AND mold_mesin = @MesinID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Type", type)
                cmd.Parameters.AddWithValue("@MesinID", machineID)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    ddlMoldTool.Items.Add(New ListItem(reader("mold_name").ToString(), reader("mold_id").ToString()))
                End While
                conn.Close()
            End Using
        End Using

        If ddlMoldTool.Items.Count > 1 Then
            ddlMoldTool.SelectedIndex = 0
        End If
    End Sub

    ' Fungsi untuk generate wor_no berdasarkan pilihan Mold atau Tool
    Private Function GenerateWorkOrderNumber(ByVal prefix As String) As String
        Dim currentYear As String = DateTime.Now.Year.ToString()
        Dim newWorNo As String = prefix & "/" & currentYear & "/"

        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT ISNULL(MAX(CAST(RIGHT(wor_no, 5) AS INT)), 0) + 1 FROM t_workorder WHERE wor_no LIKE @Prefix"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Prefix", prefix & "/" & currentYear & "%")
                conn.Open()
                Dim nextNumber As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                conn.Close()
                newWorNo &= nextNumber.ToString("D5")
            End Using
        End Using

        Return newWorNo
    End Function

    ' Saat memilih Mold
    Protected Sub btnMold_Click(ByVal sender As Object, ByVal e As EventArgs)
        Session("selectedType") = "Mold"

        btnMold.CssClass = "btn custom-dark-btn mx-2 active-btn"
        btnTool.CssClass = "btn custom-dark-btn mx-2"
        Dim machineID As String = ddlmachine.SelectedValue
        If String.IsNullOrEmpty(machineID) Then Exit Sub

        ViewState("selectedType") = "1" ' Simpan pilihan di ViewState
        ViewState("wor_no") = GenerateWorkOrderNumber("BM")
        dataMoldTool(ViewState("selectedType").ToString(), machineID)
    End Sub

    ' Saat memilih Tool
    Protected Sub btnTool_Click(ByVal sender As Object, ByVal e As EventArgs)
        Session("selectedType") = "Tooling"

        btnTool.CssClass = "btn custom-dark-btn mx-2 active-btn"
        btnMold.CssClass = "btn custom-dark-btn mx-2"
        Dim machineID As String = ddlmachine.SelectedValue
        If String.IsNullOrEmpty(machineID) Then Exit Sub

        ViewState("selectedType") = "2" ' Simpan pilihan di ViewState
        ViewState("wor_no") = GenerateWorkOrderNumber("BT")
        dataMoldTool(ViewState("selectedType").ToString(), machineID)
    End Sub


    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Cek apakah ada file yang dipilih
        If fuLampiran.HasFile Then
            ' Cek ekstensi file
            Dim allowedExtensions As String() = {".pdf", ".png", ".jpg"}
            Dim fileExtension As String = Path.GetExtension(fuLampiran.FileName).ToLower()

            If allowedExtensions.Contains(fileExtension) Then
                ' Simpan nama file ke ViewState
                ViewState("wor_lampiran") = fuLampiran.FileName
                lblMessage.ForeColor = System.Drawing.Color.Green
                lblMessage.Text = "File akan disimpan saat Work Order disimpan."
            Else
                lblMessage.ForeColor = System.Drawing.Color.Red
                lblMessage.Text = "Format file tidak diperbolehkan! Hanya PDF, PNG, atau JPG."
            End If
        Else
            lblMessage.ForeColor = System.Drawing.Color.Red
            lblMessage.Text = "Silakan pilih file untuk diupload!"
        End If
    End Sub

    ' Tombol Simpan Work Order
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs)
        If ViewState("wor_no") Is Nothing Then
            Response.Write("<script>alert('Silakan pilih Mold atau Tool terlebih dahulu!');</script>")
            Exit Sub
        End If

        Dim worNo As String = ViewState("wor_no").ToString()
        Dim worSupplier As String = Session("nameinfor").ToString()
        Dim worDamage As String = txtKerusakan.Text.Trim()
        Dim worMoldTool As String = ddlMoldTool.SelectedValue
        Dim worMachine As String = ddlmachine.SelectedValue
        Dim worRepairBy As String = If(chkKirimGS.Checked, "GS", "Supplier")
        Dim worAddNote As String = txtketerangan.Text.Trim()
        Dim worStok As Integer = If(IsNumeric(txtStok.Text.Trim()), Convert.ToInt32(txtStok.Text.Trim()), 0)
        Dim worTotalOrder As Integer = If(IsNumeric(txtTotalOrder.Text.Trim()), Convert.ToInt32(txtTotalOrder.Text.Trim()), 0)
        Dim worTglProduksi As String = txtTglProduksi.Text.Trim()
        Dim worStatus As Integer = 1
        Dim worCreateBy As String = Session("npk").ToString()
        Dim worCreateDate As DateTime = DateTime.Now
        Dim worLampiran As String = ""


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
                    'If File.Exists(filePath) Then
                    '    Response.Write("<script>alert('File berhasil disimpan di: " & filePath & "');</script>")
                    'Else
                    '    Response.Write("<script>alert('File gagal disimpan di: " & filePath & "');</script>")
                    'End If
                Catch ex As Exception
                    Response.Write("<script>alert('Error saat menyimpan file: " & ex.Message & "');</script>")
                End Try

                ' Simpan file ke folder
                fuLampiran.SaveAs(filePath)

                ' Simpan nama file ke variabel untuk database
                worLampiran = fileName
            Else
                ' Jika jenis file tidak diperbolehkan, tampilkan alert dan hentikan proses
                Response.Write("<script>alert('Format file tidak diperbolehkan! Hanya JPG, PNG, dan PDF yang bisa diupload.');</script>")
                Exit Sub
            End If

        End If

        ' Simpan ke database
        Try
            Using conn As New SqlConnection(connStr)
                Dim query As String = "INSERT INTO db_purchasing.dbo.t_workorder (wor_no, wor_supplier, wor_damage, wor_mold_tool, wor_machine, wor_repairby, wor_addnote, wor_stok, wor_total_order, wor_tglproduksi, wor_status, wor_createby, wor_createdate, wor_lampiran) " &
                      "VALUES (@WorNo, @WorSupplier, @WorDamage, @WorMoldTool, @WorMachine, @WorRepairBy, @WorAddNote, @WorStok, @WorTotalOrder, @WorTglProduksi, @WorStatus, @WorCreateBy, @WorCreateDate, @WorLampiran)"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@WorNo", worNo)
                    cmd.Parameters.AddWithValue("@WorSupplier", worSupplier)
                    cmd.Parameters.AddWithValue("@WorDamage", worDamage)
                    cmd.Parameters.AddWithValue("@WorMoldTool", worMoldTool)
                    cmd.Parameters.AddWithValue("@WorMachine", worMachine)
                    cmd.Parameters.AddWithValue("@WorRepairBy", worRepairBy)
                    cmd.Parameters.AddWithValue("@WorAddNote", worAddNote)
                    cmd.Parameters.AddWithValue("@WorStok", worStok)
                    cmd.Parameters.AddWithValue("@WorTotalOrder", worTotalOrder)
                    cmd.Parameters.AddWithValue("@WorTglProduksi", worTglProduksi)
                    cmd.Parameters.AddWithValue("@WorStatus", worStatus)
                    cmd.Parameters.AddWithValue("@WorCreateBy", worCreateBy)
                    cmd.Parameters.AddWithValue("@WorCreateDate", worCreateDate)
                    cmd.Parameters.AddWithValue("@WorLampiran", worLampiran)
                    'Response.Write("<script>alert('Nama file yang akan disimpan: " & worLampiran & "');</script>")


                    conn.Open()
                    cmd.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using

            ClientScript.RegisterStartupScript(Me.GetType(), "alert",
                "<script>Swal.fire({ " &
                "title: 'Berhasil!', " &
                "text: 'Work Order berhasil disimpan!', " &
                "icon: 'success', " &
                "confirmButtonText: 'OK' " &
                "}).then(function() { " &
                "window.location='ViewWorkOrder.aspx'; });</script>", False)

        Catch ex As Exception
            Response.Write("<script>alert('Terjadi kesalahan: " & ex.Message.Replace("'", "\'") & "');</script>")
        End Try
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