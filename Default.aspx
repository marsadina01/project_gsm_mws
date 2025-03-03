<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html>
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <title>GSM | GS Supplier Management</title>
    <link rel="icon" href="Images/logo2.png" type="image/x-icon" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <link rel="stylesheet" href="flat-login-form/css/style1.css">
    <link rel="stylesheet" href="bower_components/limonte-sweetalert2/css/sweetalert2.min.css" />
    <link rel="stylesheet" href="bower_components/limonte-sweetalert2/css/all.css" integrity="sha384-fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr" crossorigin="anonymous" />
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css">
    <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css">
    <link rel="stylesheet" href="bower_components/select2/dist/css/select2.min.css">
    <link rel="stylesheet" href="bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css">
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css">
    <script type="text/javascript" src="flat-login-form/js/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script> <!-- Updated SweetAlert2 CDN -->
    <script type="text/javascript">
        function OTP_Salah() {
            Swal.fire({
                icon: 'error',
                title: '<span style="font-family:Rubik-Regular;">OTP yang Anda masukkan salah atau kadaluarsa, coba login kembali!</span>',
                confirmButtonColor: '#DC3545',
                confirmButtonText: 'Kembali',
                allowOutsideClick: false
            });
        }
    </script>

    <style>
        .bg {
            background-image: url("dist/img/Background.png");
            background-size: cover;
            background-repeat: no-repeat;
            background-position: center center;
        }

        body {
            font-family: Rubik-Regular;
        }

        @font-face {
            font-family: Rubik-Regular;
            src: url('fonts/Rubik-Regular.ttf');
        }


        @font-face {
            font-family: Rubik-Medium;
            src: url('fonts/Rubik-Medium.ttf');
        }

        .form .title {
            font-family: Rubik-Regular;
            font-size: 3rem;
            color: #333;
            font-weight: bold;
            display: inline-block;
            vertical-align: middle;
        }

        h5 {
            font-family: Rubik-Medium;
            color: #EC3434;
        }

        .footer {
            width: 100%;
            font-family: Rubik-Regular;
            font-size: 1 em;
            display: flex;
            justify-content: flex-start;
            display: -webkit-box;
            display: -moz-box;
            display: -ms-flexbox;
            display: -webkit-flex;
            align-items: center;
            justify-content: center;
            color: darkgray;
        }

        #LoginButton {
            display: block;
            margin-top: 10px;
            width: 100%;
            height: 50px;
            border-radius: 5px;
            outline: none;
            border: none;
            background-image: linear-gradient(to right, #605ca8, #4C4986, #302e54);
            background-size: 200%;
            font-size: 14px;
            color: #fff;
            font-family: Rubik-Medium;
            text-transform: uppercase;
            letter-spacing: 2px;
            cursor: pointer;
            transition: .5s;
        }

            .btn:hover {
                background-position: right;
            }

        .alert-custom {
            background-color: #EDEDED;
        }

        .cnt {
            display: flex;
            align-items: center;
        }

        .canvas {
            padding: 10px;
            width: 800px;
            font-size: 14px;
        }

        .enabled {
            padding: 10px 20px;
            background-color: cornflowerblue;
            color: white;
            border: none;
            border-radius: 50%;
            font-size: 14px;
            cursor: pointer;
            margin-left: 10px;
        }

        #errorCap {
            color: red
        }

        #correctCap {
            color: green
        }

        .modal-backdrop.in {
            opacity: 0;
        }

        .modal-backdrop {
            z-index: 0;
        }
    </style>
</head>

