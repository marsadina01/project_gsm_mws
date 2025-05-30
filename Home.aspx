<%@ Page Language="VB" MasterPageFile="~/MasterPageNew.master" AutoEventWireup="false" CodeFile="Home.aspx.vb" Inherits="Home" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css"/>
    <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css"/>
    <link rel="stylesheet" href="bower_components/select2/dist/css/select2.min.css"/>
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css"/>
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css"/>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        @media (min-width: 992px) {
            .col-lg-2-4 {
                width: 20%;
                float: left;
                padding: 0 15px;
                box-sizing: border-box;
            }
        }
    </style>
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
          <div class="container-fluid">
            <!-- Tabs -->
            <ul class="nav nav-tabs" id="dashboardTab" role="tablist">
              <li class="nav-item">
                <a class="nav-link active" id="tabMenu-tab" data-toggle="tab" href="#tabMenu" role="tab" aria-controls="tabMenu" aria-selected="true">Menu</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" id="tabGrafik-tab" data-toggle="tab" href="#tabGrafik" role="tab" aria-controls="tabGrafik" aria-selected="false">Grafik Request</a>
              </li>
            </ul>

            <!-- Tab Contents -->
            <div class="tab-content mt-3" id="dashboardTabContent">
              <!-- Menu Tab -->
              <div class="tab-pane fade show active" id="tabMenu" role="tabpanel" aria-labelledby="tabMenu-tab">
                <div class="row" id="menuButtonsContainer">
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
                                 <p style="color:white;">Mold an Tool</p>
                             </div>
                 
                             <div class="icon" style="padding-top:15px;">
                                 <i class="ion ion-checkmark"></i>
                             </div>
                         </div>
                     </a>
                 </div>

              </div>

              <!-- Grafik Request Tab -->
                    <div class="tab-pane fade" id="tabGrafik" role="tabpanel" aria-labelledby="tabGrafik-tab">
                                        <div class="row" id="grafikContainer">
                  <div class="col-xs-12">
                    <div class="row">
                        <div class="col-lg-2-4 col-xs-12" id="div2" runat="server">
                            <div class="small-box bg-red">
                                <div class="inner">
                                    <h3 style="color:white;font-family:Rubik-Regular;">100</h3>
                                    <p style="color:white;">Need Response</p>

                                    <div class="icon" style="padding-top:15px;">
                                       <i class="fa fa-bullhorn"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2-4 col-xs-12" id="div3" runat="server">
                            <div class="small-box bg-yellow">
                                <div class="inner">
                                    <h3 style="color:white;font-family:Rubik-Regular;">50</h3>
                                    <p style="color:white;">On Progress</p>

                                    <div class="icon" style="padding-top:15px;">
                                        <i class="fa fa-line-chart"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2-4 col-xs-12" id="div4" runat="server">
                            <div class="small-box bg-primary">
                                <div class="inner">
                                    <h3 style="color:white;font-family:Rubik-Regular;">175</h3>
                                    <p style="color:white;">Waiting Approval</p>

                                    <div class="icon" style="padding-top:15px;">
                                        <i class="fa fa-hourglass-2"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2-4 col-xs-12" id="div5" runat="server">
                            <div class="small-box bg-black">
                                <div class="inner">
                                    <h3 style="color:white;font-family:Rubik-Regular;">10</h3>
                                    <p style="color:white;">Rejected</p>

                                    <div class="icon" style="padding-top:15px; color: rgba(255, 255, 255, 0.3);">
                                        <i class="ion ion-android-cancel"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2-4 col-xs-12" id="div6" runat="server">
                            <div class="small-box bg-green">
                                <div class="inner">
                                    <h3 style="color:white;font-family:Rubik-Regular;">75</h3>
                                    <p style="color:white;">Done</p>

                                    <div class="icon" style="padding-top:15px;">
                                        <i class="ion ion-android-checkmark-circle"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                  </div>
                </div>


                      <div style="padding: 20px; max-width: 100vw; margin: 0 auto; background: #f8f9fa;">
                        <h4 class="text-center mb-4" style="font-weight: 600;">Grafik Dashboard Request</h4>                          
                        <!-- Wrapper kedua chart -->
                        <div style="display: flex; flex-direction: column; align-items: center; gap: 60px; padding: 20px;">

                          <!-- Chart 1 -->
                          <div class="card" style="background-color: white; box-shadow: 0 3px 10px rgba(0,0,0,0.15); border-radius: 12px; width: 100%; max-width: 1200px;">
                            <div class="card-body p-4">
                              <canvas id="grafikChart1" style="width: 100%; height: 500px;"></canvas>
                              <asp:Literal ID="grafikDataJSON" runat="server" Visible="false" />
                            </div>
                          </div>

                          <!-- Chart 2 -->
                            <div class="card" style="background-color: white; box-shadow: 0 3px 10px rgba(0,0,0,0.15); border-radius: 12px; width: 100%; max-width: 1200px;">
                              <div class="card-body p-4">
                                <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
                                <asp:UpdatePanel ID="UpdatePanelChart" runat="server">
                                  <ContentTemplate>
                                    <canvas id="grafikChart2" style="max-height:400px; width:100%;"></canvas>
                                    <asp:Literal ID="Literal1" runat="server" Visible="true" />
                                  </ContentTemplate>
<%--                                  <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTahun" EventName="SelectedIndexChanged" />
                                  </Triggers>--%>
                                </asp:UpdatePanel>

                              </div>
                            </div>
                        </div>
                      </div>
                      </div>

          </div>
        </section>

    </div>

    <!-- Script Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>


    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>

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

        $(document).ready(function () {
            $('#dashboardTab a[href="#tabMenu"]').tab('show');

            $('#dashboardTab a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var target = $(e.target).attr("href");

                if (target === '#tabGrafik') {
                    $('#menuButtonsContainer').hide();       
                    $('#grafikContainer').show();            
                } else if (target === '#tabMenu') {
                    $('#menuButtonsContainer').show();       
                    $('#grafikContainer').hide();            
                }
            });

            // Inisialisasi: sembunyikan grafik saat pertama kali halaman dibuka
            $('#grafikContainer').hide();
        });
    </script>

    <!-- Script Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <!-- Script untuk inisialisasi grafik -->
    <script>
        // Contoh data dummy, biasanya dari server (ASP.NET binding)
        var data = {
            labels: ['01 Jan', '02 Jan', '03 Jan'],
            values: [5, 10, 7]
        };

        var ctx = document.getElementById('grafikChart1').getContext('2d');
        var chart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: data.labels,
                datasets: [{
                    label: 'Jumlah Request',
                    data: data.values,
                    backgroundColor: 'rgba(75, 192, 192, 0.5)'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false
            }
        });
    </script>
   
</asp:Content>