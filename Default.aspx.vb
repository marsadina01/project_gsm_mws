Imports System.Data
Imports System
Imports System.Data.SqlClient
Imports System.DirectoryServices
Imports System.Security.Cryptography
Imports System.Data.SqlClient.SqlConnection
Imports System.Configuration
Imports System.Text
Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Net
Imports System.Net.Mail

Partial Class _Default
    Inherits System.Web.UI.Page
    Dim strConnString As [String] = DecryptString(System.Configuration.ConfigurationManager.ConnectionStrings("Conn").ConnectionString)
    Dim strConnHR As [String] = DecryptString(System.Configuration.ConfigurationManager.ConnectionStrings("ConnMaster").ConnectionString)
    Private MessageBox As Object
    Private Shared prevPage As String = String.Empty

    Dim email_notifier As String = ConfigurationManager.AppSettings("SubcontNotifierEmail").ToString
    Dim password_notifier As String = ConfigurationManager.AppSettings("SubcontNotifierPassword").ToString
    Dim server_host As String = ConfigurationManager.AppSettings("SubcontNotifierServerHost").ToString
    Dim server_port As String = ConfigurationManager.AppSettings("SubcontNotifierServerPort").ToString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        txtucript.Focus()
    End Sub

    Protected Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim SQLF As String
        Dim commlogin As New SqlCommand
        Dim adlogin As New SqlDataAdapter
        Dim dvlogin As New DataTable
        Dim lblMsg As String
        Dim lblMs As String
        Dim admin As String
        Dim seksi As String

        Dim connFam As New SqlConnection(strConnString)
        SQLF = "SELECT md.*, us.user_nama FROM db_master_data.dbo.VIEW_DATAAUTH md join db_purchasing.dbo.tlkp_user us on md.emp_no=us.user_npk WHERE emp_no = " & txtucript.Text & ""

        Response.Write("1 SELECT md.*, us.user_nama FROM db_master_data.dbo.VIEW_DATAAUTH md join db_purchasing.dbo.tlkp_user us on md.emp_no=us.user_npk WHERE emp_no = '" & txtucript.Text & "'  ")

        commlogin.Connection = connFam
        commlogin.CommandText = SQLF
        adlogin.SelectCommand = commlogin
        adlogin.Fill(dvlogin)
        If dvlogin.Rows.Count > 0 Then
            Try
                If dvlogin.Rows.Count > 0 Then
                    If txtucript.Text = "" Then
                        lblMsg = "Silahkan Isi Username dan Password Anda!"
                        Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                    End If
                    'Dim initLDAPPath = "dc=gs, dc=astra, dc=co, dc=id"
                    'Dim initLDAPServer = "10.19.48.7"
                    'Dim initShortDomainName = "gs"
                    'Dim strErrMsg As String

                    'Dim DomainAndUsername As String = ""
                    'Dim strCommu As String
                    Dim flgLogin As Boolean = False
                    'strCommu = "LDAP://" & initLDAPServer & "/" & initLDAPPath
                    'DomainAndUsername = initShortDomainName & "\" & dvlogin.Rows(0)("user_nama")

                    'Dim entry As New DirectoryEntry(strCommu, DomainAndUsername, DecryptStringPass(txtpcript.Text))
                    'Dim obj As Object
                    'Try
                    '    obj = entry.NativeObject
                    '    Dim search As New DirectorySearcher(entry)
                    '    Dim result As SearchResult
                    '    search.Filter = "(SAMAccountName=" + dvlogin.Rows(0)("user_nama") + ")"
                    '    search.PropertiesToLoad.Add("cn")
                    '    result = search.FindOne()

                    '    If result Is Nothing Then
                    '        flgLogin = False
                    '    Else
                    flgLogin = True
                    '    End If
                    'Catch ex As Exception
                    '    flgLogin = False
                    'End Try
                    If flgLogin = True Then
                        Dim dv As DataTable = get_data_pch("SELECT * FROM tlkp_user WHERE user_npk = '" & txtucript.Text & "'")

                        If dv.Rows.Count > 0 Then
                            Dim userRow As DataRow = dv.Rows(0)

                            ' Set session variables
                            Session("foto") = ""
                            Session("name") = dvlogin.Rows(0)("first_name")
                            Session("namafull") = Trim(dvlogin.Rows(0)("Full_Name").ToString())
                            Session("npk") = Trim(dvlogin.Rows(0)("emp_no").ToString())
                            Session("no_telp") = Trim(userRow("user_no_telp").ToString)
                            Session("role1") = Trim(userRow("user_role1").ToString())
                            Session("email") = Trim(userRow("user_email").ToString())
                            Session("nameinfor") = If(userRow.Table.Columns.Contains("bp_infor"), Trim(userRow("bp_infor").ToString()), "")

                            ' Handle roles using Select Case
                            Select Case userRow("user_role").ToString().ToLower()
                                Case "admin", "superadmin", "requester", "atsreq", "teknisisup", "teknisigs", "atsteksup", "atstekgs"
                                    Session("role") = userRow("user_role").ToString()

                                    'If userRow("user_status_otp") = 1 Then
                                    '    If (userRow("user_email") Is DBNull.Value OrElse userRow("user_email").ToString() = "") And (userRow("user_no_telp") Is DBNull.Value OrElse userRow("user_no_telp").ToString() = "") Then
                                    '        Response.Redirect("Profile.aspx")
                                    '    Else
                                    '        Dim otp As String = GenerateOTP(6)
                                    '        Session("otp") = otp

                                    '        SendOTP(otp)

                                    '        Dim subject1 As String = "LOGIN PORTAL GSM"
                                    '        Dim body1 As String = "<br /> Dear <b> " + Session("namafull") + "</b>,<br /><br /> Berikut adalah kode verifikasi Anda : <b>" + Session("otp") + "</b> untuk portal <b>GSM</b>.<br />Demi keamanan, jangan bagikan kode ini.<br /><br />Terima kasih <br /><br /> -- <br /><br /><b>PT. GS Battery</b><br />Kawasan Surya Cipta Swadaya<br />Jl. Surya Utama Kav. I3 - I4, Karawang Timur, Kutamekar, Kec. Ciampel<br />Kabupaten Karawang, Jawa Barat 41363<br />Telp : (0267) 440962"

                                    '        SendEmail(Session("email"), subject1, body1)
                                    '    End If
                                    'Else
                                    Response.Redirect("Home.aspx")
                                    'End If
                                Case Else
                                    lblMsg = "You Don't Have Access Rights to the Maintenance Portal"
                                    Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                            End Select
                        Else
                            lblMsg = "Username not found."
                            ' Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                        End If
                    Else
                        lblMsg = "Username Not Found"
                        Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                    End If
                Else
                    lblMsg = "Username Not Found"
                    Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                End If
            Catch ex As Exception
                'lblMsg = "Login Error Occurred"
                'Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                Response.Write("asd" & ex.Message)
            End Try
        Else
            Dim dvNonEmp As DataTable = get_data_pch("SELECT * FROM tlkp_user WHERE user_nama= '" & txtucript.Text & "' and user_status = '1'")

            If dvNonEmp.Rows.Count > 0 Then
                If txtucript.Text = "" Then
                    lblMsg = "Silahkan Isi Username dan Password Anda!"
                    Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                End If
                Dim initLDAPPath = "dc=gs, dc=astra, dc=co, dc=id"
                Dim initLDAPServer = "10.19.48.7"
                Dim initShortDomainName = "gs"
                Dim strErrMsg As String

                Dim DomainAndUsername As String = ""
                Dim strCommu As String
                Dim flgLogin As Boolean = False
                strCommu = "LDAP://" & initLDAPServer & "/" & initLDAPPath
                DomainAndUsername = initShortDomainName & "\" & dvNonEmp.Rows(0)("user_nama")

                Dim entry As New DirectoryEntry(strCommu, DomainAndUsername, DecryptStringPass(txtpcript.Text))
                Dim obj As Object
                Try
                    obj = entry.NativeObject
                    Dim search As New DirectorySearcher(entry)
                    Dim result As SearchResult
                    search.Filter = "(SAMAccountName=" + dvNonEmp.Rows(0)("user_nama") + ")"
                    search.PropertiesToLoad.Add("cn")
                    result = search.FindOne()

                    If result Is Nothing Then
                        flgLogin = False
                    Else
                        flgLogin = True
                    End If
                Catch ex As Exception
                    flgLogin = False
                End Try
                If flgLogin = True Then
                    Dim dvEmpTrue As DataTable = get_data_pch("SELECT * FROM tlkp_user WHERE user_nama = '" & txtucript.Text & "'")

                    If dvEmpTrue.Rows.Count > 0 Then
                        Dim userRow As DataRow = dvEmpTrue.Rows(0)

                        ' Set session variables
                        Session("foto") = Trim(userRow("user_foto").ToString())
                        Session("name") = Trim(userRow("user_nama").ToString())
                        Session("namafull") = Trim(userRow("user_namafull").ToString())
                        Session("npk") = Trim(userRow("user_npk").ToString())
                        Session("no_telp") = Trim(userRow("user_no_telp").ToString)
                        Session("role1") = Trim(userRow("user_role1").ToString())
                        Session("email") = Trim(userRow("user_email").ToString())
                        Session("nameinfor") = If(userRow.Table.Columns.Contains("bp_infor"), Trim(userRow("bp_infor").ToString()), "")

                        ' Handle roles using Select Case
                        Select Case userRow("user_role").ToString().ToLower()
                            Case "admin", "mpc", "purchasing", "qc", "eng", "mws", "she", "superadmin", "vendor", "fin"
                                Session("role") = userRow("user_role").ToString()

                                'If userRow("user_status_otp") = 1 Then
                                '    If (userRow("user_email") Is DBNull.Value OrElse userRow("user_email").ToString() = "") And (userRow("user_no_telp") Is DBNull.Value OrElse userRow("user_no_telp").ToString() = "") Then
                                '        Response.Redirect("Profile.aspx")
                                '    Else
                                '        Dim otp As String = GenerateOTP(6)
                                '        Session("otp") = otp

                                '        SendOTP(otp)

                                '        Dim subject1 As String = "LOGIN PORTAL GSM"
                                '        Dim body1 As String = "<br /> Dear <b> " + Session("namafull") + "</b>,<br /><br /> Berikut adalah kode verifikasi Anda : <b>" + Session("otp") + "</b> untuk portal <b>GSM</b>.<br />Demi keamanan, jangan bagikan kode ini.<br /><br />Terima kasih <br /><br /> -- <br /><br /><b>PT. GS Battery</b><br />Kawasan Surya Cipta Swadaya<br />Jl. Surya Utama Kav. I3 - I4, Karawang Timur, Kutamekar, Kec. Ciampel<br />Kabupaten Karawang, Jawa Barat 41363<br />Telp : (0267) 440962"

                                '        SendEmail(Session("email"), subject1, body1)
                                '    End If
                                'Else
                                '    Response.Redirect("Home.aspx")
                                'End If
                            Case Else
                                lblMsg = "You Don't Have Access Rights to the Maintenance Portal"
                                Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                        End Select
                    Else
                        lblMsg = "Username not found."
                        Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                    End If
                Else
                    Dim dvVendor As DataTable = get_data_pch("SELECT * FROM tlkp_user WHERE user_nama= '" & txtucript.Text & "' and user_pass='" & EncryptString(Trim(Me.DecryptStringPass(txtpcript.Text))) & "'")
                    Response.Write("SELECT * FROM tlkp_user WHERE user_nama= '" & txtucript.Text & "' and user_pass='" & EncryptString(Trim(Me.DecryptStringPass(txtpcript.Text))) & "'")
                    If dvVendor.Rows.Count > 0 Then
                        Dim userRow As DataRow = dvVendor.Rows(0)

                        ' Set session variables
                        Session("foto") = Trim(userRow("user_foto").ToString())
                        Session("name") = Trim(userRow("user_nama").ToString())
                        Session("namafull") = Trim(userRow("user_namafull").ToString())
                        Session("npk") = Trim(userRow("user_npk").ToString())
                        Session("no_telp") = Trim(userRow("user_no_telp").ToString)
                        Session("role1") = Trim(userRow("user_role1").ToString())
                        Session("email") = Trim(userRow("user_email").ToString())
                        Session("nameinfor") = If(userRow.Table.Columns.Contains("bp_infor"), Trim(userRow("bp_infor").ToString()), "")

                        ' Handle roles
                        Select Case userRow("user_role").ToString().ToLower()
                            Case "admin", "mpc", "purchasing", "qc", "eng", "mws", "she", "superadmin", "vendor", "vendorfin", "fin"
                                Session("role") = userRow("user_role").ToString()

                                'If userRow("user_status_otp") = 1 Then
                                '    If (userRow("user_email") Is DBNull.Value OrElse userRow("user_email").ToString() = "") And (userRow("user_no_telp") Is DBNull.Value OrElse userRow("user_no_telp").ToString() = "") Then
                                '        Response.Redirect("Profile.aspx")
                                '    Else
                                '        Dim otp As String = GenerateOTP(6)
                                '        Session("otp") = otp

                                '        SendOTP(otp)

                                '        Dim subject1 As String = "LOGIN PORTAL GSM"
                                '        Dim body1 As String = "<br /> Dear <b> " + Session("namafull") + "</b>,<br /><br /> Berikut adalah kode verifikasi Anda : <b>" + Session("otp") + "</b> untuk portal <b>GSM</b>.<br />Demi keamanan, jangan bagikan kode ini.<br /><br />Terima kasih <br /><br /> -- <br /><br /><b>PT. GS Battery</b><br />Kawasan Surya Cipta Swadaya<br />Jl. Surya Utama Kav. I3 - I4, Karawang Timur, Kutamekar, Kec. Ciampel<br />Kabupaten Karawang, Jawa Barat 41363<br />Telp : (0267) 440962"

                                '        SendEmail(Session("email"), subject1, body1)
                                '    End If
                                'Else
                                '    Response.Redirect("Home.aspx")
                                'End If
                            Case Else
                                lblMsg = "You Don't Have Access Rights to the Maintenance Portal"
                                Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                        End Select
                    Else
                        'lblMsg = "Username/Password salah!"
                        'Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
                    End If

                End If
            Else
                lblMsg = "Username not found."
                Response.Write("<script>alert('" + lblMsg + "');window.location='Default.aspx';window.open='Default.aspx'</script>")
            End If
        End If
    End Sub

    Private Sub SendEmail(ByVal recipientEmail As String, ByVal subject As String, ByVal body As String)
        Try
            Dim dt3 As New DataTable()

            Dim SmtpServer As New SmtpClient()
            Dim mail As New MailMessage()

            SmtpServer.Credentials = New Net.NetworkCredential(email_notifier, password_notifier)
            SmtpServer.Port = Convert.ToInt32(server_port)
            SmtpServer.Host = server_host
            SmtpServer.EnableSsl = True
            mail = New MailMessage()
            mail.From = New MailAddress(email_notifier)

            mail.To.Add(recipientEmail)
            mail.Subject = subject
            mail.Body = body

            mail.IsBodyHtml = True

            SmtpServer.Send(mail)

        Catch ex As SmtpException
            ' Handle SMTP-related exceptions (e.g., invalid credentials, unable to connect to SMTP server)
            ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), "Gagal_Email();", True)

        Catch ex As Exception
            ' Handle other exceptions that may occur during email sending
            ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), "Gagal_Email();", True)
        End Try
    End Sub

    '----------------------------------------------------------------- SEND OTP -----------------------------------------------------------------

    'Private Function GenerateOTP(length As Integer) As String
    '    Dim otp As New Random()
    '    Dim otpDigits As String = ""
    '    For i As Integer = 1 To length
    '        otpDigits &= otp.Next(0, 9).ToString()
    '    Next
    '    Return otpDigits
    'End Function

    'Private Async Sub SendOTP(otp As String)
    '    Dim AppId As String = "jfeew-nrqhl0suw7ildgn"
    '    Dim SecretKey As String = "c5aa59958fa978ce8f3caf49114d39ab"
    '    Dim QiscusApiUrl As String = "https://multichannel.qiscus.com/whatsapp/v1/jfeew-nrqhl0suw7ildgn/4062/messages"
    '    Dim toPhoneNumber As String = Session("no_telp") ' Recipient's WhatsApp number

    '    Dim payload = New With {
    '        .to = toPhoneNumber,
    '        .type = "template",
    '        .template = New With {
    '            .namespace = "f777b733_7595_466e_8b97_359c278ed54e",
    '            .name = "pesanotp_all",
    '            .language = New With {
    '                .policy = "deterministic",
    '                .code = "id"
    '            },
    '            .components = New Object() {
    '                New With {
    '                    .type = "body",
    '                    .parameters = New Object() {
    '                        New With {
    '                            .type = "text",
    '                            .text = otp
    '                        }
    '                    }
    '                },
    '                New With {
    '                    .type = "button",
    '                    .sub_type = "url",
    '                    .index = "0",
    '                    .parameters = New Object() {
    '                        New With {
    '                            .type = "text",
    '                            .text = otp
    '                        }
    '                    }
    '                }
    '            }
    '        }
    '    }

    '    Dim jsonPayload As String = JsonConvert.SerializeObject(payload)

    '    Dim content As New StringContent(jsonPayload, Encoding.UTF8, "application/json")

    '    Dim client As New HttpClient()
    '    client.DefaultRequestHeaders.Add("Qiscus-App-Id", AppId)
    '    client.DefaultRequestHeaders.Add("Qiscus-Secret-Key", SecretKey)

    '    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
    '    Dim response As HttpResponseMessage = Await client.PostAsync(QiscusApiUrl, content)
    '    If response.IsSuccessStatusCode Then
    '        Console.WriteLine("Message sent successfully: " & response.StatusCode.ToString())
    '    Else
    '        Console.WriteLine("Failed to send message: " & response.StatusCode.ToString())
    '    End If

    '    Page.ClientScript.RegisterStartupScript(Me.[GetType](), "modelBox", "<script>$(function() { $('#modal-otp-wa').modal('show'); });</script>", False)
    'End Sub

    'Protected Sub lbt_check_otp_wa_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    If txb_masukkan_otp_wa.Text = Session("otp") Then
    '        Response.Redirect("Home.aspx")
    '    Else
    '        ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), "OTP_Salah();", True)
    '    End If
    'End Sub

    '----------------------------------------------------------------- GET DATA DB -----------------------------------------------------------------

    Private Shared Function get_data_pch(ByVal query As String) As DataTable
        Dim dt As New DataTable()
        Dim conn As String = DecryptString(ConfigurationManager.ConnectionStrings("Conn").ToString())
        Using con As New SqlConnection(conn)
            Using cmd As New SqlCommand(query)
                Using sda As New SqlDataAdapter()
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    sda.Fill(dt)
                End Using
            End Using
            Return dt
        End Using
    End Function

    Private Function DecryptStringPass(ByVal str As String) As String
        Dim decrypted As String = ""
        For Each ch As Char In str
            Dim charCode As Integer = Asc(ch) - 1
            decrypted += Chr(charCode)
        Next
        Return decrypted
    End Function

    Public Function MD5(ByVal strString As String) As String
        Dim ASCIIenc As New ASCIIEncoding
        Dim strReturn As String
        Dim ByteSourceText() As Byte = ASCIIenc.GetBytes(strString)
        Dim Md5Hash As New MD5CryptoServiceProvider
        Dim ByteHash() As Byte = Md5Hash.ComputeHash(ByteSourceText)

        strReturn = ""

        For Each b As Byte In ByteHash
            strReturn = strReturn & b.ToString("x2")
        Next

        Return strReturn

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

    Public Shared Function EncryptString(ByVal stringToEncrypt As String) As String
        Dim DES As New TripleDESCryptoServiceProvider
        Dim mKey As String = "PasswordKey"
        DES.Key = MD5Hash(mKey)
        DES.Mode = CipherMode.ECB
        Dim Buffer As Byte() = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt)
        Return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length))
    End Function
End Class