<body class="bg" style="min-height: 100%">
    <div class="row">
        <div class="col-xs-12 col-lg-12" id="form-login">
            <div class="form" style="margin-top: 200px; border-radius: 10px;">
                <asp:Label ID="InvalidCredentialsMessage" runat="server" ForeColor="Red" Text="Your username or password is invalid. Please try again." Visible="False"></asp:Label>

                <div>
                    <h2><span class="title" style="font-family: Rubik-Regular; font-size: 40px !important;">GSM</span></h2>
                    <div class="footer" style="font-family: Rubik-Regular; font-size: 18px !important; margin-top: 10px;">
                        <label>GS Supplier Management</label>
                    </div>
                </div>

                <br />
                <hr style="margin-top: 10px; margin-bottom: 10px; border: 2px solid #605ca8; border-radius: 5px; width: 10%;" />
                <br />

                <form id="Form2" runat="server">
                    <div class="form-group">
                        <div class="input-group" style="padding-top: 5px;">
                            <span class="input-group-addon"><i class="fa fa-user" style="color: lightgray"></i></span>
                            <asp:TextBox runat="server" class="form-control" ID="txtucript" AutoComplete="off" placeholder="Username" Text="" MaxLength="40"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="input-group" style="padding-top: 5px;">
                            <span class="input-group-addon"><i class="fa fa-lock" style="color: lightgray"></i></span>
                            <asp:TextBox runat="server" class="form-control" ID="txtpcript" TextMode="Password" placeholder="Password" Text="" MaxLength="30"></asp:TextBox>
                        </div>
                    </div>

                    <div class="cnt">
                        <canvas id="canvas" class="canvas"></canvas>
                        <button id="valid" class="buttonRefresh" type="button" style="background-color:lightgray;width:20%;"><i class="fa fa-refresh" aria-hidden="true"></i></button>
                    </div>

                    <div class="form-group">
                        <div class="input-group input-group-sm" style="padding-top: 5px;">
                            <input class="form-control" name="code" id="code" autocomplete="off" placeholder="Isi kode captcha" />
                            <span class="input-group-btn">
                                <button id="checkCap" class="btn" type="button" style="color:#FFF;background-color:#605ca8;">Check Captcha</button>
                            </span>
                        </div>
                    </div>

                    <div class="modal fade" id="modal-otp-wa">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header bg-gray-light">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title"><b>OTP WA</b></h4>
                                </div>
                                <div class="modal-body">
                                    <div runat="server" id="div_masukkan_wa">
                                        <asp:Label runat="server">Masukkan OTP yang dikirimkan ke WA Anda!<label style="color: #FF0000;"> *</label></asp:Label>
                                        <asp:TextBox class="form-control" runat="server" ID="txb_masukkan_otp_wa" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                                <%--<div class="modal-footer">
                                    <asp:LinkButton runat="server" ID="lbt_check_otp_wa" class="btn btn-success" OnClick="lbt_check_otp_wa_Click">Cek OTP</asp:LinkButton>
                                </div>--%>
                            </div>
                        </div>
                    </div>

                    <span id="errorCap"></span>
                    <span id="correctCap"></span>

                    <div class="alert alert-dismissible alert-custom" style="text-align: left; padding-right: 10px; padding-left: 10px;">
                        <ul style="padding-left: 30px;">
                            <%--<li>
                                <asp:Label ID="label3" Font-Bold="true" ForeColor="Orchid" Font-Size="Small" runat="server" Text="MAINTENANCE PORTAL, sehingga Vendor Tidak Dapat Login pada: 3 Feb 2024 pukul 19:00-01:00 WIB"></asp:Label></li>--%>
                            <li>
                                <asp:Label ID="label" Font-Size="Small" runat="server" Text="Aplikasi hanya bisa digunakan oleh Vendor dan Internal GS"></asp:Label></li>
                            <li>
                                <asp:Label ID="label1" Font-Size="Small" runat="server" Text="Login menggunakan akun yang sudah terdaftar di database GS Battery"></asp:Label></li>
                            <li>
                                <asp:Label ID="label2" Font-Size="Small" runat="server" Text="Apabila ada kesulitan login dapat menghubungi IT Dept. di nomor (0267) 440962 ext 3600"></asp:Label></li>
                        </ul>
                    </div>

                    <hr />

                    <asp:Button ForeColor="White" ID="LoginButton" runat="server" Text="Masuk" CssClass="btn" OnClick="LoginButton_Click" />
                    <br />
                </form>
            </div>
        </div>
    </div>

    <div class="row" style="display: none;">
        <div class="col-xs-12 col-lg-12" id="form-2-step">
            <div class="form" style="margin-top: 200px; border-radius: 10px;">
                <div>
                    <h2><span class="title" style="font-family: Rubik-Regular; font-size: 40px !important;">2-Step Verification</span></h2>
                    <div class="footer" style="font-family: Rubik-Regular; font-size: 18px !important; margin-top: 10px;">
                        <label>GS Supplier Management</label>
                    </div>
                </div>

                <br />
                <hr style="margin-top: 10px; margin-bottom: 10px; border: 2px solid #605ca8; border-radius: 5px; width: 10%;" />
                <br />
            </div>
        </div>
    </div>

    <script type="text/javascript" src="flat-login-form/js/jquery.min.js"></script>
    <script type="text/javascript" src="flat-login-form/js/index.js"></script>
    <script type="text/javascript" src="dist/js/adminlte.min.js"></script>
    <script type="text/javascript" src="bower_components/limonte-sweetalert2/js/sweetalert2.min.js"></script>
    <script type="text/javascript" src="bower_components/fastclick/lib/fastclick.js"></script>
    <script type="text/javascript" src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="bower_components/select2/dist/js/select2.full.min.js"></script>
    <script type="text/javascript" src="dist/jquery-captcha.min.js"></script>
    <script type="text/javascript" src="dist/jqueryc.js"></script>

    <script type="text/javascript">
        $(function () {
            $('.select2').select2()

        })        
    </script>
</body>
</html>
