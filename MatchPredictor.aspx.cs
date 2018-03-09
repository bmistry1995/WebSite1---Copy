using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MatchPredictor : System.Web.UI.Page
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
            lblMatches.Text = "No leagues available";
        else
        {
            foreach (string leagueName in leaguesList)
                ddlLeagues.Items.Add(new ListItem(leagueName));
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

    protected void btnSelectLeague_Click(object sender, EventArgs e)
    {
        string leagueName = ddlLeagues.Text;
        getUpcomingFixtures(leagueName);
    }

    public void getUpcomingFixtures(string leagueName)
    {
        string selectGamesConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectGamesCommString = "select * from tblFixtures join tblLeagues on tblFixtures.leagueID = tblLeagues.id where tblLeagues.leagueName = '" + leagueName + "' and tblFixtures.matchStatus = 'TIMED' order by fixtureDate";
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
                        lblMatches.Text = "Upcoming matches in " + leagueName;
                        int indexOfFixtureDate = selectGamesReader.GetOrdinal("fixtureDate");
                        int indexOfHomeTeam = selectGamesReader.GetOrdinal("homeTeam");
                        int indexOfHomeGoals = selectGamesReader.GetOrdinal("homeGoalsFT");
                        int indexOfAwayGoals = selectGamesReader.GetOrdinal("awayGoalsFT");
                        int indexOfAwayTeam = selectGamesReader.GetOrdinal("awayTeam");

                        while (selectGamesReader.Read())
                        {
                            string fixtureDate, homeTeam, awayTeam;
                            int homeGoalsFT, awayGoalsFT;

                            fixtureDate = selectGamesReader.GetValue(indexOfFixtureDate).ToString();
                            homeTeam = selectGamesReader.GetValue(indexOfHomeTeam).ToString();
                            homeGoalsFT = Convert.ToInt32(selectGamesReader.GetValue(indexOfHomeGoals).ToString());
                            awayGoalsFT = Convert.ToInt32(selectGamesReader.GetValue(indexOfAwayGoals).ToString());
                            awayTeam = selectGamesReader.GetValue(indexOfAwayTeam).ToString();


                            fixturesList.Add(new FixtureObject(fixtureDate, homeTeam, homeGoalsFT, awayGoalsFT, awayTeam));
                        }
                    }
                    else
                    {
                        lblMatches.Text = "No upcoming fixtures to show";
                    }
                }//end reader
            }//end using
        }//end using

        for (int i = 0; i < fixturesList.Count; i++)
        {
            double homeTeamAvgHomeGoalsScored, awayTeamAvgAwaysGoalsConceded, homeTeamAvgHomeGoalsConceded, awayTeamAvgAwayGoalsScored;
            homeTeamAvgHomeGoalsScored = getHomeTeamAvgHomeGoalsScored(fixturesList[i].homeTeam);
            awayTeamAvgAwaysGoalsConceded = getAwayTeamAvgAwayGoalsConceded(fixturesList[i].awayTeam);

            homeTeamAvgHomeGoalsConceded = getHomeTeamAvgHomeGoalsConceded(fixturesList[i].homeTeam);
            awayTeamAvgAwayGoalsScored = getAwayTeamAvgAwayGoalsScored(fixturesList[i].homeTeam);

            fixturesList[i].homeGoalsFT = (int)(homeTeamAvgHomeGoalsScored * awayTeamAvgAwaysGoalsConceded);
            fixturesList[i].awayGoalsFT = (int)(homeTeamAvgHomeGoalsConceded * awayTeamAvgAwayGoalsScored);

        }
        gvUpcomingFixtures.DataSource = fixturesList;
        gvUpcomingFixtures.DataBind();
    }

    public double getHomeTeamAvgHomeGoalsScored(string passedTeamName)
    {
        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "select * from tblLeagueTable where teamName like '%" + passedTeamName + "%'";
        //List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();
        double result = 0;
        using (SqlConnection selectStandingsConn = new SqlConnection(selectStandingsConnString))
        {
            using (SqlCommand selectStandingsComm = new SqlCommand(selectStandingsCommString, selectStandingsConn))
            {
                selectStandingsConn.Open();
                using (SqlDataReader selectStandingsReader = selectStandingsComm.ExecuteReader())
                {
                    if (selectStandingsReader.HasRows)
                    {
                        int indexOfHomeGF = selectStandingsReader.GetOrdinal("homeGF");
                        int indexOfHomeGA = selectStandingsReader.GetOrdinal("homeGA");
                        int indexOfHomeWin = selectStandingsReader.GetOrdinal("homeWin");
                        int indexOfHomeDraw = selectStandingsReader.GetOrdinal("homeDraw");
                        int indexOfHomeLoss = selectStandingsReader.GetOrdinal("homeLoss");
                                              
                        while (selectStandingsReader.Read())
                        {
                            int homeGF, homeGA, homeW, homeD, homeL, awayGF, awayGA, awayW, awayD, awayL;

                            homeGF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeGF));
                            homeGA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeGA));
                            homeW = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeWin));
                            homeD = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeDraw));
                            homeL = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeLoss));

                            result = Math.Round((double)homeGF / (homeW + homeD + homeL), 1);
                            //lblAvgHomeConceded.Text = "Average home goals conceded per game: " + Math.Round((double)homeGA / (homeW + homeD + homeL), 1);

                            //lblAvgAwayScored.Text = "Average away goals scored per game: " + Math.Round((double)awayGF / (awayW + awayD + awayL), 1);
                            //lblAvgAwayCondeded.Text = "Average away goals conceded per game: " + Math.Round((double)awayGA / (awayW + awayD + awayL), 1);
                            //leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                }//end reader
            }//end using
        }//end using
        return result;
    }

    public double getHomeTeamAvgHomeGoalsConceded(string passedTeamName)
    {
        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "select * from tblLeagueTable where teamName like '%" + passedTeamName + "%'";
        //List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();
        double result = 0;
        using (SqlConnection selectStandingsConn = new SqlConnection(selectStandingsConnString))
        {
            using (SqlCommand selectStandingsComm = new SqlCommand(selectStandingsCommString, selectStandingsConn))
            {
                selectStandingsConn.Open();
                using (SqlDataReader selectStandingsReader = selectStandingsComm.ExecuteReader())
                {
                    if (selectStandingsReader.HasRows)
                    {
                        int indexOfHomeGF = selectStandingsReader.GetOrdinal("homeGF");
                        int indexOfHomeGA = selectStandingsReader.GetOrdinal("homeGA");
                        int indexOfHomeWin = selectStandingsReader.GetOrdinal("homeWin");
                        int indexOfHomeDraw = selectStandingsReader.GetOrdinal("homeDraw");
                        int indexOfHomeLoss = selectStandingsReader.GetOrdinal("homeLoss");

                        while (selectStandingsReader.Read())
                        {
                            int homeGF, homeGA, homeW, homeD, homeL, awayGF, awayGA, awayW, awayD, awayL;

                            homeGF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeGF));
                            homeGA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeGA));
                            homeW = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeWin));
                            homeD = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeDraw));
                            homeL = Convert.ToInt32(selectStandingsReader.GetValue(indexOfHomeLoss));

                            result = Math.Round((double)homeGA / (homeW + homeD + homeL), 1);
                            //lblAvgHomeConceded.Text = "Average home goals conceded per game: " + Math.Round((double)homeGA / (homeW + homeD + homeL), 1);

                            //lblAvgAwayScored.Text = "Average away goals scored per game: " + Math.Round((double)awayGF / (awayW + awayD + awayL), 1);
                            //lblAvgAwayCondeded.Text = "Average away goals conceded per game: " + Math.Round((double)awayGA / (awayW + awayD + awayL), 1);
                            //leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                }//end reader
            }//end using
        }//end using
        return result;
    }

    public double getAwayTeamAvgAwayGoalsScored(string passedTeamName)
    {
        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "select * from tblLeagueTable where teamName like '%" + passedTeamName + "%'";
        //List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();
        double result = 0;
        using (SqlConnection selectStandingsConn = new SqlConnection(selectStandingsConnString))
        {
            using (SqlCommand selectStandingsComm = new SqlCommand(selectStandingsCommString, selectStandingsConn))
            {
                selectStandingsConn.Open();
                using (SqlDataReader selectStandingsReader = selectStandingsComm.ExecuteReader())
                {
                    if (selectStandingsReader.HasRows)
                    {
                        int indexOfAwayGF = selectStandingsReader.GetOrdinal("awayGF");
                        int indexOfAwayGA = selectStandingsReader.GetOrdinal("awayGA");
                        int indexOfAwayWin = selectStandingsReader.GetOrdinal("awayWin");
                        int indexOfAwayDraw = selectStandingsReader.GetOrdinal("awayDraw");
                        int indexOfAwayLoss = selectStandingsReader.GetOrdinal("awayLoss");

                        while (selectStandingsReader.Read())
                        {
                            int homeGF, homeGA, homeW, homeD, homeL, awayGF, awayGA, awayW, awayD, awayL;

                            awayGF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayGF));
                            awayGA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayGA));
                            awayW = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayWin));
                            awayD = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayDraw));
                            awayL = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayLoss));

                            result = Math.Round((double)awayGF / (awayW + awayD + awayL), 1);
                            //lblAvgHomeConceded.Text = "Average home goals conceded per game: " + Math.Round((double)homeGA / (homeW + homeD + homeL), 1);

                            //lblAvgAwayScored.Text = "Average away goals scored per game: " + Math.Round((double)awayGF / (awayW + awayD + awayL), 1);
                            //lblAvgAwayCondeded.Text = "Average away goals conceded per game: " + Math.Round((double)awayGA / (awayW + awayD + awayL), 1);
                            //leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                }//end reader
            }//end using
        }//end using
        return result;
    }

    public double getAwayTeamAvgAwayGoalsConceded(string passedTeamName)
    {
        string selectStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectStandingsCommString = "select * from tblLeagueTable where teamName like '%" + passedTeamName + "%'";
        //List<LeagueStandingObject> leagueTableList = new List<LeagueStandingObject>();
        double result = 0;
        using (SqlConnection selectStandingsConn = new SqlConnection(selectStandingsConnString))
        {
            using (SqlCommand selectStandingsComm = new SqlCommand(selectStandingsCommString, selectStandingsConn))
            {
                selectStandingsConn.Open();
                using (SqlDataReader selectStandingsReader = selectStandingsComm.ExecuteReader())
                {
                    if (selectStandingsReader.HasRows)
                    {
                        int indexOfAwayGF = selectStandingsReader.GetOrdinal("awayGF");
                        int indexOfAwayGA = selectStandingsReader.GetOrdinal("awayGA");
                        int indexOfAwayWin = selectStandingsReader.GetOrdinal("awayWin");
                        int indexOfAwayDraw = selectStandingsReader.GetOrdinal("awayDraw");
                        int indexOfAwayLoss = selectStandingsReader.GetOrdinal("awayLoss");

                        while (selectStandingsReader.Read())
                        {
                            int homeGF, homeGA, homeW, homeD, homeL, awayGF, awayGA, awayW, awayD, awayL;

                            awayGF = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayGF));
                            awayGA = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayGA));
                            awayW = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayWin));
                            awayD = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayDraw));
                            awayL = Convert.ToInt32(selectStandingsReader.GetValue(indexOfAwayLoss));

                            result = Math.Round((double)awayGA / (awayW + awayD + awayL), 1);
                            //lblAvgHomeConceded.Text = "Average home goals conceded per game: " + Math.Round((double)homeGA / (homeW + homeD + homeL), 1);

                            //lblAvgAwayScored.Text = "Average away goals scored per game: " + Math.Round((double)awayGF / (awayW + awayD + awayL), 1);
                            //lblAvgAwayCondeded.Text = "Average away goals conceded per game: " + Math.Round((double)awayGA / (awayW + awayD + awayL), 1);
                            //leagueTableList.Add(new LeagueStandingObject(position, teamName, gamesPlayed, wins, draws, losses, GF, GA, GD, points));
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                }//end reader
            }//end using
        }//end using
        return result;
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["userSesh"] = null;
        Response.Redirect("Default.aspx");
    }
}