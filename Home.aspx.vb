
Imports System.Data
Imports System.Data.SqlClient

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

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Session.Remove("url_det")
    End Sub
End Class
