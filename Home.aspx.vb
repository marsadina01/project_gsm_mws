
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Partial Class Home
    Inherits System.Web.UI.Page
    Dim connStr As String = ConfigurationManager.ConnectionStrings("Conn").ToString()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("role") Is Nothing Then
            Response.Redirect("default.aspx")
        Else
            If Session("url_det") IsNot Nothing Then
                Response.Redirect(Session("url_det"))
            End If
        End If

        If Not IsPostBack Then
            Dim jsonData As String = "[{""label"": ""01 Jan"", ""value"": 5}, {""label"": ""02 Jan"", ""value"": 3}]"
            grafikDataJSON.Text = jsonData
        End If
        ClientScript.RegisterStartupScript(Me.GetType, "ModalScript", "$(function(){$('#modal-faktur').modal('show'); });", True)

        Select Case Session("role")
            Case ("superadmin")
                adoc.NavigateUrl = "gps_home.aspx"
                adoc1.NavigateUrl = "gps_home.aspx"
                linkdok.NavigateUrl = "T_DokInternal.aspx"
                linkdok2.NavigateUrl = "T_DokInternal.aspx"
                amonitoring.NavigateUrl = "Delivery_Harian.aspx"
                asmonitoring.NavigateUrl = "Delivery_Harian.aspx"
                Exit Select
            Case ("admin")
                adoc.NavigateUrl = "gps_home.aspx"
                adoc1.NavigateUrl = "gps_home.aspx"
                'linkdok.NavigateUrl = "Add_DocExternal.aspx"
                'linkdok2.NavigateUrl = "Add_DocExternal.aspx"
                linkdok.NavigateUrl = "T_DokInternal.aspx"
                linkdok2.NavigateUrl = "T_DokInternal.aspx"
                amonitoring.NavigateUrl = "Delivery_Harian.aspx"
                asmonitoring.NavigateUrl = "Delivery_Harian.aspx"
                Exit Select
            Case "requester", "atsreq", "teknisiSup", "atstekSup", "teknisiGS", "atstekGS"
                divsetting.Visible = False
                divasset.Visible = False
                divpm.Visible = False
                divvendor.Visible = False
                divpm1.Visible = False
                divmvp_quality.Visible = False
                dvgps.Visible = False
                divprofile.Visible = False
                divapp.Visible = False
                div1.Visible = True
                Exit Select
        End Select

    End Sub

    'Private Sub BindGrafikData()
    '    Dim dt As New DataTable()

    '    Using conn As New SqlConnection(connStr)
    '        Dim cmd As New SqlCommand("SELECT TOP 10 RequestDate, TotalRequest FROM GrafikDashboard ORDER BY RequestDate DESC", conn)
    '        Dim adapter As New SqlDataAdapter(cmd)
    '        adapter.Fill(dt)
    '    End Using

    '    ' Konversi ke JSON dan simpan ke hidden field atau literal
    '    Dim serializer As New JavaScriptSerializer()
    '    Dim jsonData As String = serializer.Serialize(From row In dt.AsEnumerable()
    '                                                  Select New With {
    '                                                      .label = row.Field(Of DateTime)("RequestDate").ToString("dd MMM"),
    '                                                      .value = row.Field(Of Integer)("TotalRequest")
    '                                                  })

    '    grafikDataJSON.Text = jsonData
    'End Sub

    Private Sub BindGrafikData()
        ' Data statis untuk uji coba chart
        Dim jsonData As String = "[" &
        "{""label"": ""01 Jan"", ""value"": 5}," &
        "{""label"": ""02 Jan"", ""value"": 8}," &
        "{""label"": ""03 Jan"", ""value"": 3}," &
        "{""label"": ""04 Jan"", ""value"": 10}," &
        "{""label"": ""05 Jan"", ""value"": 6}," &
        "{""label"": ""06 Jan"", ""value"": 4}," &
        "{""label"": ""07 Jan"", ""value"": 9}" &
        "]"

        ' Bungkus dalam tag <script> agar bisa dibaca oleh JavaScript
        grafikDataJSON.Text = "<script>var grafikData = " & jsonData & ";</script>"
    End Sub


    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Session.Remove("url_det")
    End Sub
End Class
