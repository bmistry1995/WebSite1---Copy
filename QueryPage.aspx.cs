using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class QueryPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userSesh"] == null)
            Response.Redirect("Default.aspx");

        //populate the leagues drop down list
        //ddlLeagues.Items.Clear();
        List<String> leaguesList = new List<string>();
        leaguesList = getLeagues();

        if (leaguesList.Count == 0)
            lblNotify.Text = "Cant access leagues";
        else
        {
            foreach (string leagueName in leaguesList)
                ddlLeagues.Items.Add(new ListItem(leagueName));
        }

        //get current league Name selected in leagues drop down list
        //poopulate the teams drop down list
        //ddlTeams.Items.Clear();
        string selectedLeagueName = ddlLeagues.Text;
        List<string> teamsList = new List<string>();
        teamsList = getTeams(selectedLeagueName);
        if (teamsList.Count == 0)
            lblGoalsQueryInfo.Text = "No teams available";
        else
            foreach (string teamName in teamsList)
                ddlTeams.Items.Add(new ListItem(teamName));

        //populate the goals filter drop down list
        //ddlGoalFilter.Items.Clear();
        ddlGoalFilter.Items.Add("More than");
        ddlGoalFilter.Items.Add("More than or equal to");
        ddlGoalFilter.Items.Add("Equal to");
        ddlGoalFilter.Items.Add("Less than");
        ddlGoalFilter.Items.Add("Less than or equal to");

        //ddlNoOfGoals.Items.Clear();
        for (int i = 0; i < 10; i++)
        {
            ddlNoOfGoals.Items.Add(i.ToString());
        }
    }

    public List<String> getLeagues()
    {
        string selectLeaguesConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectLeaguesCommString = "SELECT * FROM tblLeagues";

        List<String> returnLeaguesList = new List<string>();

        try
        {
            using (SqlConnection selectLeaguesConn = new SqlConnection(selectLeaguesConnString))
            {
                using (SqlCommand selectLeaguesComm = new SqlCommand(selectLeaguesCommString, selectLeaguesConn))
                {
                    selectLeaguesConn.Open();
                    using (SqlDataReader selectLeaguesReader = selectLeaguesComm.ExecuteReader())
                    {
                        int indexOfID = selectLeaguesReader.GetOrdinal("id");
                        int indexOfLeagueName = selectLeaguesReader.GetOrdinal("leagueName");

                        while (selectLeaguesReader.Read())
                        {
                            int leagueId = Convert.ToInt32(selectLeaguesReader.GetValue(indexOfID));
                            string leagueName = selectLeaguesReader.GetValue(indexOfLeagueName).ToString();
                            returnLeaguesList.Add(leagueName);
                        }//end while
                    }//end reader
                }//end using
            }//end using
            return returnLeaguesList;
        }//end try
        catch
        {
            //return empty list
            return returnLeaguesList;
        }//end catch
    }

    public List<string> getTeams(string name)
    {
        string selectTeamsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectTeamsCommString = "SELECT * from tblTeams JOIN tblLeagues ON tblTeams.leagueID = tblLeagues.id WHERE tblLeagues.leagueName = '" + name + "' ORDER BY teamName";

        List<string> returnTeamList = new List<string>();

        try
        {
            using (SqlConnection selectTeamsConn = new SqlConnection(selectTeamsConnString))
            {
                using (SqlCommand selectTeamsComm = new SqlCommand(selectTeamsCommString, selectTeamsConn))
                {
                    selectTeamsConn.Open();
                    using (SqlDataReader selectTeamsReader = selectTeamsComm.ExecuteReader())
                    {
                        int indexOfteamName = selectTeamsReader.GetOrdinal("teamName");

                        while (selectTeamsReader.Read())
                        {
                            string teamName = selectTeamsReader.GetValue(indexOfteamName).ToString();
                            returnTeamList.Add(teamName);
                        }//end while
                    }//end reader
                }//end using
            }//end using
            return returnTeamList;
        }//end try
        catch
        {
            //return empty list
            return returnTeamList;
        }//end catch
    }

    protected void ddlLeagues_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblQueryHeader.Text = "Result";
        ddlTeams.Items.Clear();
        string selectedLeagueName = ddlLeagues.Text;
        List<string> teamsList = new List<string>();
        teamsList = getTeams(selectedLeagueName);
        if (teamsList.Count == 0)
            lblGoalsQueryInfo.Text = "No teams available";
        else
            foreach (string teamName in teamsList)
            {
                ddlTeams.Items.Add(new ListItem(teamName));
                lblGoalsQueryInfo.Text = "";
            }
        gvQueryResultsTable.DataSource = null;
        gvQueryResultsTable.DataBind();
    }

    protected void btnRunGoalsQuery_Click(object sender, EventArgs e)
    {
        string getTeamName = ddlTeams.Text;
        string goalFilterText = ddlGoalFilter.Text;
        int getGoals = Convert.ToInt32(ddlNoOfGoals.Text);
        string convertedGoalFilter = "";

        switch (goalFilterText)
        {
            case "More than":
                convertedGoalFilter = ">";
                break;
            case "Less than":
                convertedGoalFilter = "<";
                break;
            case "Equal to":
                convertedGoalFilter = "=";
                break;
            case "More than or equal to":
                convertedGoalFilter = ">=";
                break;
            case "Less than or equal to":
                convertedGoalFilter = "<=";
                break;
        }


        getFixturesQuery(getTeamName, convertedGoalFilter, getGoals, goalFilterText);
    }

    public void getFixturesQuery(string teamName, string goalFilter, int goals, string goalFilterText)
    {
        string selectGamesConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectGamesCommString = "select * from tblFixtures  where ((homeTeam = '" + teamName + "' and homeGoalsFT " + goalFilter +" " + goals +") or(awayTeam = '" + teamName + "' and awayGoalsFT " + goalFilter +" " + goals +")) and matchStatus = 'FINISHED' order by fixtureDate";
        List<FixtureObject> fixturesList = new List<FixtureObject>();

        using (SqlConnection selectGamesConn = new SqlConnection(selectGamesConnString))
        {
            using (SqlCommand selectGamesComm = new SqlCommand(selectGamesCommString, selectGamesConn))
            {
                selectGamesConn.Open();
                using (SqlDataReader selectGamesReader = selectGamesComm.ExecuteReader())
                {
                    if (selectGamesReader.HasRows)
                    {
                        lblQueryHeader.Text = teamName + " games " + goalFilterText + " " + goals;
                        int indexOfFixtureDate = selectGamesReader.GetOrdinal("fixtureDate");
                        int indexOfHomeTeam = selectGamesReader.GetOrdinal("homeTeam");
                        int indexoOfHomeGoalsFT = selectGamesReader.GetOrdinal("homeGoalsFT");
                        int indexOfAwayGoalsFT = selectGamesReader.GetOrdinal("awayGoalsFT");
                        int indexOfAwayTeam = selectGamesReader.GetOrdinal("awayTeam");

                        while (selectGamesReader.Read())
                        {
                            string fixtureDate, homeTeam, awayTeam;
                            int homeGoalsFT, awayGoalsFT;

                            fixtureDate = selectGamesReader.GetValue(indexOfFixtureDate).ToString();
                            homeTeam = selectGamesReader.GetValue(indexOfHomeTeam).ToString();
                            homeGoalsFT = Convert.ToInt32(selectGamesReader.GetValue(indexoOfHomeGoalsFT));
                            awayGoalsFT = Convert.ToInt32(selectGamesReader.GetValue(indexOfAwayGoalsFT));
                            awayTeam = selectGamesReader.GetValue(indexOfAwayTeam).ToString();


                            fixturesList.Add(new FixtureObject(fixtureDate, homeTeam, homeGoalsFT, awayGoalsFT, awayTeam));
                        }
                    }
                    else
                    {
                        lblQueryHeader.Text = "No fixtures match your search";
                    }
                }//end reader
            }//end using
        }//end using

        gvQueryResultsTable.DataSource = fixturesList;
        gvQueryResultsTable.DataBind();
    }

    protected void btnShowTable_Click(object sender, EventArgs e)
    {
        string leagueName = ddlLeagues.Text;
        getLeagueTable(leagueName);
    }
    public void getLeagueTable(string leagueName)
    {

        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "SELECT * FROM tblLeagueTable WHERE leagueCaption = '" + leagueName + "' ORDER BY leagueID ASC";
        List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();

        using (SqlConnection selectStandingsConn = new SqlConnection(selectStandingsConnString))
        {
            using (SqlCommand selectStandingsComm = new SqlCommand(selectStandingsCommString, selectStandingsConn))
            {
                selectStandingsConn.Open();
                using (SqlDataReader selectStandingsReader = selectStandingsComm.ExecuteReader())
                {
                    if (selectStandingsReader.HasRows)
                    {
                        lblQueryHeader.Text = "Overall League Table";
                        int indexOfPosition = selectStandingsReader.GetOrdinal("position");
                        int indexOfTeamName = selectStandingsReader.GetOrdinal("teamName");
                        int indexOfgamesPlayed = selectStandingsReader.GetOrdinal("gamesPlayed");
                        int indexOfWins = selectStandingsReader.GetOrdinal("win");
                        int indexOfDraws = selectStandingsReader.GetOrdinal("draw");
                        int indexOfLosses = selectStandingsReader.GetOrdinal("loss");
                        int indexOfGF = selectStandingsReader.GetOrdinal("GF");
                        int indexOfGA = selectStandingsReader.GetOrdinal("GA");
                        int indexOfGD = selectStandingsReader.GetOrdinal("GD");
                        int indexOfPoints = selectStandingsReader.GetOrdinal("points");

                        while (selectStandingsReader.Read())
                        {
                            int position, gamesPlayed, wins, draws, losses, GF, GA, GD, points;

                            position = Convert.ToInt32(selectStandingsReader.GetValue(indexOfPosition));
                            string teamName = selectStandingsReader.GetValue(indexOfTeamName).ToString();
                            gamesPlayed = Convert.ToInt32(selectStandingsReader.GetValue(indexOfgamesPlayed));
                            wins = Convert.ToInt32(selectStandingsReader.GetValue(indexOfWins));
                            draws = Convert.ToInt32(selectStandingsReader.GetValue(indexOfDraws));
                            losses = Convert.ToInt32(selectStandingsReader.GetValue(indexOfLosses));
                            GF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfGF));
                            GA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfGA));
                            GD = Convert.ToInt32(selectStandingsReader.GetValue(indexOfGD));
                            points = Convert.ToInt32(selectStandingsReader.GetValue(indexOfPoints));
                            leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }
                    }
                    else
                    {
                        lblQueryHeader.Text = "No full league table available";
                    }
                }//end reader
            }//end using
        }//end using

        gvQueryResultsTable.DataSource = leagueTableList;
        gvQueryResultsTable.DataBind();
    }

    protected void btnShowHomeTable_Click(object sender, EventArgs e)
    {
        string leagueName = ddlLeagues.Text;
        getHomeLeagueTable(leagueName);
    }

    public void getHomeLeagueTable(string leagueName)
    {

        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "SELECT * FROM tblLeagueTable WHERE leagueCaption = '" + leagueName + "' ORDER BY leagueID ASC";
        List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();

        using (SqlConnection selectStandingsConn = new SqlConnection(selectStandingsConnString))
        {
            using (SqlCommand selectStandingsComm = new SqlCommand(selectStandingsCommString, selectStandingsConn))
            {
                selectStandingsConn.Open();
                using (SqlDataReader selectStandingsReader = selectStandingsComm.ExecuteReader())
                {
                    if (selectStandingsReader.HasRows)
                    {
                        lblQueryHeader.Text = "Home League Table";
                        //int indexOfPosition = selectStandingsReader.GetOrdinal("position");
                        int indexOfTeamName = selectStandingsReader.GetOrdinal("teamName");
                        //int indexOfgamesPlayed = selectStandingsReader.GetOrdinal("gamesPlayed");
                        int indexOfHomeWins = selectStandingsReader.GetOrdinal("homeWin");
                        int indexOfHomeDraws = selectStandingsReader.GetOrdinal("homeDraw");
                        int indexOfHomeLosses = selectStandingsReader.GetOrdinal("homeLoss");
                        int indexOfHomeGF = selectStandingsReader.GetOrdinal("HomeGF");
                        int indexOfHomeGA = selectStandingsReader.GetOrdinal("HomeGA");
                        //int indexOfGD = selectStandingsReader.GetOrdinal("GD");
                        //int indexOfPoints = selectStandingsReader.GetOrdinal("points");
                        int position = 0;

                        while (selectStandingsReader.Read())
                        {
                            int gamesPlayed, wins, draws, losses, GF, GA, GD, points;

                            string teamName = selectStandingsReader.GetValue(indexOfTeamName).ToString();
                            
                            wins = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeWins));
                            draws = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeDraws));
                            losses = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeLosses));
                            gamesPlayed = wins + draws + losses;
                            GF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeGF));
                            GA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeGA));
                            GD = GF - GA;
                            points = (wins * 3) + (draws);
                            leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }

                    }
                    else
                    {
                        lblQueryHeader.Text = "No home league table available";
                    }
                }//end reader
            }//end using
        }//end using

        leagueTableList.Sort(delegate (LeagueStandingObject x, LeagueStandingObject y)
        {
            return y.points.CompareTo(x.points);
        });

        int newPosition = 0;
        for (int i = 0; i < leagueTableList.Count; i++)
        {
            newPosition++;
            leagueTableList[i].position = newPosition;
        }

        gvQueryResultsTable.DataSource = leagueTableList;
        gvQueryResultsTable.DataBind();
    }

    protected void btnShowAwayTable_Click(object sender, EventArgs e)
    {
        string leagueName = ddlLeagues.Text;
        getAwayLeagueTable(leagueName);
    }

    public void getAwayLeagueTable(string leagueName)
    {

        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "SELECT * FROM tblLeagueTable WHERE leagueCaption = '" + leagueName + "' ORDER BY leagueID ASC";
        List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();

        using (SqlConnection selectStandingsConn = new SqlConnection(selectStandingsConnString))
        {
            using (SqlCommand selectStandingsComm = new SqlCommand(selectStandingsCommString, selectStandingsConn))
            {
                selectStandingsConn.Open();
                using (SqlDataReader selectStandingsReader = selectStandingsComm.ExecuteReader())
                {
                    if (selectStandingsReader.HasRows)
                    {
                        lblQueryHeader.Text = "Away League Table";
                        //int indexOfPosition = selectStandingsReader.GetOrdinal("position");
                        int indexOfTeamName = selectStandingsReader.GetOrdinal("teamName");
                        //int indexOfgamesPlayed = selectStandingsReader.GetOrdinal("gamesPlayed");
                        int indexOfAwayWins = selectStandingsReader.GetOrdinal("awayWin");
                        int indexOfAwayDraws = selectStandingsReader.GetOrdinal("awayDraw");
                        int indexOfAwayLosses = selectStandingsReader.GetOrdinal("awayLoss");
                        int indexOfAwayGF = selectStandingsReader.GetOrdinal("awayGF");
                        int indexOfAwayGA = selectStandingsReader.GetOrdinal("awayGA");
                        //int indexOfGD = selectStandingsReader.GetOrdinal("GD");
                        //int indexOfPoints = selectStandingsReader.GetOrdinal("points");
                        int position = 0;

                        while (selectStandingsReader.Read())
                        {
                            int gamesPlayed, wins, draws, losses, GF, GA, GD, points;

                            string teamName = selectStandingsReader.GetValue(indexOfTeamName).ToString();

                            wins = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayWins));
                            draws = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayDraws));
                            losses = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayLosses));
                            gamesPlayed = wins + draws + losses;
                            GF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayGF));
                            GA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayGA));
                            GD = GF - GA;
                            points = (wins * 3) + (draws);
                            leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }

                    }
                    else
                    {
                        lblQueryHeader.Text = "No away league table available";
                    }
                }//end reader
            }//end using
        }//end using

        leagueTableList.Sort(delegate (LeagueStandingObject x, LeagueStandingObject y)
        {
            return y.points.CompareTo(x.points);
        });

        int newPosition = 0;
        for (int i = 0; i < leagueTableList.Count; i++)
        {
            newPosition++;
            leagueTableList[i].position = newPosition;
        }

        gvQueryResultsTable.DataSource = leagueTableList;
        gvQueryResultsTable.DataBind();
    }

    protected void btnBestAttack_Click(object sender, EventArgs e)
    {
        string leagueName = ddlLeagues.Text;
        getBestAttackLeagueTable(leagueName);
    }

    public void getBestAttackLeagueTable(string leagueName)
    {

        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "SELECT * FROM tblLeagueTable WHERE leagueCaption = '" + leagueName + "' ORDER BY leagueID ASC";
        List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();

        using (SqlConnection selectStandingsConn = new SqlConnection(selectStandingsConnString))
        {
            using (SqlCommand selectStandingsComm = new SqlCommand(selectStandingsCommString, selectStandingsConn))
            {
                selectStandingsConn.Open();
                using (SqlDataReader selectStandingsReader = selectStandingsComm.ExecuteReader())
                {
                    if (selectStandingsReader.HasRows)
                    {
                        lblQueryHeader.Text = "Best Attack League Table";
                        //int indexOfPosition = selectStandingsReader.GetOrdinal("position");
                        int indexOfTeamName = selectStandingsReader.GetOrdinal("teamName");
                        int indexOfgamesPlayed = selectStandingsReader.GetOrdinal("gamesPlayed");
                        int indexOfWins = selectStandingsReader.GetOrdinal("win");
                        int indexOfDraws = selectStandingsReader.GetOrdinal("draw");
                        int indexOfLosses = selectStandingsReader.GetOrdinal("loss");
                        int indexOfGF = selectStandingsReader.GetOrdinal("GF");
                        int indexOfGA = selectStandingsReader.GetOrdinal("GA");
                        int indexOfGD = selectStandingsReader.GetOrdinal("GD");
                        int indexOfPoints = selectStandingsReader.GetOrdinal("points");

                        while (selectStandingsReader.Read())
                        {
                            int position, gamesPlayed, wins, draws, losses, GF, GA, GD, points;

                            position = 0;
                            string teamName = selectStandingsReader.GetValue(indexOfTeamName).ToString();
                            gamesPlayed = Convert.ToInt32(selectStandingsReader.GetValue(indexOfgamesPlayed));
                            wins = Convert.ToInt32(selectStandingsReader.GetValue(indexOfWins));
                            draws = Convert.ToInt32(selectStandingsReader.GetValue(indexOfDraws));
                            losses = Convert.ToInt32(selectStandingsReader.GetValue(indexOfLosses));
                            GF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfGF));
                            GA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfGA));
                            GD = Convert.ToInt32(selectStandingsReader.GetValue(indexOfGD));
                            points = Convert.ToInt32(selectStandingsReader.GetValue(indexOfPoints));
                            leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }
                    }
                    else
                    {
                        lblQueryHeader.Text = "No best attack league table available";
                    }
                }//end reader
            }//end using
        }//end using

        leagueTableList.Sort(delegate (LeagueStandingObject x, LeagueStandingObject y)
        {
            return y.GF.CompareTo(x.GF);
        });

        int newPosition = 0;
        for (int i = 0; i < leagueTableList.Count; i++)
        {
            newPosition++;
            leagueTableList[i].position = newPosition;
        }

        gvQueryResultsTable.DataSource = leagueTableList;
        gvQueryResultsTable.DataBind();
    }


    protected void btnBestDefence_Click(object sender, EventArgs e)
    {
        string leagueName = ddlLeagues.Text;
        getBestDefenceLeagueTable(leagueName);
    }

    public void getBestDefenceLeagueTable(string leagueName)
    {

        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "SELECT * FROM tblLeagueTable WHERE leagueCaption = '" + leagueName + "' ORDER BY leagueID ASC";
        List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();

        using (SqlConnection selectStandingsConn = new SqlConnection(selectStandingsConnString))
        {
            using (SqlCommand selectStandingsComm = new SqlCommand(selectStandingsCommString, selectStandingsConn))
            {
                selectStandingsConn.Open();
                using (SqlDataReader selectStandingsReader = selectStandingsComm.ExecuteReader())
                {
                    if (selectStandingsReader.HasRows)
                    {
                        lblQueryHeader.Text = "Best Defence League Table";
                        //int indexOfPosition = selectStandingsReader.GetOrdinal("position");
                        int indexOfTeamName = selectStandingsReader.GetOrdinal("teamName");
                        int indexOfgamesPlayed = selectStandingsReader.GetOrdinal("gamesPlayed");
                        int indexOfWins = selectStandingsReader.GetOrdinal("win");
                        int indexOfDraws = selectStandingsReader.GetOrdinal("draw");
                        int indexOfLosses = selectStandingsReader.GetOrdinal("loss");
                        int indexOfGF = selectStandingsReader.GetOrdinal("GF");
                        int indexOfGA = selectStandingsReader.GetOrdinal("GA");
                        int indexOfGD = selectStandingsReader.GetOrdinal("GD");
                        int indexOfPoints = selectStandingsReader.GetOrdinal("points");

                        while (selectStandingsReader.Read())
                        {
                            int position, gamesPlayed, wins, draws, losses, GF, GA, GD, points;

                            position = 0;
                            string teamName = selectStandingsReader.GetValue(indexOfTeamName).ToString();
                            gamesPlayed = Convert.ToInt32(selectStandingsReader.GetValue(indexOfgamesPlayed));
                            wins = Convert.ToInt32(selectStandingsReader.GetValue(indexOfWins));
                            draws = Convert.ToInt32(selectStandingsReader.GetValue(indexOfDraws));
                            losses = Convert.ToInt32(selectStandingsReader.GetValue(indexOfLosses));
                            GF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfGF));
                            GA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfGA));
                            GD = Convert.ToInt32(selectStandingsReader.GetValue(indexOfGD));
                            points = Convert.ToInt32(selectStandingsReader.GetValue(indexOfPoints));
                            leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }
                    }
                    else
                    {
                        lblQueryHeader.Text = "No best defence league table available";
                    }
                }//end reader
            }//end using
        }//end using

        leagueTableList.Sort(delegate (LeagueStandingObject x, LeagueStandingObject y)
        {
            return x.GA.CompareTo(y.GA);
        });

        int newPosition = 0;
        for (int i = 0; i < leagueTableList.Count; i++)
        {
            newPosition++;
            leagueTableList[i].position = newPosition;
        }

        gvQueryResultsTable.DataSource = leagueTableList;
        gvQueryResultsTable.DataBind();
    }


    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["userSesh"] = null;
        Response.Redirect("Default.aspx");
    }
}