<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryPage.aspx.cs" Inherits="QueryPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search</title>
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
                <a class="nav-link" href="QueryPage.aspx">Search<span class="sr-only">(current)</span></a>
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
            <br />
            <div class="card">
                <br />
                <div class="row">
                    <div class="col-2">
                        <asp:Label ID="Label1" runat="server" Text="Select League:"></asp:Label>
                    </div>
                    <div class="col-4">
                        <asp:DropDownList AutoPostBack="true" ID="ddlLeagues" runat="server" onselectedindexchanged="ddlLeagues_SelectedIndexChanged">                    
                        </asp:DropDownList>
                    </div>
                    <div class="col-4">
                        <asp:Label ID="lblNotify" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
            </div>
            <br />
            <div class="card">
                <br />
                <div class="row">
                    <div class="col-4">
                        <asp:DropDownList ID="ddlTeams" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-2">
                        <asp:DropDownList ID="ddlGoalFilter" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-2">
                        <asp:DropDownList ID="ddlNoOfGoals" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-2">
                        <asp:Button ID="btnRunGoalsQuery" runat="server" Text="Run Query" OnClick="btnRunGoalsQuery_Click" />
                    </div>
                    <div class="col-2">
                        <asp:Label ID="lblGoalsQueryInfo" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col">
                        <asp:Button ID="btnShowTable" runat="server" Text="LeagueTable" OnClick="btnShowTable_Click" />
                        <asp:Button ID="btnShowHomeTable" runat="server" Text="Home Table" OnClick="btnShowHomeTable_Click" />
                        <asp:Button ID="btnShowAwayTable" runat="server" Text="Away Table" OnClick="btnShowAwayTable_Click" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col">

                        <asp:Button ID="btnBestAttack" runat="server" OnClick="btnBestAttack_Click" Text="Best Attack" />
                        <asp:Button ID="btnBestDefence" runat="server" OnClick="btnBestDefence_Click" Text="Best Defence" />

                    </div>
                </div>
                <br />
            </div>
            <div class="row">
            </div>
        </div>
        <br />
        <div class="container-fluid">
            <div class="col">
                <div class="accordion">
                  <div class="card">
                    <div class="card-header" id="headingOne">
                      <h5 class="mb-0">
                          <asp:Label ID="lblQueryHeader" runat="server" Text="Result"></asp:Label>
                      </h5>
                    </div>

                    <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordion">
                      <div class="card-body">
                          <asp:GridView ID="gvQueryResultsTable" runat="server" CssClass="table table-hover table-striped" GridLines="None">
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
