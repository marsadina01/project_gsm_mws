Imports System.Data
Imports System.Data.SqlClient

Partial Class MasterPageNew
    Inherits System.Web.UI.MasterPage
    Dim connsql As String = ConfigurationManager.ConnectionStrings("Conn").ToString()

    Public Sub logout_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session.RemoveAll()
        Response.Redirect("default.aspx")
    End Sub
    Public Sub changepas_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("Change_Password.aspx")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("role") Is Nothing Then
            Session.RemoveAll()
            Response.Redirect("default.aspx")
        End If

        'Select Case Session("role")
        '    Case ("superadmin")
        '        lihomea.Visible = True
        '        liproject.Visible = True
        '        lipm.Visible = True
        '        limaster.Visible = True
        '        lireport.Visible = True
        '        liwor.Visible = True
        '        liwol.Visible = True
        '        licreatepm.Visible = True
        '        liPMSchedule.Visible = True
        '        liWOPM.Visible = True
        '        Exit Select
        '    Case ("admin")
        '        lihomea.Visible = True
        '        'liproject.Visible = True
        '        lipm.Visible = True
        '        limaster.Visible = True
        '        lireport.Visible = True
        '        'liwor.Visible = True
        '        'liwol.Visible = True
        '        licreatepm.Visible = True
        '        liPMSchedule.Visible = True
        '        liWOPM.Visible = True
        '        Exit Select
        '    Case ("sh")
        '        lipm.Visible = True
        '        liPMSchedule.Visible = True
        '        liWOPM.Visible = True
        '        lilistpm.Visible = True
        '        Exit Select
        '    Case ("dh")
        '        lipm.Visible = True
        '        liPMSchedule.Visible = True
        '        liWOPM.Visible = True
        '        lilistpmdh.Visible = True
        '        Exit Select
        '    Case ("ppc")
        '        lipm.Visible = True
        '        liPMSchedule.Visible = True
        '        'liWOPM.Visible = True
        '        lilistpmppc.Visible = True
        '        Exit Select
        '    Case ("divh")
        '        lipm.Visible = True
        '        liPMSchedule.Visible = True
        '        'liWOPM.Visible = True
        '        lilistpmdiv.Visible = True
        '        Exit Select
        '    Case ("technician")
        '        lipm.Visible = True
        '        liPMSchedule.Visible = True
        '        'liWOPM.Visible = True
        '        'lilistpmdiv.Visible = True
        '        Exit Select
        'End Select

        If Session("role") = "requester" Or
           Session("role") = "atsreq" Or
           Session("role") = "teknisiGS" Or
           Session("role") = "atstekGS" Or
           Session("role") = "teknisiSup" Or
           Session("role") = "atstekSup" Then

            li_master.Visible = False
        End If




        If Not IsPostBack Then
            user.Text = Session("namafull").ToString
            user1.Text = Session("namafull").ToString
            role.Text = Session("role").ToString
            lbljudul.Text = Session("name").ToString

            If Session("role") = "atsreq" Then
                role.Text = "Atasan Requester"
            ElseIf Session("role") = "teknisiSup" Then
                role.Text = "Teknisi Supplier"
            ElseIf Session("role") = "teknisiGS" Then
                role.Text = "Teknisi GS"
            ElseIf Session("role") = "atstekSup" Then
                role.Text = "Atasan Teknisi Supplier"
            ElseIf Session("role") = "atstekGS" Then
                role.Text = "Atasan Teknisi GS"
            End If

            If Session("foto") = "" Then
                img1.Src = "dist/img/avatar.png"
                img2.Src = "dist/img/avatar.png"
            Else
                img1.Src = Session("foto")
                img2.Src = Session("foto")
            End If

        End If
    End Sub
End Class

