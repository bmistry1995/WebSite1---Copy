using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userSesh"] != null)
            lbWelcome.Text += Session["userSesh"].ToString();
        else
            Response.Redirect("Default.aspx");
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["userSesh"] = null;
        Response.Redirect("Default.aspx");
    }

    protected void btnAddLeagues_Click(object sender, EventArgs e)
    {
        string response;
        //get response from the leagues data link
        response = getJSONResponse("https://www.football-data.org/v1/competitions");
        //passing the response to be parsed
        parseLeagueJSON(response);
    }

    //method takes and endpoint and returns a JSON response
    private string getJSONResponse(string endPoint)
    {
        //creeate new instance of restClient
        RestClient restClient = new RestClient();
        //restClientClass restClient = new restClientClass();
        //set restClients endPoint as the passed enpoint
        restClient.endPoint = endPoint;
        //store response in a string and return
        string responseString = string.Empty;
        responseString = restClient.makeRequest();
        return responseString;
    }

    //method to dynamically parse JSON
    private void parseLeagueJSON(string strJSON)
    {
        //variables to hold the json array elements
        int id, currentMatchday, noOfMatchdays, noOfTeams, noOfGames;
        string leagueName, lastUpdated, teamsLink, fixturesLink, leagueTableLink;

        //boolean to represent if the ID already exists
        bool idExists;

        var leagues = JsonConvert.DeserializeObject<dynamic>(strJSON);

        //command to be used on the table
        String insertLeaguesCommand = @"INSERT INTO tblLeagues 
                            ([id], [leagueName], [currentMatchday], [noOfMatchdays], [noOfTeams], [noOfGames], [lastUpdated], [teamsLink], [fixturesLink], [leagueTableLink])
                            VALUES (@id, @leagueName, @currentMatchday, @noOfMatchdays, @noOfTeams, @noOfGames, @lastUpdated, @teamsLink, @fixturesLink, @leagueTableLink)";

        foreach (var array in leagues)
        {
            //set idExists to false initially
            idExists = false;
            //set these first so we can check if the ID exists in the table, if so report and skip to next element in the  reponse array
            id = array.id;
            leagueName = array.caption;

            idExists = checkId(id);
            if (idExists == true)
            {
                lbLeagueProgress.Items.Add("Already exists: " + id + " - " + leagueName);
                //MessageBox.Show("Already exists: " + id + " - " + leagueName);
                continue;
            }
            else
            {
                //get each element from the array and assign to variables
                currentMatchday = array.currentMatchday;
                noOfMatchdays = array.numberOfMatchdays;
                noOfTeams = array.numberOfTeams;
                noOfGames = array.numberOfGames;
                lastUpdated = Convert.ToString(array.lastUpdated);
                teamsLink = Convert.ToString(array._links.teams.href);
                fixturesLink = Convert.ToString(array._links.fixtures.href);
                leagueTableLink = Convert.ToString(array._links.leagueTable.href);
            }

            String insertLeaguesConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
            using (SqlConnection sqlConn = new SqlConnection(insertLeaguesConnString))
            {
                try
                {
                    using (SqlCommand sqlComm = new SqlCommand(insertLeaguesCommand, sqlConn))
                    {
                        sqlConn.Open();
                        sqlComm.Parameters.AddWithValue("@id", id);
                        sqlComm.Parameters.AddWithValue("@leagueName", leagueName);
                        sqlComm.Parameters.AddWithValue("@currentMatchday", currentMatchday);
                        sqlComm.Parameters.AddWithValue("@noOfMatchdays", noOfMatchdays);
                        sqlComm.Parameters.AddWithValue("@noOfTeams", noOfTeams);
                        sqlComm.Parameters.AddWithValue("noOfGames", noOfGames);
                        sqlComm.Parameters.AddWithValue("@lastUpdated", Convert.ToDateTime(lastUpdated));
                        sqlComm.Parameters.AddWithValue("@teamsLink", teamsLink);
                        sqlComm.Parameters.AddWithValue("@fixturesLink", fixturesLink);
                        sqlComm.Parameters.AddWithValue("@leagueTableLink", leagueTableLink);
                        sqlComm.ExecuteNonQuery();
                        sqlConn.Close();
                        lbLeagueProgress.Items.Add("Done: " + id + " - " + leagueName);
                    }
                }//end try
                catch
                {
                    lbLeagueProgress.Items.Add("Failed: " + id + " - " + leagueName);
                    //MessageBox.Show("Failed for " + id + " - " + leagueName);
                }//end catch
            }//end using
        }//end foreach
        lblLeagueProgress.Text = "Done Leagues";
    }//end parseLeagueJSON

    public bool checkId(int ID)
    {
        String checkIdCommand = "SELECT COUNT(*) FROM tblLeagues WHERE id like @id";
        String checkIdConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        using (SqlConnection conn = new SqlConnection(checkIdConnString))
        {
            using (SqlCommand comm = new SqlCommand(checkIdCommand, conn))
            {
                conn.Open();
                comm.Parameters.AddWithValue("@id", ID);
                int idCount = (int)comm.ExecuteScalar();
                conn.Close();
                if (idCount > 0)
                {
                    //means id exists
                    return true;
                }
                else
                {
                    //id doesnt exist
                    return false;
                }

            }//end using command
        }//end using connection
    }

    protected void btnAddTeams_Click(object sender, EventArgs e)
    {
        String selectIdTeamLinkConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        String selectIdTeamLinkComm = "SELECT id, teamsLink, fixturesLink, leagueTableLink FROM tblLeagues";
        using (SqlConnection conn = new SqlConnection(selectIdTeamLinkConnString))
        {
            using (SqlCommand comm = new SqlCommand(selectIdTeamLinkComm, conn))
            {
                conn.Open();
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    //get the index of the current id and teamsLink
                    int indexOfId = reader.GetOrdinal("id");
                    int indexOfTeamsLink = reader.GetOrdinal("teamsLink");
                    int indexOfFixturesLink = reader.GetOrdinal("fixturesLink");
                    int indexOfLeagueTableLink = reader.GetOrdinal("leagueTableLink");
                    while (reader.Read())
                    {
                        //get the values in the current ID and teamsLink index
                        var id = reader.GetValue(indexOfId);
                        var teamsLink = reader.GetValue(indexOfTeamsLink);
                        var fixturesLink = reader.GetValue(indexOfFixturesLink);
                        var leagueTableLink = reader.GetValue(indexOfLeagueTableLink);

                        int strId = Convert.ToInt32(id);
                        string strTeamsLink = teamsLink.ToString();
                        string strFixturesLink = fixturesLink.ToString();
                        string strLeagueTableLink = leagueTableLink.ToString();

                        /*string teamJSON = getJSONResponse(strTeamsLink);
                        if (teamJSON == null)
                            continue;
                        else
                            parseTeamsJSON(teamJSON, strId);

                        string fixturesJSON = getJSONResponse(strFixturesLink);
                        if (fixturesJSON == null)
                            continue;
                        else
                            parseFixturesJSON(fixturesJSON, strId);

                        string standingsJSON = getJSONResponse(strLeagueTableLink);
                        if (standingsJSON == null)
                            continue;
                        else
                            parseLeagueTableJSON(standingsJSON, strId);
                        //add delay
                        System.Threading.Thread.Sleep(1000);*/

                        if (strId == 445 || strId ==450 || strId ==452  || strId == 456 || strId ==446 )
                        {
                            System.Threading.Thread.Sleep(1500);
                            //get JSON of the leagueLink
                            /*string teamJSON = getJSONResponse(strTeamsLink);
                            //pass the JSON and league ID to be parsed 
                            parseTeamsJSON(teamJSON, strId);
                            
                            //get SJON of the fixtures link
                            string fixturesJSON = getJSONResponse(strFixturesLink);
                            //pass fixtures JSON, ID to be parsed and stored in DB
                            parseFixturesJSON(fixturesJSON, strId);*/

                            //get JSON of the league table link
                            string leagueTableJSON = getJSONResponse(strLeagueTableLink);
                            //pass leaguetable JSON and the league ID to be parsed and stored in DB
                            parseLeagueTableJSON(leagueTableJSON, strId); 
                                                
                        }
                        else
                        {
                            continue;
                        }

                    }//end while
                }//end using data reader
            }//end using command
        }//end using connection
    }

    private void parseTeamsJSON(string json, int leagueID)
    {
        bool teamExists;
        var teamsJSON = JsonConvert.DeserializeObject<dynamic>(json);

        string insertTeamsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string insertTeamsCommString = @"INSERT INTO tblTeams ([teamName], [leagueID], [shortName], [crestURL], [playersLink])
                                    VALUES (@teamName, @leagueID, @shortName, @crest, @playersLink)";
        var teams = teamsJSON.teams;
        foreach (var team in teams)
        {
            teamExists = false;
            string shortName, crest, playersLink;
            string teamName = team.name;
            teamExists = checkTeam(leagueID, teamName);
            if (teamExists == true)
            {
                lbTeamStatus.Items.Add("Already Exists: " + teamName);
                continue;
            }
            else
            {
                shortName = team.shortName;
                crest = "";
                playersLink = Convert.ToString(team._links.players.href);
            }
            using (SqlConnection insertTeamsConn = new SqlConnection(insertTeamsConnString))
            {
                try
                {
                    using (SqlCommand insertTeamsComm = new SqlCommand(insertTeamsCommString, insertTeamsConn))
                    {
                        insertTeamsConn.Open();
                        insertTeamsComm.Parameters.AddWithValue("@teamName", teamName);
                        insertTeamsComm.Parameters.AddWithValue("@leagueID", leagueID);
                        insertTeamsComm.Parameters.AddWithValue("@shortName", shortName);
                        insertTeamsComm.Parameters.AddWithValue("@crest", crest);
                        insertTeamsComm.Parameters.AddWithValue("@playersLink", playersLink);
                        insertTeamsComm.ExecuteNonQuery();
                        insertTeamsConn.Close();
                        lbTeamStatus.Items.Add("Done: " + teamName);
                    }
                }
                catch
                {
                    lbTeamStatus.Items.Add("Failed: " + teamName);
                }//end catch
            }//end sql conn
        }//end foreach
        lblTeamProgress.Text = "Done Teams";
    }

    public bool checkTeam(int ID, string teamName)
    {
        String checkTeamCommString = "SELECT COUNT(*) FROM tblTeams WHERE leagueID like @id AND teamName like @teamName";
        String checkTeamConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        using (SqlConnection checkTeamConn = new SqlConnection(checkTeamConnString))
        {
            using (SqlCommand checkTeamComm = new SqlCommand(checkTeamCommString, checkTeamConn))
            {
                checkTeamConn.Open();
                checkTeamComm.Parameters.AddWithValue("@id", ID);
                checkTeamComm.Parameters.AddWithValue("@teamName", teamName);
                int idCount = (int)checkTeamComm.ExecuteScalar();
                checkTeamConn.Close();
                if (idCount > 0)
                {
                    //means id exists
                    return true;
                }
                else
                {
                    //id doesnt exist
                    return false;
                }

            }//end using command
        }//end using connection
    }

    private void parseFixturesJSON(string json, int leagueID)
    {
        bool fixtureExists;
        var fixturesJSON = JsonConvert.DeserializeObject<dynamic>(json);

        string dateFormat = "yyyy-MM-dd HH:mm:ss";
        string insertFixturesConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string insertFixturesCommString = @"INSERT INTO tblFixtures ([leagueID], [homeTeam], [awayTeam], [fixtureDate], [matchday], [matchStatus], [homeGoalsFT], [awayGoalsFT], [homeGoalsHT], [awayGoalsHT])
                                    VALUES (@leagueID, @homeTeam, @awayTeam, @fixtureDate, @matchday, @matchStatus, @homeGoalsFT, @awayGoalsFT, @homeGoalsHT, @awayGoalsHT)";
        //fixtures array
        var fixtures = fixturesJSON.fixtures;
        int fixtureCount = 0;
        //loop through the array
        foreach (var fixture in fixtures)
        {
            fixtureCount++;
            fixtureExists = false;
            //store all the attributes into variables
            string homeTeam, awayTeam, matchStatus;
            DateTime fixtureDate;
            int matchday, homeGoalsFT, awayGoalsFT, homeGoalsHT, awayGoalsHT;

            //set the homeTeam and awayTeam names
            homeTeam = fixture.homeTeamName;
            awayTeam = fixture.awayTeamName;

            //pass the league id, home + away team, to see if it aready exists 
            fixtureExists = checkFixture(leagueID, homeTeam, awayTeam);
            if (fixtureExists == true)
            {
                //if exists, notify and skip
                lbFixtureStatus.Items.Add("Already Exists: " + homeTeam + " Vs " + awayTeam);
                continue;
            }
            else
            {
                try
                {
                    fixtureDate = fixture.date;
                    matchday = Convert.ToInt32(fixture.matchday);
                    matchStatus = fixture.status;
                    if (matchStatus == "SCHEDULED" || matchStatus == "TIMED")
                    {
                        //if scheduled/timed, these are always going to be zero
                        homeGoalsFT = 0;
                        awayGoalsFT = 0;
                        homeGoalsHT = 0;
                        awayGoalsHT = 0;
                    }//end if
                    else
                    {
                        //if finished, then get the HT/FT home/away goals
                        homeGoalsFT = Convert.ToInt32(fixture.result.goalsHomeTeam);
                        awayGoalsFT = Convert.ToInt32(fixture.result.goalsAwayTeam);
                        homeGoalsHT = Convert.ToInt32(fixture.result.halfTime.goalsHomeTeam);
                        awayGoalsHT = Convert.ToInt32(fixture.result.halfTime.goalsAwayTeam);
                    }//end else

                    //lbFixtureStatus.Items.Add(fixtureCount + " " + homeTeam + " " + homeGoalsFT + " (" + homeGoalsHT + ") Vs (" + awayGoalsHT + ") " + awayGoalsFT + " " + awayTeam);
                    //NEED TO STORE IN DB
                    using (SqlConnection insertFixturesConn = new SqlConnection(insertFixturesConnString))
                    {
                        try
                        {
                            using (SqlCommand insertFixturesComm = new SqlCommand(insertFixturesCommString, insertFixturesConn))
                            {
                                insertFixturesConn.Open();
                                insertFixturesComm.Parameters.AddWithValue("@leagueID", leagueID);
                                insertFixturesComm.Parameters.AddWithValue("@homeTeam", homeTeam);
                                insertFixturesComm.Parameters.AddWithValue("@awayTeam", awayTeam);
                                insertFixturesComm.Parameters.AddWithValue("@fixtureDate", fixtureDate.ToString(dateFormat));
                                insertFixturesComm.Parameters.AddWithValue("@matchday", matchday);
                                insertFixturesComm.Parameters.AddWithValue("@matchStatus", matchStatus);
                                insertFixturesComm.Parameters.AddWithValue("@homeGoalsFT", homeGoalsFT);
                                insertFixturesComm.Parameters.AddWithValue("@awayGoalsFT", awayGoalsFT);
                                insertFixturesComm.Parameters.AddWithValue("@homeGoalsHT", homeGoalsHT);
                                insertFixturesComm.Parameters.AddWithValue("@awayGoalsHT", awayGoalsHT);
                                insertFixturesComm.ExecuteNonQuery();
                                insertFixturesConn.Close();
                                lbFixtureStatus.Items.Add("Done: " + homeTeam + " Vs " + awayTeam);
                            }//end using
                        }//end try
                        catch
                        {
                            lbTeamStatus.Items.Add("Failed: " + homeTeam + " Vs " + awayTeam);
                        }//end catch
                    }//end sql conn*/
                }//end try
                catch
                {
                    lbFixtureStatus.Items.Add("Failed for " + fixtureCount);
                }
            }//end else
        }//end foreach
        lblTeamProgress.Text = "Done Teams";
    }

    public bool checkFixture(int ID, string homeTeam, string awayTeam)
    {
        String checkFixtureCommString = "SELECT COUNT(*) FROM tblFixtures WHERE leagueID like @id AND homeTeam like @homeTeam AND awayTeam like @awayTeam";
        String checkFixtureConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        using (SqlConnection checkFixtureConn = new SqlConnection(checkFixtureConnString))
        {
            using (SqlCommand checkFixtureComm = new SqlCommand(checkFixtureCommString, checkFixtureConn))
            {
                checkFixtureConn.Open();
                checkFixtureComm.Parameters.AddWithValue("@id", ID);
                checkFixtureComm.Parameters.AddWithValue("@homeTeam", homeTeam);
                checkFixtureComm.Parameters.AddWithValue("@awayTeam", awayTeam);
                int idCount = (int)checkFixtureComm.ExecuteScalar();
                checkFixtureConn.Close();
                if (idCount > 0)
                {
                    //means id exists
                    return true;
                }
                else
                {
                    //id doesnt exist
                    return false;
                }

            }//end using command
        }//end using connection
    }


    //parsing the leagueTable JSON
    private void parseLeagueTableJSON(string json, int leagueID)
    {
        bool standingExists;
        var leagueStandings = JsonConvert.DeserializeObject<dynamic>(json);

        string insertStandingsConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        //
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //UPTO HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //
        string insertStandingsCommString = @"INSERT INTO tblLeagueTable ([leagueID], [position], [teamName], [leagueCaption], [currentMatchday], [gamesPlayed], [points], [GF], [GA], [GD], [win], [loss], [draw], [homeGF], [homeGA], [homeWin], [homeLoss], [homeDraw], [awayGF], [awayGA], [awayWin], [awayLoss], [awayDraw])
                                    VALUES (@leagueID, @position, @teamName, @leagueCaption, @currentMatchday, @gamesPlayed, @points, @GF, @GA, @GD, @win, @loss, @draw, @homeGF, @homeGA, @homeWin, @homeLoss, @homeDraw, @awayGF, @awayGA, @awayWin, @awayLoss, @awayDraw)";
        //fixtures array
        var standings = leagueStandings.standing;
        string leagueCaption = leagueStandings.leagueCaption;
        int currentMatchday = Convert.ToInt32(leagueStandings.matchday);
        //loop through the array
        foreach (var standing in standings)
        {
            //store all the attributes into variables
            string teamName;
            int position, gamesPlayed, points, GF, GA, GD, win, loss, draw;
            int homeGF, homeGA, homeWin, homeLoss, homeDraw;
            int awayGF, awayGA, awayWin, awayLoss, awayDraw;

            position = standing.position;
            teamName = standing.teamName;

            //pass the league id, home + away team, to see if it aready exists 
            //standingExists = checkStandings(leagueID, position, teamName);
            standingExists = false;
            if (standingExists == true)
            {
                //if exists, notify and skip
                lbStandingsStatus.Items.Add("Already Exists: " + position + " - " + teamName);
                continue;
            }
            else
            {
                try
                {
                    gamesPlayed = Convert.ToInt32(standing.playedGames);
                    points = Convert.ToInt32(standing.points);
                    GF = Convert.ToInt32(standing.goals);
                    GA = Convert.ToInt32(standing.goalsAgainst);
                    GD = Convert.ToInt32(standing.goalDifference);
                    win = Convert.ToInt32(standing.wins);
                    loss = Convert.ToInt32(standing.losses);
                    draw = Convert.ToInt32(standing.draws);
                    homeGF = Convert.ToInt32(standing.home.goals);
                    homeGA = Convert.ToInt32(standing.home.goalsAgainst);
                    homeWin = Convert.ToInt32(standing.home.wins);
                    homeLoss = Convert.ToInt32(standing.home.losses);
                    homeDraw = Convert.ToInt32(standing.home.draws);
                    awayGF = Convert.ToInt32(standing.away.goals);
                    awayGA = Convert.ToInt32(standing.away.goalsAgainst);
                    awayWin = Convert.ToInt32(standing.away.wins);
                    awayLoss = Convert.ToInt32(standing.away.losses);
                    awayDraw = Convert.ToInt32(standing.away.draws);

                    //lbFixtureStatus.Items.Add(fixtureCount + " " + homeTeam + " " + homeGoalsFT + " (" + homeGoalsHT + ") Vs (" + awayGoalsHT + ") " + awayGoalsFT + " " + awayTeam);
                    //NEED TO STORE IN DB
                    using (SqlConnection insertStandingsConn = new SqlConnection(insertStandingsConnString))
                    {
                        try
                        {
                            using (SqlCommand insertStandingsComm = new SqlCommand(insertStandingsCommString, insertStandingsConn))
                            {
                                insertStandingsConn.Open();
                                insertStandingsComm.Parameters.AddWithValue("@leagueID", leagueID);
                                insertStandingsComm.Parameters.AddWithValue("@position", position);
                                insertStandingsComm.Parameters.AddWithValue("@teamName", teamName);
                                insertStandingsComm.Parameters.AddWithValue("@leagueCaption", leagueCaption);
                                insertStandingsComm.Parameters.AddWithValue("@currentMatchday", currentMatchday);
                                insertStandingsComm.Parameters.AddWithValue("@gamesPlayed", gamesPlayed);
                                insertStandingsComm.Parameters.AddWithValue("@points", points);
                                insertStandingsComm.Parameters.AddWithValue("@GF", GF);
                                insertStandingsComm.Parameters.AddWithValue("@GA", GA);
                                insertStandingsComm.Parameters.AddWithValue("@GD", GD);
                                insertStandingsComm.Parameters.AddWithValue("@win", win);
                                insertStandingsComm.Parameters.AddWithValue("@loss", loss);
                                insertStandingsComm.Parameters.AddWithValue("@draw", draw);
                                insertStandingsComm.Parameters.AddWithValue("@homeGF", homeGF);
                                insertStandingsComm.Parameters.AddWithValue("@homeGA", homeGA);
                                insertStandingsComm.Parameters.AddWithValue("@homeWin", homeWin);
                                insertStandingsComm.Parameters.AddWithValue("@homeLoss", homeLoss);
                                insertStandingsComm.Parameters.AddWithValue("@homeDraw", homeDraw);
                                insertStandingsComm.Parameters.AddWithValue("@awayGF", awayGF);
                                insertStandingsComm.Parameters.AddWithValue("@awayGA", awayGA);
                                insertStandingsComm.Parameters.AddWithValue("@awayWin", awayWin);
                                insertStandingsComm.Parameters.AddWithValue("@awayLoss", awayLoss);
                                insertStandingsComm.Parameters.AddWithValue("@awayDraw", awayDraw);
                                insertStandingsComm.ExecuteNonQuery();
                                insertStandingsConn.Close();
                                lbStandingsStatus.Items.Add("Done: " + position + " - " + teamName);
                            }//end using
                        }//end try
                        catch (Exception e)
                        {
                            lblHelp.Text = e.ToString();
                            lbStandingsStatus.Items.Add("Failed to add: " + position + " - " + teamName);

                        }//end catch
                    }//end sql conn*/
                }//end try
                catch
                {
                    lbStandingsStatus.Items.Add("Failed: " + position + " - " + teamName);
                }
            }//end else
        }//end foreach
        lblStandingsProgress.Text = "Done Standings";
    }

    /*public bool checkStandings(int ID, int position, string teamName)
    {
        String checkFixtureCommString = "SELECT COUNT(*) FROM tblFixtures WHERE leagueID like @id AND homeTeam like @homeTeam AND awayTeam like @awayTeam";
        String checkFixtureConnString = @"Server=DESKTOP-RRF1L4V\SQLEXPRESS;Database=dbFootballStats2018;Trusted_Connection=True";
        using (SqlConnection checkFixtureConn = new SqlConnection(checkFixtureConnString))
        {
            using (SqlCommand checkFixtureComm = new SqlCommand(checkFixtureCommString, checkFixtureConn))
            {
                checkFixtureConn.Open();
                checkFixtureComm.Parameters.AddWithValue("@id", ID);
                checkFixtureComm.Parameters.AddWithValue("@homeTeam", homeTeam);
                checkFixtureComm.Parameters.AddWithValue("@awayTeam", awayTeam);
                int idCount = (int)checkFixtureComm.ExecuteScalar();
                checkFixtureConn.Close();
                if (idCount > 0)
                {
                    //means id exists
                    return true;
                }
                else
                {
                    //id doesnt exist
                    return false;
                }

            }//end using command
        }//end using connection
    }*/

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        deleteFromTable("tblTeams");
        deleteFromTable("tblFixtures");
        deleteFromTable("tblLeagueTable");
    }

    public void deleteFromTable(string tableName)
    {
        string deleteFromConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string deleteFromCommString = "delete from " + tableName;
        using (SqlConnection deleteFromConn = new SqlConnection(deleteFromConnString))
        {
            using (SqlCommand deleteFromComm = new SqlCommand(deleteFromCommString, deleteFromConn))
            {
                deleteFromConn.Open();
                deleteFromComm.ExecuteNonQuery();
                deleteFromConn.Close();
                lbDeleteFrom.Items.Add("Deleted from " + tableName);

            }//end using
        }//end uusing

    }
}