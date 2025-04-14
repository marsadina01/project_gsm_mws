﻿<%@ Page Language="VB" MasterPageFile="~/MasterPageNew.master" AutoEventWireup="false" CodeFile="DetailWorkOrder.aspx.vb" Inherits="DetailWorkOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css" />
    <link rel="stylesheet" href="bower_components/select2/dist/css/select2.min.css" />
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css" />
    <!-- DataTables -->
    <link rel="stylesheet" href="bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css" />

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Tambahkan FontAwesome untuk ikon -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/js/all.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/js/all.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>



    <script type="text/javascript">
        function showRejectReason() {
                    Swal.fire({
                        title: 'Masukkan alasan reject',
                        icon: 'info', // ✅ Ikon informasi
                        input: 'textarea',
                        inputLabel: 'Alasan',
                        inputPlaceholder: 'Tulis alasan di sini...',
                        inputAttributes: {
                            'aria-label': 'Tulis alasan di sini'
                        },
                        showCancelButton: true,
                        confirmButtonText: 'Submit',
                        cancelButtonText: 'Batal',
                        preConfirm: (reason) => {
                            if (!reason) {
                                Swal.showValidationMessage('Alasan tidak boleh kosong');
                            } else {
                                document.getElementById('<%= hdnRejectReason.ClientID %>').value = reason;
                        }
                    }
                }).then((result) => {
                    if (result.isConfirmed) {
                        __doPostBack('<%= btnReject.UniqueID %>', '');
                    }
                });

            return false;
        }


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


        function showLampiranModal(fileUrl) {
            document.getElementById('iframeLampiran').src = fileUrl;
            document.getElementById('uploadForm').style.display = 'none';
            $('#lampiranModal').modal('show');
        }

        function showUploadForm() {
            document.getElementById('uploadForm').style.display = 'block';
        }

        function toggleUploadBox() {
            var uploadBox = document.getElementById("uploadBox");
            uploadBox.style.display = (uploadBox.style.display === "none" || uploadBox.style.display === "") ? "block" : "none";
        }

        // Drag & Drop Event
        document.getElementById("uploadArea").addEventListener("dragover", function (e) {
            e.preventDefault();
            this.style.background = "#e9ecef";
        });

        document.getElementById("uploadArea").addEventListener("dragleave", function (e) {
            this.style.background = "#fff";
        });

        document.getElementById("uploadArea").addEventListener("drop", function (e) {
            e.preventDefault();
            this.style.background = "#fff";
            alert("File siap diunggah!");
        });

        document.getElementById("fileInput").addEventListener("change", function () {
            alert("File dipilih: " + this.files[0].name);
        });

    </script>

    <style>
        .custom-dark-btn {
            background-color: #343a40;
            color: #fff;
            border: 1px solid #1d2124;
            padding: 10px 20px;
            border-radius: 5px;
            font-weight: bold;
            transition: all 0.3s ease-in-out;
            cursor: pointer;
        }

        /* Warna hijau saat aktif */
        .active-btn {
            background-color: #28a745 !important;
            border-color: #1e7e34 !important;
            color: #fff !important;
        }

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

        .lampiran-btn {
            background-color: #87CEEB !important; /* Biru Langit */
            color: #000 !important; /* Teks Hitam */
            display: inline-block; /* Biar lebarnya sesuai teks */
            width: auto; /* Tidak memenuhi parent */
            max-width: 200px; /* Batasi lebar maksimum */
            padding: 5px 10px; /* Perkecil padding */
            font-weight: bold; /* Tulisan lebih tegas */
            border-radius: 5px; /* Sedikit membulat */
            border: none; /* Tambahkan border agar lebih jelas */
            white-space: nowrap; /* Hindari teks pecah ke bawah */
        }


        .lampiran-btn:hover {
            background-color: #5dade2 !important; /* Warna hover lebih gelap */
            color: #fff !important; /* Teks putih saat hover */
        }

        .timeline {
            list-style: none;
            padding: 0;
            margin: 0;
            position: relative;
        }

        .timeline-container {
            max-height: 300px; /* Sesuaikan tinggi sesuai kebutuhan */
            overflow-y: auto; /* Aktifkan scroll vertikal */
            border: 1px solid #ddd; /* Tambahkan border opsional */
            padding: 10px;
        }

        /* Garis vertikal hanya satu kali */
        .timeline::before {
            content: '';
            position: absolute;
            left: 8px;
            top: 0;
            bottom: 0;
            width: 2px;
            background: #ddd;
        }

        .timeline-item {
            display: flex;
            align-items: center;
            margin-bottom: 8px;
            position: relative;
            padding-left: 25px; /* Biar sejajar dengan garis */
        }

        .timeline-icon {
            width: 16px;
            height: 16px;
            border-radius: 50%;
            background: #bbb;
            display: flex;
            justify-content: center;
            align-items: center;
            color: white;
            font-size: 10px;
            margin-right: 10px;
            position: absolute;
            left: 0;
            top: 50%;
            transform: translateY(-50%);
        }

        .latest .timeline-icon {
            background: #007bff;
        }

        .latest .timeline-text {
            color: #007bff;
        }

        .latest .timeline-date {
            color: #007bff;
        }

        .timeline-date {
            font-size: 12px;
            color: #6c757d;
            margin-right: 8px;
            white-space: nowrap;
        }

        .timeline-text {
            font-size: 14px;
            color: #333;
        }

        .btn-secondary-outline {
          color: #6c757d; /* Warna teks sesuai btn-secondary */
          background-color: transparent;
          border: 2px solid #6c757d; /* Outline sesuai warna btn-secondary */
          padding: 0.375rem 0.75rem;
          font-size: 1rem;
          border-radius: 0.25rem;
          transition: all 0.3s ease-in-out;
        }

        .btn-secondary-outline:hover {
          background-color: #6c757d;
          color: #fff;
        }

        .h-100 {
            height: 100%;
        }

        .fixed-height {
            height: 200px; /* Sesuaikan dengan kebutuhan */
            overflow-y: auto;
        }

