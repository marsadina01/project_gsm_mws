
Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Web.Script.Serialization

Partial Class Home
    Inherits System.Web.UI.Page
    Dim connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("role") Is Nothing Then
            Response.Redirect("default.aspx")
        Else
            If Session("url_det") IsNot Nothing Then
                Response.Redirect(Session("url_det"))
            End If
        End If

        If Not IsPostBack Then
            Dim chartDataJson As String = GetChartData()

            Dim script As String = "<script>" &
                               "var ctx2 = document.getElementById('grafikChart2').getContext('2d');" &
                               "var chartData = " & chartDataJson & ";" &
                               "var chart2 = new Chart(ctx2, {" &
                               "type: 'bar'," &
                               "data: chartData," &
                               "options: {" &
                               "responsive: true," &
                               "maintainAspectRatio: false," &
                               "plugins: { legend: { position: 'top' } }," &
                               "scales: { y: { beginAtZero: true, ticks: { stepSize: 1 } } }" &
                               "}" &
                               "});" &
                               "</script>"

            Literal1.Text = script
            Literal1.Visible = True
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

    Private Function GetChartData() As String
        Dim result As New Dictionary(Of String, Object)
        Dim labels As New List(Of String)
        Dim datasets As New List(Of Dictionary(Of String, Object))
        Dim dt As New DataTable()

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand("sp_GetMoldToolCountPerMonth", conn)
                cmd.CommandType = CommandType.StoredProcedure
                conn.Open()

                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        ' Ambil semua bulan (distinct)
        Dim bulanSet = dt.AsEnumerable().Select(Function(r) r.Field(Of String)("period")).Distinct().ToList()
        labels = bulanSet

        ' Ambil semua jenis mold_type_name (Mold/Tooling)
        Dim jenisSet = dt.AsEnumerable().Select(Function(r) r.Field(Of String)("mt_name")).Distinct()
        For Each jenis In jenisSet
            Dim dataset As New Dictionary(Of String, Object)
            dataset("label") = jenis
            dataset("backgroundColor") = If(jenis = "Mold", "rgba(128, 0, 128, 0.6)", "rgba(0, 128, 0, 0.6)")
            dataset("data") = (
        From b In bulanSet
        Let row = dt.AsEnumerable().FirstOrDefault(Function(r) r.Field(Of String)("period") = b AndAlso r.Field(Of String)("mt_name") = jenis)
        Select If(row IsNot Nothing, row.Field(Of Integer)("total"), 0)
    ).ToList()

            datasets.Add(dataset)
        Next


        result("labels") = labels
        result("datasets") = datasets

        Dim js As New JavaScriptSerializer()
        Return js.Serialize(result)
    End Function

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Session.Remove("url_det")
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
