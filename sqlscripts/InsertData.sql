use Dipl;
go

insert into USER_TYPE
	values('Guest'),
			('Regular'),
			('Admin')
go

insert into USERS(USERNAME)
	values('Dipl.Reserved.Guest');
go

insert into ACCOUNTS(USERNAME,USER_TYPE)
	values('Dipl.Reserved.Guest',0);
go

insert into PLAYLISTS (ACCOUNTID,PLAYLISTNAME)
	values	(1,'testPlaylist'),
			(1,'testPlaylist1'),
			(1,'testPlaylist2'),
			(1,'testPlaylist3')
go

insert into AUDIO
	values	('C:/Users/Димас/Desktop/Новая папка/1.mp3',1,1,'testPlaylist'),
			('C:/Users/Димас/Desktop/Новая папка/1.mp3',0,1,'testPlaylist'),
			('C:/Users/Димас/Desktop/Новая папка/1.mp3',1,1,'testPlaylist'),
			('C:/Users/Димас/Desktop/Новая папка/1.mp3',0,1,'testPlaylist1'),
			('C:/Users/Димас/Desktop/Новая папка/1.mp3',1,1,'testPlaylist1'),
			('C:/Users/Димас/Desktop/Новая папка/1.mp3',0,1,'testPlaylist1'),
			('C:/Users/Димас/Desktop/Новая папка/1.mp3',1,1,'testPlaylist2'),
			('C:/Users/Димас/Desktop/Новая папка/1.mp3',1,1,'testPlaylist2'),
			('C:/Users/Димас/Desktop/Новая папка/1.mp3',0,1,'testPlaylist2')
go

select * from USERS
select * from ACCOUNTS
select * from PLAYLISTS
select * from AUDIO