/*<<<<<<< HEAD
        .badge {
            font-size: 0.875rem;
            padding: 6px 12px;
            font-weight: 500;
            border-radius: 12px;
        }

=======*/
        .d-flex {
            display: flex;
            justify-content: space-between;
            align-items: center;
            width: 100%;
        }

        asp\:Literal {
            white-space: nowrap; /* Mencegah elemen melompat ke baris berikutnya */
        }

        /* Menambahkan margin atas pada form group */
        .form-group {
            margin-top: 20px; /* Menambah jarak atas untuk alasan reject */
        }

        /* Menambahkan margin bawah pada TextBox */
        .form-group .form-control {
            margin-bottom: 10px;
        }

                .upload-area {
            border: 2px dashed #007bff;
            padding: 20px;
            text-align: center;
            cursor: pointer;
        }

        .upload-area:hover {
            background: #f1f1f1;
        }

        .upload-icon {
            display: inline-block;
            padding: 10px;
            color: #007bff;
            font-size: 18px;
            cursor: pointer;
        }


    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1 style="font-family:Rubik-Regular;">
                <b>Detail Work Order Request External</b>
            </h1>
            <ol class="breadcrumb">
                <li><a href="ViewWorkOrder.aspx"><i class="fa fa-folder-open"></i> WOR</a></li>
                <li>Add New</li>
            </ol>
        </section>

        <section class="content">
            <div class="row" id="detail2" runat="server">
                <!-- Card Informasi Request -->
                <div class="col-md-6">
                    <div class="box">
                        <div class="box-body">
                            <h4 class="box-title"><strong>Information</strong></h4>
                            <table class="table">
                                <tr>
                                    <td>Requestor</td>
                                    <td><asp:Label ID="lblrequestor" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Machine No.</td>
                                    <td><asp:Label ID="lblmesin" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblMoldLabel" runat="server" Text="Mold"></asp:Label></td>
                                    <td><asp:Label ID="lblmold" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- Card Timeline -->
                <div class="col-md-6">
                    <div class="box h-100">
                        <div class="box-body fixed-height">

                            <div class="d-flex justify-content-between align-items-center w-100">
                                <h4 class="box-title m-0"><strong>Timeline</strong></h4>
                                <asp:Literal ID="litBadgeStatus" runat="server" />
                            </div>
                            <hr />
                            <ul class="timeline">
                                <asp:Literal ID="litTimeline" runat="server"></asp:Literal>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>

            <div class="box">
                <div class="box-body">
                    <div id="detail1" runat="server">
                        <div class="form-group row">
                            <label class="col-sm-3 col-form-label">Requestor</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtrequestor" runat="server" class="form-control" disabled></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Machine</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlmachine" runat="server" class="form-control" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group text-center">
                        <asp:LinkButton ID="btnMold" runat="server" CssClass="btn btn-outline-primary mx-2 custom-dark-btn" EnableViewState="true" Text="Mold" Enabled="false"/>
                        <asp:LinkButton ID="btnTool" runat="server" CssClass="btn btn-outline-primary mx-2 custom-dark-btn" EnableViewState="true" Text="Tooling" Enabled="false"/>
                    </div>

                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Mold / Tooling</label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlMoldTool" runat="server" class="form-control" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Gejala Kerusakkan</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtkerusakan" runat="server" class="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Additional Note</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtketerangan" runat="server" class="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:CheckBox ID="chkRepairBy" runat="server" Text="Kirim GS untuk Memperbaiki Mold/Tooling" Enabled="false" />
                    </div>
                    <div id="fclose" runat="server">
                        <div class="form-group row">
                            <label class="col-sm-3 col-form-label">Analisa Kejadian <span style="color: red">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtanalisa" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-3 col-form-label">Perbaikan <span style="color: red">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtperbaikan" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <table class="table table-bordered">
                        <tr>
                            <th>Jumlah Stok</th>
                            <td>
                                <asp:TextBox ID="txtStok" runat="server" CssClass="form-control" Enabled="false" TextMode="Number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>Total Order</th>
                            <td>
                                <asp:TextBox ID="txtTotalOrder" runat="server" CssClass="form-control" Enabled="false" TextMode="Number"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <th>Tgl Produksi Dibutuhkan</th>
                            <td>
                                <asp:TextBox ID="txtTglProduksi" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                            </td>
                        </tr>
                    </table>
                            <!-- Menampilkan alasan reject yang tidak bisa diedit -->
                    <div class="form-group row mt-3">
                        <label ID="LabelRejectReason" runat="server" class="col-sm-3 col-form-label">Alasan Reject</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="TetxtRejectReason" runat="server" class="form-control" disabled></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>



            <div class="box">
                <div class="box-body text-left">
                    <asp:HyperLink ID="lnkLampiran" runat="server" onClick="showLampiranModal" CssClass="btn lampiran-btn" Visible="false">
                        Lihat Lampiran
                    </asp:HyperLink>
                </div>
            </div>

            <!-- Modal untuk menampilkan file -->
            <div id="lampiranModal" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Lampiran</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
            
                        <div class="modal-body text-center">
                            <!-- Preview Lampiran -->
                            <iframe id="iframeLampiran" style="width:100%; height:500px; border:none;"></iframe>

                            <!-- Tombol edit -->
                            <asp:Button ID="btnEditLampiran" runat="server" Text="Edit Lampiran" CssClass="btn btn-warning mt-3" OnClientClick="showUploadForm(); return false;" />

                            <!-- Upload ulang file -->
                            <div id="uploadForm" style="display:none;" class="mt-3">
                                <asp:FileUpload ID="fuLampiranBaru" runat="server" />
                                <asp:Button ID="btnUploadLampiranBaru" runat="server" Text="Upload Ulang" CssClass="btn btn-success" OnClick="btnUploadLampiranBaru_Click" />
                                <br />
                                <asp:Label ID="lblMessageBaru" runat="server" CssClass="d-block mt-2 text-danger" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="box">
                <div class="box-header">
                    <div class="form-group">
                        <asp:Button ID="btnLampiran" runat="server" class="btn btn-secondary" Text="Lampiran WO" OnClientClick="toggleUploadBox(); return false;" />
                    </div>
                </div>

                <!-- Box Body (Awalnya Tersembunyi) -->
                <div class="box-body" id="uploadBox" style="display: none;">
                    <div class="upload-area" id="uploadArea">
                        <label for="fileInput" class="upload-icon">
                        </label>
                        <input type="file" id="fileInput" style="display: none;" />
                        <asp:FileUpload ID="fuLampiran" runat="server" />
