
Imports System.Data
Imports System
Imports System.Security.Cryptography
Imports System.Data.SqlClient

Partial Class ViewWorkOrder
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

        Dim MatSql As String = "SELECT " +
                            "w.wor_no AS wor_no, " +
                            "w.wor_supplier AS wor_supplier, " +
                            "w.wor_damage AS wor_kerusakkan, " +
                            "w.wor_mold_tool AS wor_no_mold, " +
                            "w.wor_createdate AS wor_request_date, " +
                            "d.dt_techresponsedate AS wor_response_date, " +
                            "w.wor_repairby AS wor_repairby, " +
                            "w.wor_finisheddate AS wor_finished_date, " +
                            "w.wor_status AS wor_status " +
                        "FROM db_purchasing.dbo.t_workorder w " +
                        "LEFT JOIN db_purchasing.dbo.t_detailworkorder d " +
                        "ON w.wor_no = d.dt_wor_no " +
                        "WHERE 1=1 "

        ' Tambahkan parameter jika user melakukan filter
        If ddlBreakdown IsNot Nothing AndAlso ddlBreakdown.SelectedValue <> "0" Then
            MatSql += " AND w.wor_no LIKE @Breakdown"
            MatComm.Parameters.AddWithValue("@Breakdown", "%" & ddlBreakdown.SelectedValue & "%")
        End If

        If ddlStatus IsNot Nothing AndAlso Not String.IsNullOrEmpty(ddlStatus.SelectedValue) Then
            MatSql += " AND w.wor_status = @Status"
            MatComm.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue)
        End If

        If txtReqDate IsNot Nothing AndAlso Not String.IsNullOrEmpty(txtReqDate.Text) Then
            Dim requestDate As DateTime
            If DateTime.TryParse(txtReqDate.Text, requestDate) Then
                MatSql += " AND CAST(w.wor_createdate AS DATE) = @ReqDate"
                MatComm.Parameters.AddWithValue("@ReqDate", requestDate.ToString("yyyy-MM-dd"))
            End If
        End If

        MatComm.Connection = conn
        MatComm.CommandText = MatSql
        oradap.SelectCommand = MatComm
        oradap.Fill(dtab)
        dv = dtab.DefaultView

        If dtab.Rows.Count > 0 Then
            rptWorkOrder.DataSource = dv
            Me.rptWorkOrder.DataBind()
            'gvdata.DataBind()
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

    Public Function Get_EmptyDataTable() As DataTable
        Dim dtEmpty As New DataTable()

        ' Tambahkan semua kolom yang ada di GridView
        dtEmpty.Columns.Add("wor_no", GetType(String))
        dtEmpty.Columns.Add("wor_supplier", GetType(String))
        dtEmpty.Columns.Add("wor_kerusakkan", GetType(String))
        dtEmpty.Columns.Add("wor_no_mold", GetType(String))
        dtEmpty.Columns.Add("wor_request_date", GetType(String))
        dtEmpty.Columns.Add("wor_response_date", GetType(String))
        dtEmpty.Columns.Add("wor_repairby", GetType(String))
        dtEmpty.Columns.Add("wor_finished_date", GetType(String))
        dtEmpty.Columns.Add("wor_status", GetType(String))

        ' Buat baris kosong
        Dim datatRow As DataRow = dtEmpty.NewRow()

        ' Isi kolom pertama dengan teks "No data available in table"
        datatRow("wor_no") = "No data available in table"

        ' Tambahkan baris ke DataTable
        dtEmpty.Rows.Add(datatRow)

        Return dtEmpty
    End Function

    Public Function GetStatusText(ByVal statusID As Object) As String
        If IsDBNull(statusID) Then Return ""

        Select Case Convert.ToInt32(statusID)
            Case 1 : Return "Waiting Approval"
            Case 2 : Return "Need Response"
            Case 3 : Return "On Progress"
            Case 4 : Return "Waiting Approval by Technical Superior"
            Case 5 : Return "Done"
            Case 0 : Return "Cancelled"
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
