
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
            Case ("mpc")
                amonitoring.NavigateUrl = "http://warehouse.gs.astra.co.id/Delivery_NoteM.aspx"
                asmonitoring.NavigateUrl = "http://warehouse.gs.astra.co.id/Delivery_NoteM.aspx"
                divsetting.Visible = False
                divpm.Visible = False
                divvendor.Visible = False
                divpm1.Visible = False
                divmvp_quality.Visible = True
                dvgps.Visible = False
                Exit Select
            Case ("purchasing")
                linkdok.NavigateUrl = "Add_DocExternal.aspx"
                linkdok2.NavigateUrl = "T_DokInternal.aspx"
                amonitoring.NavigateUrl = "Delivery_Harian.aspx"
                asmonitoring.NavigateUrl = "Delivery_Harian.aspx"
                divsetting.Visible = False
                'divasset.Visible = False
                divpm.Visible = True
                divvendor.Visible = True
                divpm1.Visible = True
                dvgps.Visible = False
                Exit Select
            Case ("qc")
                linkdok.NavigateUrl = "Add_DocExternal.aspx"
                linkdok2.NavigateUrl = "Add_DocExternal.aspx"
                divsetting.Visible = False
                divasset.Visible = False
                divpm.Visible = True
                divvendor.Visible = False
                divpm1.Visible = False
                divmvp_quality.Visible = True
                dvgps.Visible = False
                Exit Select
            Case ("mws")
                linkdok.NavigateUrl = "Add_DocExternal.aspx"
                linkdok2.NavigateUrl = "Add_DocExternal.aspx"
                divsetting.Visible = False
                divasset.Visible = False
                divpm.Visible = False
                divvendor.Visible = False
                divpm1.Visible = False
                divmvp_quality.Visible = False
                dvgps.Visible = False
                Exit Select
            Case ("she")
                linkdok.NavigateUrl = "Add_DocExternal.aspx"
                linkdok2.NavigateUrl = "Add_DocExternal.aspx"
                divsetting.Visible = False
                divasset.Visible = False
                divpm.Visible = False
                divvendor.Visible = False
                divpm1.Visible = False
                divmvp_quality.Visible = True
                dvgps.Visible = False
                Exit Select
            Case ("vendor")
                adoc.NavigateUrl = "gps_home.aspx"
                adoc1.NavigateUrl = "gps_home.aspx"
                linkdok.NavigateUrl = "Tambah_Dokumen.aspx"
                linkdok2.NavigateUrl = "Tambah_Dokumen.aspx"
                amonitoring.NavigateUrl = "Delivery_HarianVendor.aspx"
                asmonitoring.NavigateUrl = "Delivery_HarianVendor.aspx"
                divsetting.Visible = False
                divpm.Visible = True
                divvendor.Visible = False
                divapp.Visible = False
                dvgps.Visible = False
                ClientScript.RegisterStartupScript(Me.GetType, "ModalScript", "$(function(){$('#modal-faktur').modal('show'); });", True)
                Exit Select
            Case ("fin")
                adoc.NavigateUrl = "inv_status.aspx"
                adoc1.NavigateUrl = "inv_status.aspx"
                divasset.Visible = False
                divpm1.Visible = False
                divsetting.Visible = False
                divpm.Visible = False
                divvendor.Visible = False
                divapp.Visible = False
                ClientScript.RegisterStartupScript(Me.GetType, "ModalScript", "$(function(){$('#modal-faktur').modal('show'); });", True)
                Exit Select
            Case ("vendorfin")
                adoc.NavigateUrl = "gps_home.aspx"
                adoc1.NavigateUrl = "gps_home.aspx"
                divasset.Visible = False
                divpm1.Visible = False
                divsetting.Visible = False
                divpm.Visible = False
                divvendor.Visible = False
                divapp.Visible = False
                ClientScript.RegisterStartupScript(Me.GetType, "ModalScript", "$(function(){$('#modal-faktur').modal('show'); });", True)
                Exit Select
        End Select

        If Session("role") = "requesters" Then
            divpm1.Visible = False
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Session.Remove("url_det")
    End Sub
End Class
