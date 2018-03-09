using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LeagueHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userSesh"] == null)
            Response.Redirect("Default.aspx");

        int leagueID = Convert.ToInt32(Request.QueryString["leagueID"]);
        getLeagueInfo(leagueID);
        getLeagueTeams(leagueID);
        getLeagueTable(leagueID);
    }

    public void getLeagueInfo(int ID)
    {
        string selectLeaguesConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectLeaguesCommString = "SELECT * FROM tblLeagues WHERE id = " + ID;

        using (SqlConnection selectLeaguesConn = new SqlConnection(selectLeaguesConnString))
        {
            using (SqlCommand selectLeaguesComm = new SqlCommand(selectLeaguesCommString, selectLeaguesConn))
            {
                selectLeaguesConn.Open();
                using (SqlDataReader selectLeaguesReader = selectLeaguesComm.ExecuteReader())
                {
                    int indexOfLeagueName = selectLeaguesReader.GetOrdinal("leagueName");
                    int indexOfCurrentMatchday = selectLeaguesReader.GetOrdinal("currentMatchday");
                    int indexOfTotalMatchdays = selectLeaguesReader.GetOrdinal("noOfMatchdays");
                    int indexOfTotalTeams = selectLeaguesReader.GetOrdinal("noOfTeams");
                    int indexOftotalGames = selectLeaguesReader.GetOrdinal("noOfGames");

                    while (selectLeaguesReader.Read())
                    {
                        string leagueName = selectLeaguesReader.GetValue(indexOfLeagueName).ToString();
                        int currentMatchday = Convert.ToInt32(selectLeaguesReader.GetValue(indexOfCurrentMatchday));
                        int totalMatchdays = Convert.ToInt32(selectLeaguesReader.GetValue(indexOfTotalMatchdays));
                        int totalTeams = Convert.ToInt32(selectLeaguesReader.GetValue(indexOfTotalTeams));
                        int totalGames = Convert.ToInt32(selectLeaguesReader.GetValue(indexOftotalGames));

                        lblLeagueName.Text = leagueName;
                        lblCurrentMatchday.Text = "Current Matchday: " + currentMatchday.ToString();
                        lblTotalMatchdays.Text = "Total Matchdays: " + totalMatchdays.ToString();
                        lblTotalTeams.Text = "Total Teams: " + totalTeams.ToString();
                        lblTotalGames.Text = "Total Games: " + totalGames.ToString();

                    }//end while
                }//end reader
            }//end using
        }//end using
    }

    public void getLeagueTeams(int ID)
    {
        string selectTeamsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectTeamsCommString = "SELECT * FROM tblTeams WHERE leagueID = " + ID + " ORDER BY teamName ASC";
        List<TeamObject> teams = new List<TeamObject>();

        using (SqlConnection selectTeamsConn = new SqlConnection(selectTeamsConnString))
        {
            using (SqlCommand selectTeamsComm = new SqlCommand(selectTeamsCommString, selectTeamsConn))
            {
                selectTeamsConn.Open();
                using (SqlDataReader selectTeamsReader = selectTeamsComm.ExecuteReader())
                {
                    if(selectTeamsReader.HasRows)
                    {
                        int indexOfLeagueID = selectTeamsReader.GetOrdinal("leagueID");
                        int indexOfTeamName = selectTeamsReader.GetOrdinal("teamName");
                        while (selectTeamsReader.Read())
                        {
                            teams.Add(new TeamObject(Convert.ToInt32(selectTeamsReader.GetValue(indexOfLeagueID)), selectTeamsReader.GetValue(indexOfTeamName).ToString()));
                        }
                    }
                    else
                    {
                        lblTeamsHeader.Text = "No teams available";
                    }
                }//end reader
            }//end using
        }//end using

        printTeams.DataSource = teams;
        printTeams.DataBind();
    }

    public void getLeagueTable(int ID)
    {
        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "SELECT * FROM tblLeagueTable WHERE leagueID = " + ID + " ORDER BY leagueID ASC";
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
                        lblLeagueTableHeader.Text = "No league table available";
                    }
                }//end reader
            }//end using
        }//end using

        gvLeagueTable.DataSource = leagueTableList;
        gvLeagueTable.DataBind();
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["userSesh"] = null;
        Response.Redirect("Default.aspx");
    }
}