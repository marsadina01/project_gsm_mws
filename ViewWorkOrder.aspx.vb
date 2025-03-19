
Imports System.Data
Imports System
Imports System.Security.Cryptography
Imports System.Data.SqlClient
Imports iTextSharp.text.pdf.AcroFields
Imports System.Web.Services

Partial Class ViewWorkOrder
    Inherits System.Web.UI.Page

    ' Variabel connection string sebagai Shared
    Private Shared ReadOnly connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("name") Is Nothing Then
            Response.Redirect("default.aspx")
        End If

        If Not IsPostBack Then
            data()
        End If
    End Sub

    Public Sub data(Optional ByVal isReset As Boolean = False)
        ' Ambil kontrol filter langsung dari halaman, bukan dari RepeaterItem
        Dim ddlBreakdown As DropDownList = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("ddlBreakdown"), DropDownList)
        Dim txtReqDate As TextBox = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("txtReqDate"), TextBox)
        Dim ddlStatus As DropDownList = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("ddlStatus"), DropDownList)

        Dim MatComm As New SqlCommand
        Dim dv As DataView
        Dim dtab As New DataTable
        Dim oradap As New SqlDataAdapter

        Dim conn As New SqlConnection(connStr)
        conn.Open()

        ' Perbaikan sintaks SQL (DESC harus setelah ORDER BY)
        Dim MatSql As String = "SELECT " +
                                    " w.wor_no," +
                                    " s.spl_nama As wor_supplier," +
                                    " w.wor_damage," +
                                    " m.mold_name As wor_mold_tool," +
                                    " w.wor_createdate," +
                                    " w.wor_responsedate," +
                                    " w.wor_repairby," +
                                    " w.wor_finisheddate," +
                                    " w.wor_status  " +
                                 "From db_purchasing.dbo.t_workorder w " +
                                    "INNER Join db_purchasing.dbo.tlkp_supplier s ON w.wor_supplier = s.spl_id " +
                                    "INNER Join db_master_data.dbo.tlkp_mnt m ON w.wor_mold_tool = m.mold_id " +
                                 "WHERE 1 = 1 "

        ' Tambahkan filter berdasarkan Session("namainfor") jika ada
        If Session("nameinfor") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Session("nameinfor").ToString()) Then
            MatSql += " AND w.wor_supplier = @NamaInfor"
            MatComm.Parameters.AddWithValue("@NamaInfor", Session("nameinfor").ToString())
        End If

        If Session("role") = "teknisiSup" Then
            MatSql += " AND w.wor_repairby = 'Supplier' AND w.wor_status <> 1 AND w.wor_status <> 0"
        ElseIf Session("role") = "teknisiGS" Then
            MatSql += " AND w.wor_repairby = 'GS' AND w.wor_status <> 1 AND w.wor_status <> 0"
        ElseIf Session("role") = "atstekSup" Then
            MatSql += " AND w.wor_repairby = 'Supplier' AND w.wor_status <> 1 AND w.wor_status <> 2 AND w.wor_status <> 3 AND w.wor_status <> 0"
        ElseIf Session("role") = "atstekGS" Then
            MatSql += " AND w.wor_repairby = 'GS' AND w.wor_status <> 1 AND w.wor_status <> 2 AND w.wor_status <> 3 AND w.wor_status <> 0"
        End If

        ' Tambahkan parameter jika user melakukan filter
        If ddlBreakdown IsNot Nothing AndAlso ddlBreakdown.SelectedValue <> "0" Then
            MatSql += " AND wor_no LIKE @Breakdown"
            MatComm.Parameters.AddWithValue("@Breakdown", "%" & ddlBreakdown.SelectedValue & "%")
        End If

        If ddlStatus IsNot Nothing AndAlso Not String.IsNullOrEmpty(ddlStatus.SelectedValue) Then
            MatSql += " AND wor_status = @Status"
            MatComm.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue)
        End If

        If txtReqDate IsNot Nothing AndAlso Not String.IsNullOrEmpty(txtReqDate.Text) Then
            Dim requestDate As DateTime
            If DateTime.TryParse(txtReqDate.Text, requestDate) Then
                MatSql += " AND CAST(wor_createdate AS DATE) = @ReqDate"
                MatComm.Parameters.AddWithValue("@ReqDate", requestDate.ToString("yyyy-MM-dd"))
            End If
        End If

        ' ORDER BY harus di akhir, dan DESC setelahnya
        MatSql += " ORDER BY wor_createdate DESC"

        MatComm.Connection = conn
        MatComm.CommandText = MatSql
        oradap.SelectCommand = MatComm
        oradap.Fill(dtab)
        dv = dtab.DefaultView

        If dtab.Rows.Count > 0 Then
            rptWorkOrder.DataSource = dv
            Me.rptWorkOrder.DataBind()
        Else
            rptWorkOrder.DataSource = Me.Get_EmptyDataTable()
            rptWorkOrder.DataBind()
        End If

        conn.Close()
    End Sub

    ' Fungsi pencarian dan reset
    Protected Sub OnSearchOrRefresh(ByVal sender As Object, ByVal e As EventArgs)
        Dim isReset As Boolean = (DirectCast(sender, LinkButton).ID = "btnReset")

        Dim ddlBreakdown As DropDownList = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("ddlBreakdown"), DropDownList)
        Dim txtReqDate As TextBox = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("txtReqDate"), TextBox)
        Dim ddlStatus As DropDownList = TryCast(Master.FindControl("ContentPlaceHolder1").FindControl("ddlStatus"), DropDownList)

        ' Jika reset, bersihkan input filter
        If isReset Then
            ddlBreakdown.SelectedIndex = 0
            txtReqDate.Text = String.Empty
            ddlStatus.SelectedIndex = 0
        End If

        ' Panggil fungsi `data`
        data(isReset)
    End Sub

    <WebMethod()>
    Public Shared Function OnResponse(ByVal worNo As String) As String
        ' Cek apakah parameter kosong
        If String.IsNullOrEmpty(worNo) Then
            Return "Work Order tidak valid."
        End If

        Dim con As New SqlConnection(connStr)
        Dim transaction As SqlTransaction = Nothing

        Try
            con.Open()
            transaction = con.BeginTransaction()

            ' Query 1: Update status di tabel t_workorder
            Dim query1 As String = "UPDATE db_purchasing.dbo.t_workorder SET wor_status = 3, wor_responsedate = GETDATE() WHERE wor_no = @wor_no"

            ' Query 2: Insert ke tabel t_detailworkorder
            Dim query2 As String = "INSERT INTO db_purchasing.dbo.t_detailworkorder (dt_wor_no, dt_createby, dt_createdate, dt_level) VALUES (@wor_no, @npk, GETDATE(), 2)"

            ' Eksekusi Query 1
            Using cmd1 As New SqlCommand(query1, con, transaction)
                cmd1.Parameters.AddWithValue("@wor_no", worNo)
                cmd1.ExecuteNonQuery()
            End Using

            ' Eksekusi Query 2
            Using cmd2 As New SqlCommand(query2, con, transaction)
                cmd2.Parameters.AddWithValue("@wor_no", worNo)
                cmd2.Parameters.AddWithValue("@npk", HttpContext.Current.Session("npk")) ' Ambil nilai NPK dari session
                cmd2.ExecuteNonQuery()
            End Using

            ' Commit jika semua berhasil
            transaction.Commit()
            Return "WO telah direspons."
        Catch ex As Exception
            ' Rollback jika ada error
            If transaction IsNot Nothing Then
                transaction.Rollback()
            End If
            Return "Terjadi kesalahan: " & ex.Message
        Finally
            con.Close()
        End Try
    End Function


    Public Function Get_EmptyDataTable() As DataTable
        Dim dtEmpty As New DataTable()

        ' Tambahkan semua kolom yang ada di GridView
        dtEmpty.Columns.Add("wor_no", GetType(String))
        dtEmpty.Columns.Add("wor_supplier", GetType(String))
        dtEmpty.Columns.Add("wor_damage", GetType(String))
        dtEmpty.Columns.Add("wor_mold_tool", GetType(String))
        dtEmpty.Columns.Add("wor_createdate", GetType(String))
        dtEmpty.Columns.Add("wor_responsedate", GetType(String))
        dtEmpty.Columns.Add("wor_repairby", GetType(String))
        dtEmpty.Columns.Add("wor_finisheddate", GetType(String))
        dtEmpty.Columns.Add("wor_status", GetType(String))

        ' Buat baris kosong
        Dim datatRow As DataRow = dtEmpty.NewRow()

        ' Isi kolom pertama dengan teks "No data available in table"
        datatRow("wor_no") = "No data available in table"

        ' Tambahkan baris ke DataTable
        dtEmpty.Rows.Add(datatRow)

        Return dtEmpty
    End Function

    Public Function GetActionButtons(ByVal worStatus As Object, ByVal worNo As Object) As String
        ' Cek apakah worStatus atau worNo adalah NULL atau kosong sebelum digunakan
        If worStatus Is DBNull.Value OrElse worNo Is DBNull.Value Then
            Return "" ' Jika NULL, jangan tampilkan tombol
        End If

        Dim status As String = worStatus.ToString()
        Dim workOrderNo As String = worNo.ToString()
        Dim buttons As String = ""

        ' Pastikan status adalah angka valid
        If Not String.IsNullOrEmpty(status) AndAlso IsNumeric(status) Then
            If status = "2" Then
                buttons &= "<button type='button' class='btn btn-danger btn-sm' title='Response' style='margin-right: 5px;' onclick='respondToWorkOrder(""" & workOrderNo & """)'>"
                buttons &= "<i class='fa fa-deafness'></i></button>"
            End If
        End If

        If Not String.IsNullOrEmpty(workOrderNo) AndAlso workOrderNo <> "No data available in table" Then
            buttons &= "<a href='DetailWorkOrder.aspx?wor_no=" & workOrderNo & "' class='btn btn-info btn-sm' title='Detail' style='margin-right: 5px;'>"
            buttons &= "<i class='fa fa-eye'></i></a>"
        End If

        Return buttons
    End Function

    Public Function GetStatusText(ByVal statusID As Object) As String
        If IsDBNull(statusID) Then Return ""

        Select Case Convert.ToInt32(statusID)
            Case 1 : Return "Waiting Approval"
            Case 2 : Return "Need Response"
            Case 3 : Return "On Progress"
            Case 4 : Return "Waiting Approval by Technical Superior"
            Case 5 : Return "Done"
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
            Case 0 : Return "background-color:#D3D3D3; color:#000000; font-weight:bold; text-align:center;" ' Abu-abu
            Case Else : Return "background-color:#FFFFFF; color:#000000; font-weight:bold; text-align:center;" ' Default (Putih)
        End Select
    End Function


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
