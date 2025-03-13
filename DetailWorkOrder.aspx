<%@ Page Language="VB" MasterPageFile="~/MasterPageNew.master" AutoEventWireup="false" CodeFile="DetailWorkOrder.aspx.vb" Inherits="DetailWorkOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css"/>
    <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css"/>
    <link rel="stylesheet" href="bower_components/select2/dist/css/select2.min.css"/>
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css"/>
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css"/>
     <!-- DataTables -->
    <link rel="stylesheet" href="bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css"/>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script type="text/javascript">
        function setActive(button) {
            var buttons = document.querySelectorAll(".custom-dark-btn");
            buttons.forEach(btn => btn.classList.remove("active-btn"));
            button.classList.add("active-btn");
        }

        function showRejectPopup() {
            document.getElementById("rejectPopup").style.display = "block";
        }

        function closeRejectPopup() {
            document.getElementById("rejectPopup").style.display = "none";
        }
    </script>

    <style>
            .file-list {
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
            background-color: #f9f9f9;
        }

        .file-list a {
            display: block;
            padding: 5px;
            color: #007bff;
            font-weight: bold;
            text-decoration: none;
        }

        .file-list a:hover {
            text-decoration: underline;
        }
     </style>

    <style>
    .lampiran-btn {
        background-color: #87CEEB !important; /* Biru Langit */
        color: #000 !important; /* Teks Hitam */
        display: inline-block; /* Biar lebarnya sesuai teks */
        width: auto; /* Tidak memenuhi parent */
        max-width: 200px; /* Batasi lebar maksimum */
        padding: 5px 10px; /* Perkecil padding */
        font-weight: bold; /* Tulisan lebih tegas */
        border-radius: 5px; /* Sedikit membulat */
        border: 1px solid #000; /* Tambahkan border agar lebih jelas */
        text-align: center; /* Posisikan teks di tengah */
        white-space: nowrap; /* Hindari teks pecah ke bawah */
    }

    .lampiran-btn:hover {
        background-color: #5dade2 !important; /* Warna hover lebih gelap */
        color: #fff !important; /* Teks putih saat hover */
    }
</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1><b>Detail Work Order Request External</b></h1>
                        <ol class="breadcrumb">
                <li><a href="ViewWorkOrder.aspx"><i class="fa fa-folder-open"></i> WOR</a></li>
                <li>Add New</li>
            </ol>

        </section>

        <section class="content">
            <div class="box">
                <div class="box-body">
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Requestor</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtrequestor" runat="server" class="form-control" disabled></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Machine</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlmachine" Runat="server" class="form-control" disabled></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group text-center">
                        <asp:LinkButton ID="btnMold" runat="server" class="btn btn-outline-primary mx-2 custom-dark-btn" OnClientClick="setActive(this)" Text="Mold" />
                        <asp:LinkButton ID="btnTool" runat="server" class="btn btn-outline-primary mx-2 custom-dark-btn" OnClientClick="setActive(this)" Text="Tooling" />
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Mold / Tooling</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlMoldTool" runat="server" class="form-control" disabled></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Gejala Kerusakkan</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtkerusakan" runat="server" class="form-control" disabled></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Additional Note</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtketerangan" runat="server" class="form-control" disabled></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                            <asp:CheckBox ID="chkRepairBy" runat="server" Text="Kirim GS untuk Memperbaiki Mold/Tooling" Enabled="false" />
                    </div>

                    <table class="table table-bordered">
                        <tr>
                            <th>Jumlah Stok</th>
                            <td><asp:TextBox ID="txtStok" runat="server" CssClass="form-control" disabled TextMode="Number"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Total Order</th>
                            <td><asp:TextBox ID="txtTotalOrder" runat="server" CssClass="form-control" disabled TextMode="Number"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Tgl Produksi Dibutuhkan</th>
                            <td><asp:TextBox ID="txtTglProduksi" runat="server" CssClass="form-control" disabled></asp:TextBox></td>
                        </tr>
                    </table>

                </div>
            </div>
            <div class="box">
                <div class="form-group row">
                    <div class="col-sm-9 file-list">
                        <div class="form-group row">
                            <div class="col-sm-9">
                                <asp:HyperLink ID="lnkLampiran" runat="server" Target="_blank" CssClass="btn lampiran-btn" Visible="false">
                                    Lihat Lampiran
                                </asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group d-flex justify-content-between">
                <asp:Button ID="btnReject" runat="server" class="btn btn-danger" Text="Reject" OnClientClick="showRejectPopup(); return false;" />
                <asp:Button ID="btnApprove" runat="server" class="btn btn-success" Text="Approve" OnClick="btnApprove_Click" />
            </div>
        </section>
    </div>
    
    <div id="rejectPopup" class="modal" style="display:none; position:fixed; top:50%; left:50%; transform:translate(-50%, -50%); background:white; padding:20px; border-radius:10px; box-shadow:0px 0px 10px gray;">
        <h4>Alasan Reject</h4>
        <asp:TextBox ID="txtRejectReason" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
        <div class="text-right mt-3">
            <asp:Button ID="btnSubmitReject" runat="server" class="btn btn-danger" Text="Submit" OnClick="btnReject_Click" />
            <button type="button" class="btn btn-secondary" onclick="closeRejectPopup();">Cancel</button>
        </div>
    </div>

    <script>
        function closeRejectPopup() {
            document.getElementById("rejectPopup").style.display = "none";
        }
    </script>

    <!-- Bootstrap -->
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="bower_components/fastclick/lib/fastclick.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="dist/js/demo.js"></script>
    <script src="bower_components/select2/dist/js/select2.full.min.js"></script>

</asp:Content>
