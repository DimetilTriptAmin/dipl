--create database Dipl;
--go

use Dipl;
go

create table [USER_TYPE]
(
	TYPEID int identity(0,1) primary key(TYPEID),
	TYPENAME nchar(10),
);

create table [User]
(
	USERNAME nchar(20) not null
		constraint PK_USERS primary key (USERNAME),
	PASSWORDHASH nvarchar(max),
);

create table [Account]
(
	ACCOUNTID int identity(1,1) 
		constraint PK_ACCOUNTS primary key (ACCOUNTID),
	USER_TYPE int not null
		constraint FK_ACOUNTS_USERTYPE foreign key (USER_TYPE)
			references USER_TYPE(TYPEID),
	USERNAME nchar(20) not null
		constraint FK_ACOUNTS_USERS foreign key (USERNAME)
			references [User](USERNAME),
	IMAGEURl nvarchar(max)
);

create table [Playlist]
(
	ACCOUNTID int not null
		constraint FK_PLAYLISTS_ACCOUNTS foreign key (ACCOUNTID)
			references [Account](ACCOUNTID),
	IMAGEURl nvarchar(max),
	PLAYLISTNAME nvarchar(20), 
	constraint PK_PLAYLISTS primary key (PLAYLISTNAME, ACCOUNTID)
);

create table [Audio]
(
	AIDIOID int primary key identity(0,1),
	AUDIOURl nvarchar(max) not null,
	ISLIKED bit,
	USERID int not null,
	PLAYLISTNAME nvarchar(20) not null,
	constraint FK_AUDIO_PLAYLISTS foreign key (PLAYLISTNAME,USERID)
			references [Playlist](PLAYLISTNAME, ACCOUNTID)
);


--drop table AUDIO;
--drop table PLAYLIST;
--drop table ACCOUNT;
--drop table USER_TYPE;
--drop table [USER];
--drop database Dipl;