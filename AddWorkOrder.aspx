<%@ Page Language="VB" MasterPageFile="~/MasterPageNew.master" AutoEventWireup="false" CodeFile="AddWorkOrder.aspx.vb" Inherits="AddWorkOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="bower_components/select2/dist/css/select2.min.css"/>
    <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css"/>
    <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css"/>
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css"/>
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css"/>
     <!-- DataTables -->
    <link rel="stylesheet" href="bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" rel="stylesheet" />

    <!-- Pastikan jQuery dipanggil lebih awal -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>

    <!-- SweetAlert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            initSelect2();

            var url = window.location;
            $('.sidebar .sidebar-menu').find('.active').removeClass('active');
            $('.sidebar .sidebar-menu li a').each(function () {
                if (this.href == url) {
                    $(this).parent().addClass('treeview active');
                }
            });
        });

        function setActive(button) {
            // Ambil semua tombol dengan class 'custom-dark-btn'
            var buttons = document.querySelectorAll(".custom-dark-btn");

            // Hapus class 'active-btn' dari semua tombol
            buttons.forEach(btn => btn.classList.remove("active-btn"));

            // Tambahkan class 'active-btn' ke tombol yang diklik
            button.classList.add("active-btn");
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

        function validateForm() {
            var kerusakan = document.getElementById('<%= txtKerusakan.ClientID %>').value.trim();
            if (kerusakan === "") {
                alert("Gejala Kerusakan harus diisi!");
                return false;
            }
            return true;
        }

        function initSelect2() {
            var ddlMachineID = '<%= ddlmachine.ClientID %>';
            $('#' + ddlMachineID).select2({
                placeholder: "-- Pilih No Machine --",
                allowClear: true
            });

            var ddlMoldToolID = '<%= ddlMoldTool.ClientID %>';
            $('#' + ddlMoldToolID).select2({
                placeholder: "-- Pilih ID Mold / Tool --",
                allowClear: true
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            initSelect2();
        });

    </script>

    <style>

        .select2-container .select2-selection--single {
            height: 38px !important;
            padding: 5px;
            border-radius: 4px;
        }

        /* Warna default */
        .custom-dark-btn {
            background-color: #343a40; /* Warna dasar dark */
            color: #fff;
            border: 1px solid #1d2124;
            padding: 10px 20px;
            border-radius: 5px;
            font-weight: bold;
            transition: all 0.3s ease-in-out;
            cursor: pointer;
        }

        /* Warna hijau saat aktif */
        .custom-dark-btn.active-btn {
            background-color: #28a745 !important;
            border-color: #1e7e34 !important;
            color: #fff !important;
        }

        .left, .right {
            flex: 1; /* Menyebarkan ruang antara elemen */
        }

        .left {
            text-align: left;
        }

        .right {
            text-align: right;
        }

        .btn-custom {
            display: inline-block; /* Memastikan keduanya memiliki properti display yang sama */
            padding: 10px 20px; /* Samakan padding */
            font-size: 16px; /* Samakan ukuran font */
            height: 40px; /* Samakan tinggi */
            line-height: 20px; /* Pastikan teks sejajar */
            border-radius: 5px; /* Pastikan bentuk seragam */
            vertical-align: middle;
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

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager runat="server" />
    <div class="content-wrapper">
        <section class="content-header">
            <h1 style="font-family:Rubik-Regular;">
                <b>Add Work Order Request External</b>
            </h1>
            <ol class="breadcrumb">
                <li><a href="ViewWorkOrder.aspx"><i class="fa fa-folder-open"></i> WOR</a></li>
                <li>Add New</li>
            </ol>
        </section>
        
        <section class="content">
            <div class="box">
                <div class="box-body">
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Requestor <span style="color: red">*</span></label>
                        <div class="col-sm-9">
                            <asp:Label visible="false" ID="lblidreq" runat="server" />
                            <asp:TextBox ID="txtrequestor" runat="server" class="form-control" disabled></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Machine <span style="color: red">*</span></label>
                        <div class="col-sm-9">   
                            <asp:DropDownList ID="ddlmachine" runat="server" CssClass="form-control select2" required="true">
                                <asp:ListItem Text="-- Pilih No Machine --" Value="" Selected="True" Disabled="True"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group d-flex justify-content-center w-100 mt-3">
                        <div class="text-center">
                            <asp:LinkButton ID="btnMold" runat="server" class="btn custom-dark-btn mx-2" 
                                Text="Mold" OnClientClick="setActive(this)" OnClick="btnMold_Click" />
                            <asp:LinkButton ID="btnTool" runat="server" class="btn custom-dark-btn mx-2" 
                                Text="Tooling" OnClientClick="setActive(this)" OnClick="btnTool_Click" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Mold / Tooling <span style="color: red">*</span></label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlMoldTool" runat="server" CssClass="form-control select2" ClientIDMode="Static" onchange="logSelectedMoldTool()">
                                <asp:ListItem Text="-- Pilih ID Mold / Tool --" Value="" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Gejala Kerusakkan <span style="color: red">*</span></label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtKerusakan" runat="server" CssClass="form-control" placeholder="Gejala Kerusakan"></asp:TextBox>
                            <asp:CustomValidator ID="cvKerusakan" runat="server" ControlToValidate="txtKerusakan" ClientValidationFunction="validateKerusakan"
                                ErrorMessage="Gejala Kerusakan harus diisi." Display="Dynamic" ForeColor="Red" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Total Data Shots <span style="color: red">*</span></label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtDatashot" runat="server" CssClass="form-control" TextMode="Number" placeholder="Total Data Shots"></asp:TextBox>
                            <asp:CustomValidator ID="cvDatashot" runat="server" ControlToValidate="txtDatashot" ClientValidationFunction="validateDatashot"
                                ErrorMessage="Total Data Shots harus diisi." Display="Dynamic" ForeColor="Red" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Additional Note</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtketerangan" runat="server" class="form-control" placeholder="Keterangan Tambahan"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:CheckBox ID="chkKirimGS" runat="server" /> Kirim GS untuk Memperbaiki Mold/Tooling
                    </div>
                    
                    <table class="table table-bordered">
                        <tr>
                            <th>Jumlah Stok</th>
                            <td><asp:TextBox ID="txtStok" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Total Order</th>
                            <td><asp:TextBox ID="txtTotalOrder" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Tgl Produksi Dibutuhkan</th>
                            <td><asp:TextBox ID="txtTglProduksi" runat="server" CssClass="form-control"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="box">
                <div class="box-header">
                    <div class="form-group">
                        <asp:Button ID="btnLampiran" runat="server" class="btn btn-secondary" Text="Lampiran" OnClientClick="toggleUploadBox(); return false;" />
                    </div>
                </div>

                <!-- Box Body (Awalnya Tersembunyi) -->
                <div class="box-body" id="uploadBox" style="display: none;">
                    <div class="upload-area" id="uploadArea">
                        <label for="fileInput" class="upload-icon">
                            <i class="fa fa-upload"></i> Pilih File
                        </label>
                        <input type="file" id="fileInput" style="display: none;" />
                        <asp:FileUpload ID="fuLampiran" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload File" OnClick="btnUpload_Click" CssClass="btn btn-primary" />
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="form-group d-flex justify-content-between align-items-center w-100">
                <a type="button" class="btn btn-danger custom-btn" href="ViewWorkOrder.aspx">Cancel WO</a>
                <asp:Button ID="btnSimpan" runat="server" CssClass="btn btn-primary custom-btn" Text="Simpan" OnClientClick="return validateForm();" OnClick="btnSave_Click" />

            </div>
        </section>
    </div>

    <script>
        function logSelectedMoldTool() {
            var ddl = document.getElementById("ddlMoldTool");
            var selectedValue = ddl.value; // Ambil ID yang dipilih
            var selectedText = ddl.options[ddl.selectedIndex].text; // Ambil nama mold/tool
            console.log("Selected Mold/Tool ID: " + selectedValue);
            console.log("Selected Mold/Tool Name: " + selectedText);
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
    <%--<script src="bower_components/select2/dist/js/select2.full.min.js"></script>--%>

</asp:Content>