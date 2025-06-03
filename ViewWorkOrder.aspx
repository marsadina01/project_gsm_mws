<%@ Page Language="VB" MasterPageFile="~/MasterPageNew.master" AutoEventWireup="false" CodeFile="ViewWorkOrder.aspx.vb" Inherits="ViewWorkOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css"/>
    <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css"/>
    <link rel="stylesheet" href="bower_components/select2/dist/css/select2.min.css"/>
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css"/>
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css"/>

    <!-- DataTables CSS -->
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css"/>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>
    
    <!-- SweetAlert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            $('#myTable').DataTable({
                "scrollX": true,
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": true,
                "autoWidth": false
            });

            // Aktifkan Select2
            $('.select2').select2();
        });


    </script>

    <style>

        .select2-container .select2-selection--single {
            height: 33px !important;
            padding: 5px;
            border-radius: 4px;
        }

        .table-responsive {
            overflow-x: auto;
            white-space: nowrap;
        }

        .btn-reset {
            display: inline-block;
            padding: 8px 16px;
            font-size: 16px;
            font-weight: 600;
            color: #0d6efd; /* Warna biru sesuai Bootstrap primary */
            background-color: transparent;
            border: 2px solid #0d6efd;
            border-radius: 5px;
            cursor: pointer;
            transition: all 0.3s ease-in-out;
        }

        .btn-reset:hover {
            background-color: #0d6efd;
            color: #fff;
        }

        .btn-reset:active {
            background-color: #0b5ed7;
            border-color: #0a58ca;
            color: #fff;
        }

        .btn-search {
            display: inline-block;
            padding: 8px 16px;
            font-size: 16px;
            font-weight: 600;
            color: #fff;
            background-color: #0d6efd; /* Warna primary Bootstrap */
            border: 2px solid #0d6efd;
            border-radius: 5px;
            cursor: pointer;
            transition: all 0.3s ease-in-out;
        }

        .btn-search:hover {
            background-color: #0b5ed7;
            border-color: #0a58ca;
        }

        .btn-search:active {
            background-color: #0a58ca;
            border-color: #094db5;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1 style="font-family:Rubik-Regular;">
                <b>Work Order Request</b>
                <small>Transaction</small>
            </h1>
            <ol class="breadcrumb">
                <li><a href="#"><i class="fa fa-folder-open"></i> Dashboard</a></li>
                <li>My Request</li>
            </ol>
        </section>
        
        <section class="content">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header" align="left">
                            <div class="row">
                                <!-- Pilih Tipe Breakdown -->
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlBreakdown" runat="server" class="form-control select2">
                                            <asp:ListItem Text="-- Pilih Tipe Breakdown --" Value="0" Selected="True" Disabled="True"></asp:ListItem>
                                            <asp:ListItem Text="Mold" Value="BM"></asp:ListItem>
                                            <asp:ListItem Text="Tool" Value="BT"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <!-- Textbox Pencarian -->
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtReqDate" TextMode="Date" runat="server" class="form-control" title="Date Range"></asp:TextBox>
                                    </div>
                                </div>

                                <!-- Dropdown Status -->
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlStatus" runat="server" class="form-control select2">
                                            <asp:ListItem Text="-- Pilih Status --" Value="" Selected="True" Disabled="True"></asp:ListItem>
                                            <asp:ListItem Text="Waiting Approval" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Need Response" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="On Progress" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Waiting Approval by Kasie Technician" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Done" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="Rejected by Kasie Req" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="Rejected by Kasie Technician" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="Canceled" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <!-- Tombol Search & Reset -->
                                <div class="col-md-3">
                                    <div class="form-group d-flex align-items-center">
                                        <asp:LinkButton ID="btnSearch" runat="server" class="btn btn-search btn-lg" OnClick="OnSearchOrRefresh">
                                            <i class="fa fa-search"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnReset" runat="server" class="btn btn-reset btn-lg" OnClick="OnSearchOrRefresh">
                                            <i class="glyphicon glyphicon-refresh icon-black"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div id="btnAdd" runat="server">
                                <a type="button" class="btn btn-primary" href="AddWorkOrder.aspx">
                                    Add New
                                </a>
                            </div>
                            
                        </div>
                        
                        <div class="box-body">
                            <div class="table-responsive">
                                <asp:Repeater ID="rptWorkOrder" runat="server">
                                    <HeaderTemplate>
                                        <table id="myTable" class="display nowrap" style="width:100%">
                                            <thead>
                                                <tr class="center">
                                                    <th>No</th>
                                                    <th>Work Order</th>
                                                    <th>Supplier</th>
                                                    <th>Kerusakan</th>
                                                    <th>No Mold/Tool</th>
                                                    <th>Request Date</th>
                                                    <th>Response Date</th>
                                                    <th>Repair By</th>
                                                    <th>Finished Date</th>
                                                    <th>Status</th>
                                                    <th>Aksi</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:Label visible="false" ID="lblno" Text='<%# Eval("wor_no") %>' runat="server" />
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td><%# Eval("wor_no") %></td>
                                            <td><%# Eval("wor_supplier") %></td>
                                            <td><%# Eval("wor_damage") %></td>
                                            <td><%# Eval("wor_mold_tool") %></td>
                                            <td><%# Eval("wor_createdate", "{0:dd/MM/yyyy HH:mm}") %></td>
                                            <td><%# Eval("wor_responsedate", "{0:dd/MM/yyyy HH:mm}") %></td>
                                            <td><%# Eval("wor_repairby") %></td>
                                            <td><%# Eval("wor_finisheddate", "{0:dd/MM/yyyy HH:mm}") %></td>
                                            <td style='<%# GetStatusStyle(Eval("wor_status")) %>'>
                                                <%# GetStatusText(Eval("wor_status")) %>
                                            </td>
                                            <td>
                                                <%# GetActionButtons(Eval("wor_status"), Eval("wor_no")) %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>

                                                <script>
                                                    function respondToWorkOrder(worNo) {
                                                        Swal.fire({
                                                            title: "Apakah Anda ingin merespons WO ini?",
                                                            text: "no WO: " + worNo,
                                                            icon: "question",
                                                            showCancelButton: true,
                                                            confirmButtonColor: "#3085d6",
                                                            cancelButtonColor: "#d33",
                                                            confirmButtonText: "Ya, Lanjutkan",
                                                            cancelButtonText: "Batal"
                                                        }).then((result) => {
                                                            if (result.isConfirmed) {
                                                                $.ajax({
                                                                    type: "POST",
                                                                    url: "ViewWorkOrder.aspx/OnResponse",
                                                                    data: JSON.stringify({ worNo: worNo }),
                                                                    contentType: "application/json; charset=utf-8",
                                                                    dataType: "json",
                                                                    success: function (response) {
                                                                        Swal.fire("Sukses", response.d, "success").then(() => {
                                                                            location.reload();
                                                                        });
                                                                    },
                                                                    error: function (xhr, status, error) {
                                                                        Swal.fire("Error", "Terjadi kesalahan: " + xhr.responseText, "error");
                                                                    }
                                                                });
                                                            }
                                                        });
                                                    }

                                                    function cancelWorkOrder(worNo) {
                                                        Swal.fire({
                                                            title: "Apakah Anda ingin membatalkan WO ini?",
                                                            text: "no WO: " + worNo,
                                                            icon: "question",
                                                            showCancelButton: true,
                                                            confirmButtonColor: "#3085d6",
                                                            cancelButtonColor: "#d33",
                                                            confirmButtonText: "Ya, Lanjutkan",
                                                            cancelButtonText: "Batal"
                                                        }).then((result) => {
                                                            if (result.isConfirmed) {
                                                                $.ajax({
                                                                    type: "POST",
                                                                    url: "ViewWorkOrder.aspx/OnCancel",
                                                                    data: JSON.stringify({ worNo: worNo }),
                                                                    contentType: "application/json; charset=utf-8",
                                                                    dataType: "json",
                                                                    success: function (response) {
                                                                        Swal.fire("Sukses", response.d, "success").then(() => {
                                                                            location.reload();
                                                                        });
                                                                    },
                                                                    error: function (xhr, status, error) {
                                                                        Swal.fire("Error", "Terjadi kesalahan: " + xhr.responseText, "error");
                                                                    }
                                                                });
                                                            }
                                                        });
                                                    }

                                                </script>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                    </div>
                </div>
                <!-- ./col -->
            </div>
        </section>
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
