<%@ Page Language="VB" MasterPageFile="~/MasterPageNew.master" AutoEventWireup="false" CodeFile="Home.aspx.vb" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css"/>
    <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css"/>
    <link rel="stylesheet" href="bower_components/select2/dist/css/select2.min.css"/>
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css"/>
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1 style="font-family:Rubik-Regular;">
                <b>Dashboard</b>
                <small>v2</small>
            </h1>
            <ol class="breadcrumb">
                <li class="active"><a href="#"><i class="fa fa-home"></i> Dashboard</a></li>
            </ol>
        </section>
        
        <section class="content">
            <div class="row">                
       <%--          <div class="col-lg-3 col-xs-6"  id="div1" runat="server">
                    <!-- small box -->
                    <a href="Default.aspx" style="color:White">
                    <div class="small-box bg-primary">
                    <div class="inner">
                        <h3 style="color:white;font-family:Rubik-Light;"><asp:Label ID="setting" runat="Server" Text="Component"/></h3>

                        <p style="color:white; font-size:smaller";>WHS Component</p>
                           <div class="icon" style="padding-top:15px;">
                                <i class="glyphicon glyphicon-search" style="color:white"></i>
                         </div>
                           <a href="Default.aspx" class="small-box-footer"></a>
                    </div> 
                    </div>
                    </a>
                </div>--%>

                <div class="col-lg-3 col-xs-12" id="divsetting" runat="server" >
                    <a href="Master_UserP.aspx">
                        <div class="small-box" style="background-color:#656565;">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;">Setting</h3>
                                <p style="color:white;">Setting Data</p>
                            </div>
                        
                            <div class="icon" style="padding-top:15px;">
                                <i class="ion ion-android-settings"></i>
                            </div>
                        </div>
                    </a>
                </div>

                <div class="col-lg-3 col-xs-12" id="divvendor" runat="server" >
                    <a href="Master_UseV.aspx">
                        <div class="small-box bg-red">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;">Vendor</h3>
                                <p style="color:white;">Vendor</p>
                            </div>
                        
                            <div class="icon" style="padding-top:15px;">
                                <i class="ion ion-briefcase"></i>
                            </div>
                        </div>
                    </a>
                </div>

                <div class="col-lg-3 col-xs-12" id="divprofile" runat="server" >
                    <a href="Profile.aspx">
                        <div class="small-box bg-primary">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;">Profile</h3>
                                <p style="color:white;">Profile</p>
                            </div>
                        
                            <div class="icon" style="padding-top:15px;">
                                <i class="ion ion-android-people"></i>
                            </div>
                        </div>
                    </a>
                </div>

                <div class="col-lg-3 col-xs-12" id="divasset" runat="server">
                    <asp:HyperLink ID="amonitoring" runat="server">
                        <div class="small-box bg-aqua">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;font-weight:bold;">Monitoring</h3>
                                <p style="color:white;">Daily Delivery Monitoring</p>

                                <asp:HyperLink class="small-box-footer" ID="asmonitoring" runat="server"></asp:HyperLink>
                            </div>

                            <div class="icon" style="padding-top:15px;">
                                <i class="ion ion-monitor"></i>
                            </div>
                      </div>
                    </asp:HyperLink>
                </div>

                <div class="col-lg-3 col-xs-12" id="divpm" runat="server" >
                    <a href="VQM.aspx">
                        <div class="small-box bg-purple">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;">MVP</h3>
                                <p style="color:white;">Monthly Vendor Performance</p>
                            </div>
                        
                            <div class="icon" style="padding-top:15px;">
                                <i class="fa fa-line-chart"></i>
                            </div>
                        </div>
                    </a>
                </div>

                <div class="col-lg-3 col-xs-12" id="divmvp_quality" runat="server" visible="false">
                    <a href="VQM_Quality.aspx">
                        <div class="small-box bg-purple-active">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;">MVP</h3>
                                <p style="color:white;">Resume MVP Quality</p>
                            </div>
                        
                            <div class="icon" style="padding-top:15px;">
                                <i class="ion ion-android-star"></i>
                            </div>
                        </div>
                    </a>
                </div>

                <div class="col-lg-3 col-xs-12" id="divpm1" runat="server">
                    <asp:HyperLink ID="linkdok2" runat="server">
                        <div class="small-box bg-maroon">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;font-weight:bold;">Document</h3>
                                <p style="color:white;">Procedure, SE, SM, PPAP, etc.</p>

                                <asp:HyperLink class="small-box-footer" ID="linkdok" runat="server"></asp:HyperLink>
                            </div>

                            <div class="icon" style="padding-top:15px;">
                                <i class="ion ion-android-document"></i>
                            </div>
                      </div>
                    </asp:HyperLink>
                </div>

                <div class="col-lg-3 col-xs-12" id="dvgps" runat="server">
                    <asp:HyperLink ID="adoc1" runat="server">
                        <div class="small-box bg-yellow">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;font-weight:bold;">Invoices</h3>
                                <p style="color:white;">Document</p>

                                <asp:HyperLink class="small-box-footer" ID="adoc" runat="server"></asp:HyperLink>
                            </div>

                            <div class="icon" style="padding-top:15px;">
                                <i class="ion ion-android-list"></i>
                            </div>
                      </div>
                    </asp:HyperLink>
                </div>

                <div class="col-lg-3 col-xs-12" id="divapp" runat="server" >
                    <a href="App_Doc.aspx">
                        <div class="small-box bg-green">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;">Approval</h3>
                                <p style="color:white;">Approval Document</p>
                            </div>
                        
                            <div class="icon" style="padding-top:15px;">
                                <i class="ion ion-checkmark"></i>
                            </div>
                        </div>
                    </a>
                </div>

                <div class="col-lg-3 col-xs-12" id="div1" runat="server" >
                    <a href="ViewWorkOrder.aspx">
                        <div class="small-box bg-red">
                            <div class="inner">
                                <h3 style="color:white;font-family:Rubik-Regular;">MWS</h3>
                                <p style="color:white;">Mold an Toold</p>
                            </div>
                        
                            <div class="icon" style="padding-top:15px;">
                                <i class="ion ion-checkmark"></i>
                            </div>
                        </div>
                    </a>
                </div>

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
    </script>
    <!--script type="text/javascript">
    var url = window.location;
        // for sidebar menu entirely but not cover treeview
        $('ul.sidebar-menu a').filter(function() {
            return this.href != url;
        }).parent().removeClass('active');

        // for sidebar menu entirely but not cover treeview
        $('ul.sidebar-menu a').filter(function() {
            return this.href == url;
        }).parent().addClass('active');

        // for treeview
        $('ul.treeview-menu a').filter(function() {
            return this.href == url;
        }).parentsUntil(".sidebar-menu > .treeview-menu").addClass('active');
    </script!-->
</asp:Content>

