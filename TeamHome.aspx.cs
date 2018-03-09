using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TeamHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userSesh"] == null)
            Response.Redirect("Default.aspx");

        string teamName;
        int leagueID = Convert.ToInt32(Request.QueryString["leagueID"]);
        try
        {
            teamName = Request.QueryString["teamName"].ToString();
        }
        catch
        {
            teamName = "not available";
        }

        lblteamName.Text = teamName;

        getResults(teamName);
        getUpcomingResults(teamName);
        getTeamStats(teamName);
    }

    public void getResults(string teamName)
    {
        string selectGamesConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectGamesCommString = "select * from tblFixtures where (homeTeam like '%" + teamName + "%' or awayTeam like '%" + teamName + "%')  and matchStatus = 'FINISHED' order by fixtureDate ";
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
                        lblResults.Text = "No results to show";
                    }
                }//end reader
            }//end using
        }//end using

        gvResultsTable.DataSource = fixturesList;
        gvResultsTable.DataBind();
    }

    public void getUpcomingResults(string teamName)
    {
        string selectGamesConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectGamesCommString = "select * from tblFixtures where (homeTeam like '%" + teamName + "%' or awayTeam like '%" + teamName + "%')  and matchStatus != 'FINISHED' order by fixtureDate ";
        List<UpcomingFixtureObject> fixturesList = new List<UpcomingFixtureObject>();

        using (SqlConnection selectGamesConn = new SqlConnection(selectGamesConnString))
        {
            using (SqlCommand selectGamesComm = new SqlCommand(selectGamesCommString, selectGamesConn))
            {
                selectGamesConn.Open();
                using (SqlDataReader selectGamesReader = selectGamesComm.ExecuteReader())
                {
                    if (selectGamesReader.HasRows)
                    {

                        int indexOfFixtureDate = selectGamesReader.GetOrdinal("fixtureDate");
                        int indexOfHomeTeam = selectGamesReader.GetOrdinal("homeTeam");
                        int indexOfAwayTeam = selectGamesReader.GetOrdinal("awayTeam");

                        while (selectGamesReader.Read())
                        {
                            string fixtureDate, homeTeam, awayTeam;
                            int homeGoalsFT, awayGoalsFT;

                            fixtureDate = selectGamesReader.GetValue(indexOfFixtureDate).ToString();
                            homeTeam = selectGamesReader.GetValue(indexOfHomeTeam).ToString();
                            awayTeam = selectGamesReader.GetValue(indexOfAwayTeam).ToString();


                            fixturesList.Add(new UpcomingFixtureObject(fixtureDate, homeTeam, awayTeam));
                        }
                    }
                    else
                    {
                        lblUpcomingFixtures.Text = "No upcoming fixtures to show";
                    }
                }//end reader
            }//end using
        }//end using

        gvUpcomingFixtures.DataSource = fixturesList;
        gvUpcomingFixtures.DataBind();
    }

    public void getTeamStats(string passedTeamName)
    {
        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "select * from tblLeagueTable where teamName like '%" + passedTeamName + "%'";
        //List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();

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

                        int indexOfHomeGF = selectStandingsReader.GetOrdinal("homeGF");
                        int indexOfHomeGA = selectStandingsReader.GetOrdinal("homeGA");
                        int indexOfHomeWin = selectStandingsReader.GetOrdinal("homeWin");
                        int indexOfHomeDraw = selectStandingsReader.GetOrdinal("homeDraw");
                        int indexOfHomeLoss = selectStandingsReader.GetOrdinal("homeLoss");

                        int indexOfAwayGF = selectStandingsReader.GetOrdinal("awayGF");
                        int indexOfAwayGA = selectStandingsReader.GetOrdinal("awayGA");
                        int indexOfAwayWin = selectStandingsReader.GetOrdinal("awayWin");
                        int indexOfAwayDraw = selectStandingsReader.GetOrdinal("awayDraw");
                        int indexOfAwayLoss = selectStandingsReader.GetOrdinal("awayLoss");

                        while (selectStandingsReader.Read())
                        {
                            int position, gamesPlayed, wins, draws, losses, GF, GA, GD, points;
                            int homeGF, homeGA, homeW, homeD, homeL, awayGF, awayGA, awayW, awayD, awayL;

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

                            homeGF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeGF));
                            homeGA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeGA));
                            homeW = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeWin));
                            homeD = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeDraw));
                            homeL = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeLoss));

                            awayGF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayGF));
                            awayGA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayGA));
                            awayW = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayWin));
                            awayD = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayDraw));
                            awayL = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayLoss));

                            //float avgScored = GF / gamesPlayed;

                            lblTotalGoals.Text = "Goals scored: " + GF;
                            lblTotalConceded.Text = "Goals conceded: " + GA;
                            lblAvgScored.Text = "Average goals scored per game: " + Math.Round((double) GF/gamesPlayed, 1);
                            lblAvgConceded.Text = "Average goals conceded per game: " + Math.Round((double)GA / gamesPlayed, 1);

                            lblAvgHomeScored.Text = "Average home goals scored per game: " + Math.Round((double)homeGF/ (homeW + homeD + homeL), 1);
                            lblAvgHomeConceded.Text = "Average home goals conceded per game: " + Math.Round((double)homeGA / (homeW + homeD + homeL), 1);

                            lblAvgAwayScored.Text = "Average away goals scored per game: " + Math.Round((double)awayGF / (awayW + awayD + awayL), 1);
                            lblAvgAwayCondeded.Text = "Average away goals conceded per game: " + Math.Round((double)awayGA / (awayW + awayD + awayL), 1);
                            //leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }
                    }
                    else
                    {
                        lblStatsHeader.Text = "No stats available";
                    }
                }//end reader
            }//end using
        }//end using
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["userSesh"] = null;
        Response.Redirect("Default.aspx");
    }
}