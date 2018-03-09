<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MatchPredictor.aspx.cs" Inherits="MatchPredictor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Match Predictor</title>
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
                <a class="nav-link" href="MatchPredictor.aspx">Predictor<span class="sr-only">(current)</span></a>
                </li>
                <li class="nav-item">
                <a class="nav-link" href="#">Change Details</a>
              </li>
            </ul>
          </div>
            <asp:Button ID="btnLogout" runat="server" OnClick="btnLogout_Click" Text="Logout" />
        </nav>
        <!--UPTO HERE!!-->

        <br />
        <div class="container-fluid">
            <div class="card">
                <br />
                <div class="row">
                    <div class="col-2">
                        <asp:Label ID="Label1" runat="server" Text="Select League:"></asp:Label>
                    </div>
                    <div class="col-4">
                        <asp:DropDownList ID="ddlLeagues" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="2">
                        <asp:Button ID="btnSelectLeague" runat="server" Text="Select League" OnClick="btnSelectLeague_Click" />
                    </div>
                </div>
                <br />
            </div>
        </div>
        <br />
        <div class="container-fluid">
            <div class="col">
                <div class="accordion">
                  <div class="card">
                    <div class="card-header" id="headingTwo">
                      <h5 class="mb-0">
                          &nbsp;<asp:Label ID="lblMatches" runat="server" Text=""></asp:Label>
                      </h5>
                    </div>
                    <div id="collapseTwo" class="collapse show" aria-labelledby="headingTwo" data-parent="#accordion">
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
    </form>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
</body>
</html>
