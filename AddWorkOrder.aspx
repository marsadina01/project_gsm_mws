<%@ Page Language="VB" MasterPageFile="~/MasterPageNew.master" AutoEventWireup="false" CodeFile="AddWorkOrder.aspx.vb" Inherits="AddWorkOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css"/>
    <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css"/>
    <link rel="stylesheet" href="bower_components/select2/dist/css/select2.min.css"/>
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css"/>
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css"/>
     <!-- DataTables -->
    <link rel="stylesheet" href="bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css"/>

    <script type="text/javascript">
        $(document).ready(function () {
            var url = window.location;
            $('.sidebar .sidebar-menu').find('.active').removeClass('active');
            $('.sidebar .sidebar-menu li a').each(function () {
                if (this.href == url) {
                    $(this).parent().addClass('treeview active');
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
             <div class="row">
                 <div class="col-xs-12">
                     <div class="box">
                         <div class="box-body">
                            
                        </div>

                         <div class="modal fade" id="modal-default">
                           <div class="modal-dialog">
                               <div class="modal-content">
                                   <div class="modal-header">
                                       <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                       <span aria-hidden="true">&times;</span></button>
                                       <h4 class="modal-title">Add Mold / Tool</h4>
                                   </div>
                                   <div class="modal-body pad">
                                       <div class="form-group">
                                           <label>Nama Mold / Tool </label><label style="color: #FF0000"> *</label>
                                           <asp:textbox id="txtname" Runat="server" class="form-control" required="true"></asp:textbox>
                                       </div>
                                       <div class="form-group">
                                            <label>Tipe Mold / Tool </label><label style="color: #FF0000"> *</label>
                                            <asp:DropDownList id="ddlType" Runat="server" class="form-control" required="true">
                                                <asp:ListItem Text="-- Pilih Tipe --" Value="" Selected="True" Disabled="True"></asp:ListItem>
                                                <asp:ListItem Text="Mold" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Tool" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                       </div>
                                   </div>
                                   <div class="modal-footer">
                                       <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                                       <asp:button runat="server" ID="btnsave" Text="Save" class="btn btn-primary" OnClick="btnsave_Click" />
                                   </div>
                               </div>
                           </div>
                       </div>
                       </div>
           
                        
                     </div>
                 </div>
               <!-- ./col -->
             </div>
        </section>
    </div>
    <script src="bower_components/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="bower_components/fastclick/lib/fastclick.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="dist/js/demo.js"></script>
    <script type="text/javascript" src="bower_components/select2/dist/js/select2.full.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.select2').select2()

            $('#site').DataTable()
            $('#template').DataTable({
                'paging'      : true,
                'lengthChange': false,
                'searching'   : false,
                'ordering'    : true,
                'info'        : true,
                'autoWidth'   : false
            })
        })

        function OnEdit(sender) {
            var row = sender.closest('tr');
            document.getElementById(row.id + '_lblsite_nama').style.display = 'none';
            document.getElementById(row.id + '_txtsite_nama').style.display = 'inline';
            document.getElementById(row.id + '_lblsite_tipe').style.display = 'none';
            document.getElementById(row.id + '_ddlsite_tipe').style.display = 'inline';
            document.getElementById(row.id + '_lbubah').style.display = 'none';
            document.getElementById(row.id + '_lbsimpan').style.display = 'inline';
            document.getElementById(row.id + '_lbcancel').style.display = 'inline';
        }
    </script>
   
</asp:Content>

