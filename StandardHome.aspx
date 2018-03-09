<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardHome.aspx.cs" Inherits="StandardHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home Page</title>
    <link href="css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <!--NEED ALL THIS FOR EACH PAGE-->
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
          <a class="navbar-brand" href="StandardHome.aspx">Home<span class="sr-only">(current)</span></a>
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
              <asp:Button ID="btnLogout" runat="server" OnClick="btnLogout_Click" Text="Logout" />
          </div>
        </nav>
        <!--UPTO HERE!!-->

        <br />
        <div class="container-fluid" >
        <asp:Repeater ID="printLeagues" runat="server">
            <ItemTemplate>
                <a href="<%# String.Format("LeagueHome.aspx?leagueID={0}", Eval("leagueId")) %>">
                    <div class="row">
                        <div class="col-3">
                        </div>
                        <div class="col-6">
                            <div class="accordion">
                                <div class="card">
                                    <div class="card-header" id="headingTwo">
                                      <h5 class="mb-0">
                                          &nbsp;<td runat="server"><%# Eval("leagueName") %></td>
                                      </h5>
                                    </div>
                                    <div id="collapseTwo" class="collapse show" aria-labelledby="headingTwo" data-parent="#accordion">
                                      <div class="card-body">
                                          <img style='height: 50%; width: 50%; object-fit: contain' src="<%# Eval("crestURL") %>">
                                      </div>
                                    </div>
                                  </div>
                            </div>
                            <br />
                        </div>
                        <div class="col-3"></div>
                    </div>
                </a>
            </ItemTemplate>
        </asp:Repeater>
            </div>

    </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
</body>
</html>
