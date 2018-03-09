<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeamHome.aspx.cs" Inherits="TeamHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Team</title>
    <link href="css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <!--NEED ALL THIS FOR EACH PAGE-->
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
          <a class="navbar-brand" href="StandardHome.aspx">Home</a>
          <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>
          <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <ul class="navbar-nav mr-auto">
              <li class="nav-item">
                <a class="nav-link" href="QueryPage.aspx">Search</a>
              </li>
                <li class="nav-item">
                <a class="nav-link" href="MatchPredictor.aspx">Predictor</a>
                </li>
                <li class="nav-item">
                <a class="nav-link" href="#">Change Details</a>
              </li>
            </ul>
          </div>
            <asp:Button ID="btnLogout" runat="server" OnClick="btnLogout_Click" Text="Logout" />
        </nav>
        <!--UPTO HERE!!-->

        <div class="container-fluid">
            <div class="col">
                    <blockquote class="blockquote text-center">    
                        <h1><asp:Label ID="lblteamName" runat="server"></asp:Label></h1>
                    </blockquote>
            </div>
            <br />
            <div class="col">
                <div class="accordion">
                    <div class="card">
                        <div class="card-header" id="headingTwo">
                            <h5 class="mb-0">
                                &nbsp;<asp:Label ID="lblStatsHeader" runat="server" Text="Statistics"></asp:Label>
                            </h5>
                        </div>
                        <div id="collapseTwo" class="collapse show" aria-labelledby="headingTwo" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-5">
                                        <asp:Label ID="lblTotalGoals" runat="server" Text=""></asp:Label>
                                        <br />
                                        <asp:Label ID="lblTotalConceded" runat="server" Text=""></asp:Label>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblAvgScored" runat="server" Text=""></asp:Label>
                                        <br />
                                        <asp:Label ID="lblAvgConceded" runat="server" Text=""></asp:Label>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblAvgHomeScored" runat="server" Text=""></asp:Label>
                                        <br />
                                        <asp:Label ID="lblAvgHomeConceded" runat="server" Text=""></asp:Label>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblAvgAwayScored" runat="server" Text=""></asp:Label>
                                        <br />
                                        <asp:Label ID="lblAvgAwayCondeded" runat="server" Text=""></asp:Label>
                                        <br />
                                    </div>
                                    <div class="col-5">
                                        <img style='height: 75%; width: 75%; object-fit:contain' src="http://upload.wikimedia.org/wikipedia/de/d/da/Manchester_United_FC.svg" class="img-responsive center-block">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br /> 
                    <div class="card">
                        <div class="card-header" id="heading3">
                            <h5 class="mb-0">
                                &nbsp;<asp:Label ID="lblResults" runat="server" Text="Results"></asp:Label>
                            </h5>
                        </div>
                        <div id="collapse3" class="collapse show" aria-labelledby="heading3" data-parent="#accordion">
                            <div class="card-body">
                              <asp:GridView ID="gvResultsTable" runat="server" CssClass="table table-hover table-striped" GridLines="None">
                              </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <br /> 
                    <div class="card">
                        <div class="card-header" id="heading4">
                            <h5 class="mb-0">
                                &nbsp;<asp:Label ID="lblUpcomingFixtures" runat="server" Text="Upcoming Fixtures"></asp:Label>
                            </h5>
                        </div>
                        <div id="collapse4" class="collapse show" aria-labelledby="heading4" data-parent="#accordion">
                            <div class="card-body">
                              <asp:GridView ID="gvUpcomingFixtures" runat="server" CssClass="table table-hover table-striped" GridLines="None">
                              </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        




    
    </div>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>

    </form>
    
</body>
</html>
