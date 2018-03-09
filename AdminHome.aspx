<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminHome.aspx.cs" Inherits="AdminHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Home</title>
    <link href="css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <div class="container">
            <div class="row">
                <div class="col-3">
                    <asp:Label ID="lbWelcome" runat="server" Text="Welcome "></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="btnLogout" runat="server" OnClick="btnLogout_Click" Text="Logout" />
                </div>
                <div class="col-6">
                    Update process<br />
                    <br />
                    <asp:Button ID="btnAddLeagues" runat="server" OnClick="btnAddLeagues_Click" Text="Add Leagues" />
                    <br />
                    <br />
                    <asp:ListBox ID="lbLeagueProgress" runat="server" Width="495px"></asp:ListBox>
                    <br />
                    <br />
                    <asp:Button ID="btnAddTeams" runat="server" OnClick="btnAddTeams_Click" Text="Add Teams, Fixtures &amp; Standings" />
                    <br />
                    <br />
                    <asp:ListBox ID="lbTeamStatus" runat="server" Width="493px"></asp:ListBox>
                    <br />
                    <br />
                    <asp:ListBox ID="lbFixtureStatus" runat="server" Width="490px"></asp:ListBox>
                    <br />
                    <br />
                    <asp:ListBox ID="lbStandingsStatus" runat="server" Width="490px"></asp:ListBox>
                    <br />
                    <br />
                    <asp:Label ID="lblHelp" runat="server" Text="Label"></asp:Label>
                    <br />
                    <br />
                </div>
                <div class ="col-3">column 3<br />
                    <asp:Label ID="lblLeagueProgress" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblTeamProgress" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblFixtureProgress" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblStandingsProgress" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
                    <br />
                    <br />
                    <asp:ListBox ID="lbDeleteFrom" runat="server" Width="295px"></asp:ListBox>
                    <br />
                </div>
            </div>
        </div>
    
    </div>
        <p>
            &nbsp;</p>
    </form>
</body>
</html>
