--script to create tblLeagues (holds all the league info)

create table tblLeagues(
	id int not null,
	leagueName nvarchar(1024),
	currentMatchday int,
	noOfMatchdays int,
	noOfTeams int,
	noOfGames int,
	lastUpdated date,
	teamsLink nvarchar(max),
	fixturesLink nvarchar(max),
	leagueTableLink nvarchar(max)
);