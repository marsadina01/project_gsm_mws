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

        .chart-section {
          margin: 0 auto 60px auto;
        }

        .chart-section h4 {
          text-align: center;
          font-weight: 600;
          margin-bottom: 10px;
        }

        .chart-dropdown-wrapper {
          display: flex;
          justify-content: flex-end;
          margin-bottom: 15px;
        }

        .chart-dropdown-wrapper label {
          margin-right: 8px;
          font-weight: 500;
          align-self: center;
        }

        .chart-dropdown-wrapper select,
        .chart-dropdown-wrapper .form-control {
          width: 120px;
        }

        .card {
            background-color: #ffffff;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
            padding: 20px;
            width: 100%;
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }

        .card-body {
            padding: 20px;
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
                                    <h3 style="color:white;font-family:Rubik-Regular;"><asp:Label ID="lblNeedResponse" runat="server" Text="0" /></h3>
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
                                    <h3 style="color:white;font-family:Rubik-Regular;"><asp:Label ID="lblOnProgress" runat="server" Text="0" /></h3>
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
                                    <h3 style="color:white;font-family:Rubik-Regular;"><asp:Label ID="lblWaitingApproval" runat="server" Text="0" /></h3>
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
                                    <h3 style="color:white;font-family:Rubik-Regular;"><asp:Label ID="lblRejected" runat="server" Text="0" /></h3>
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
                                    <h3 style="color:white;font-family:Rubik-Regular;"><asp:Label ID="lblDone" runat="server" Text="0" /></h3>
                                    <p style="color:white;">Done</p>

                                    <div class="icon" style="padding-top:15px;">
                                        <i class="ion ion-android-checkmark-circle"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Chart Section -->
                    <asp:ScriptManager ID="ScriptManager1" runat="server" />

                    <!-- Chart Section 1 -->
                    <asp:UpdatePanel ID="UpdatePanelChart1" runat="server">
                        <ContentTemplate>
                            <div class="chart-section row mt-4" style="margin-top: 20px;">
                                <div class="card">
                                    <div class="card-header d-flex justify-content-between align-items-center">
                                        <h4 class="mb-0">Grafik Perbandingan Jumlah Work Order Request</h4>
                                        <div class="chart-dropdown-wrapper d-flex align-items-center">
                                            <label for="ddlTahunChart1" class="me-2 mb-0">Tahun:</label>
                                            <asp:DropDownList ID="ddlTahunChart1" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTahunChart1_SelectedIndexChanged"
                                                CssClass="form-control select2" />
                                        </div>
                                    </div>

                                    <div class="card-body">
                                        <asp:Literal ID="grafikDataJSON" runat="server" Visible="false" />
                                        <canvas id="grafikChart1" style="width: 100%; height: 500px;"></canvas>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <!-- Chart Section 2 -->
                    <asp:UpdatePanel ID="UpdatePanelChart2" runat="server">
                        <ContentTemplate>
                            <div class="chart-section row mt-4" style="margin-top: 20px;">
                                <div class="card">
                                    <div class="card-header d-flex justify-content-between align-items-center">
                                        <h4 class="mb-0">Grafik Jumlah Kerusakkan Mold dan Tool</h4>
                                        <div class="chart-dropdown-wrapper d-flex align-items-center">
                                            <label for="ddlTahunChart2" class="me-2 mb-0">Tahun:</label>
                                            <asp:DropDownList ID="ddlTahunChart2" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTahunChart2_SelectedIndexChanged"
                                                CssClass="form-control select2" />
                                        </div>
                                    </div>

                                    <div class="card-body">
                                        <asp:Literal ID="Literal1" runat="server" Visible="false" />
                                        <canvas id="grafikChart2" style="width: 100%; height: 500px;"></canvas>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                  </div>
                </div>

                      </div>

          </div>
        </section>

    </div>

    <!-- Script Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            var chart1Data = <%= grafikDataJSON.Text %>;

        var ctx1 = document.getElementById('grafikChart1').getContext('2d');
        new Chart(ctx1, {
            type: 'bar',
            data: {
                labels: chart1Data.labels,
                datasets: [
                    {
                        label: 'Progress',
                        backgroundColor: 'rgba(255, 205, 86, 0.7)',
                        data: chart1Data.progress
                    },
                    {
                        label: 'Done',
                        backgroundColor: 'rgba(75, 192, 192, 0.7)',
                        data: chart1Data.done
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { position: 'top' }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: { stepSize: 1 }
                    }
                }
            }
        });
    });
    </script>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<script>
    document.addEventListener("DOMContentLoaded", function () {
        var jsonData = JSON.parse('<%= Literal1.Text %>');
        var ctx = document.getElementById('grafikChart2').getContext('2d');
        var chart = new Chart(ctx, {
            type: 'bar',
            data: jsonData,
            options: {
                responsive: true,
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
    });
</script>



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
   
</asp:Content>