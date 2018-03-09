create table tblFixtures(
	leagueID int not null,
	homeTeam nvarchar(1024),
	awayTeam nvarchar(1024),
	fixtureDate date,
	matchday int,
	matchStatus nvarchar(1024),
	homeGoalsFT int,
	awayGoalsFT int,
	homeGoalsHT int,
	awayGoalsHT int
);