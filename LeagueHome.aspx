<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeagueHome.aspx.cs" Inherits="LeagueHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>League</title>
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

        <div class="row">
            <div class="container">
              <blockquote class="blockquote text-center">
                <h1><asp:Label ID="lblLeagueName" runat="server"></asp:Label></h1>
              </blockquote>    
            </div>
        </div>

        <div class ="container-fluid">
            <div class="row">
                <div class="col-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col"><asp:Label ID="lblCurrentMatchday" runat="server" Text=""></asp:Label></div>
                            </div>
                            <div class="row">
                                <div class="col"><asp:Label ID="lblTotalMatchdays" runat="server" Text=""></asp:Label></div>
                            </div>
                            <div class="row">
                                <div class="col"><asp:Label ID="lblTotalTeams" runat="server" Text=""></asp:Label></div>
                            </div>
                            <div class="row">
                                <div class="col"><asp:Label ID="lblTotalGames" runat="server" Text=""></asp:Label></div>
                            </div>
                        </div>
                    </div>
                </div>        
                <div class="col-6">
                    <div class="card">
                        <div class="card-body">

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="container-fluid">
            <div class="col">

                <div id="accordion">
                  <div class="card">
                    <div class="card-header" id="headingOne">
                      <h5 class="mb-0">
                          <asp:Label ID="lblTeamsHeader" runat="server" Text="Teams"></asp:Label>
                      </h5>
                    </div>

                    <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordion">
                      <div class="card-body">

                        <asp:Repeater ID="printTeams" runat="server">
                            <ItemTemplate>
                                <a href="<%# String.Format("TeamHome.aspx?leagueID={0}&teamName={1}", Eval("leagueId"), Eval("teamName")) %>">
                                    <div class="row">
                                        <h5>
                                            <td runat="server"><%# Eval("teamName") %></td>
                                        </h5>
                                    </div>
                                </a>
                            </ItemTemplate>
                        </asp:Repeater>

                      </div>
                    </div>
                  </div>
                  <div class="card">
                    <div class="card-header" id="headingTwo">
                      <h5 class="mb-0">
                          &nbsp;<asp:Label ID="lblLeagueTableHeader" runat="server" Text="League Table"></asp:Label>
                      </h5>
                    </div>
                    <div id="collapseTwo" class="collapse show" aria-labelledby="headingTwo" data-parent="#accordion">
                      <div class="card-body">
                          <asp:GridView ID="gvLeagueTable" runat="server" CssClass="table table-hover table-striped" GridLines="None">

                          </asp:GridView>
                      </div>
                    </div>
                  </div>
                </div>

            </div>
        </div>

    </div>


    </form>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
</body>
</html>
