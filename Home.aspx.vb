
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
            Dim chartDataJson As String = GetChartData(Now.Year.ToString())

            Dim tahun As Integer = Date.Now.Year
            Dim bulan As Integer = Date.Now.Month

            For year As Integer = tahun To 2020 Step -1
                ddlTahunChart1.Items.Add(New ListItem(year.ToString(), year.ToString()))
                ddlTahunChart2.Items.Add(New ListItem(year.ToString(), year.ToString()))
            Next

            ddlTahunChart1.SelectedValue = tahun.ToString()
            ddlTahunChart2.SelectedValue = tahun.ToString()

            LoadDashboardCards(tahun, bulan)
            LoadChartData(tahun)
            Literal1.Text = GetChartData(Convert.ToInt32(ddlTahunChart2.SelectedValue))
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

    Public Shared Function GetChartData(tahun As Integer) As String
        Dim connStr As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())

        Dim moldData(11) As Integer
        Dim toolingData(11) As Integer

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand("sp_GetMoldToolCountPerMonth", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@tahun", tahun)
                conn.Open()

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        ' period format: yyyy-MM, kita ambil bulan saja
                        Dim period As String = reader("period").ToString() ' misal "2025-03"
                        Dim bulan As Integer = Convert.ToInt32(period.Substring(5, 2)) - 1 ' 0-based index bulan

                        Dim jenis As String = reader("mt_name").ToString()
                        Dim total As Integer = Convert.ToInt32(reader("total"))

                        If jenis = "Mold" Then
                            moldData(bulan) = total
                        ElseIf jenis = "Tooling" Then
                            toolingData(bulan) = total
                        End If
                    End While
                End Using
            End Using
        End Using

        Dim months = New String() {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}

        Dim datasets = New List(Of Object) From {
        New With {.label = "Mold", .backgroundColor = "rgba(128, 0, 128, 0.6)", .data = moldData},
        New With {.label = "Tooling", .backgroundColor = "rgba(0, 128, 0, 0.6)", .data = toolingData}
    }

        Dim result = New With {
        .labels = months,
        .datasets = datasets
    }

        Dim js As New JavaScriptSerializer()
        Return js.Serialize(result)
    End Function

    Protected Sub ddlTahunChart2_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedYear As Integer = Convert.ToInt32(ddlTahunChart2.SelectedValue)
        Literal1.Text = GetChartData(selectedYear)
    End Sub

    Protected Sub ddlTahunChart1_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedYear As Integer = Convert.ToInt32(ddlTahunChart1.SelectedValue)
        LoadChartData(selectedYear)
    End Sub

    Private Sub LoadChartData(tahun As Integer)
        Dim progress(11) As Integer
        Dim done(11) As Integer

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand("sp_getchart_workorder", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@tahun", tahun)

                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim monthIndex As Integer = Convert.ToInt32(reader("work_month")) - 1
                        Dim statusGroup As String = reader("status_group").ToString()
                        Dim total As Integer = Convert.ToInt32(reader("total"))

                        If statusGroup = "progress" Then
                            progress(monthIndex) = total
                        ElseIf statusGroup = "done" Then
                            done(monthIndex) = total
                        End If
                    End While
                End Using
            End Using
        End Using

        ' Format dan kirim JSON ke literal
        Dim months = New String() {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
        Dim data = New With {
            .labels = months,
            .progress = progress,
            .done = done
        }

        Dim serializer As New JavaScriptSerializer()
        Dim jsonData As String = serializer.Serialize(data)
        grafikDataJSON.Text = jsonData
    End Sub

    Private Sub LoadDashboardCards(tahun As Integer, bulan As Integer)
        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand("sp_get_card_dashboard", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@tahun", tahun)
                cmd.Parameters.AddWithValue("@bulan", bulan)

                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    ' Reset nilai awal
                    lblWaitingApproval.Text = "0"
                    lblNeedResponse.Text = "0"
                    lblOnProgress.Text = "0"
                    lblDone.Text = "0"
                    lblRejected.Text = "0"

                    While reader.Read()
                        Dim groupStatus As String = reader("status_group").ToString()
                        Dim total As Integer = Convert.ToInt32(reader("total"))

                        Select Case groupStatus
                            Case "Waiting Approval"
                                lblWaitingApproval.Text = total.ToString()
                            Case "Need Response"
                                lblNeedResponse.Text = total.ToString()
                            Case "On Progress"
                                lblOnProgress.Text = total.ToString()
                            Case "Done"
                                lblDone.Text = total.ToString()
                            Case "Rejected"
                                lblRejected.Text = total.ToString()
                        End Select
                    End While
                End Using
            End Using
        End Using
    End Sub


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