<%--                        <asp:Button ID="btnUpload" runat="server" Text="Upload File" OnClick="btnUpload_Click" CssClass="btn btn-primary" />--%>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>


            <div class="form-group d-flex justify-content-between">
                <asp:Button ID="btnReject" runat="server"
                    Text="Reject"
                    class="btn btn-danger"
                    OnClientClick="return showRejectReason();"
                    OnClick="btnReject_Click" />
                <asp:Button ID="btnApprove" runat="server" class="btn btn-success" Text="Approve" OnClick="btnApprove_Click" />
                <asp:Button ID="btnBack" runat="server" class="btn btn-secondary-outline" Text="Back" OnClick="btnBack_Click" />
                <asp:Button ID="btnClose" runat="server" CssClass="btn btn-primary" Text="Close WO" OnClick="btnClose_Click" />
                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-success" Text="Update" Onclick="btnUpdate_Click" />
<%--                <asp:Label ID="lblDebug" runat="server" ForeColor="Red" />--%>

            </div>

            <asp:HiddenField ID="hdnRejectReason" runat="server" />
        </section>
    </div>

    <div id="rejectPopup" class="modal" style="display: none; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); background: white; padding: 20px; border-radius: 10px; box-shadow: 0px 0px 10px gray;">
        <h4>Alasan Reject</h4>
        <asp:TextBox ID="txtRejectReason" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
        <div class="text-right mt-3">
            <asp:Button ID="btnSubmitReject" runat="server" CssClass="btn btn-danger" Text="Submit" OnClick="btnReject_Click" />
            <button type="button" class="btn btn-secondary" onclick="closeRejectPopup();">Cancel</button>
        </div>
    </div>

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
