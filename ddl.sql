-- Drop table

-- DROP TABLE mpimedianet_management.dbo.division

CREATE TABLE mpimedianet_management.dbo.division (
	name varchar(100) NOT NULL,
	description varchar(1000) NULL,
	institution_id int NOT NULL,
	created_date datetime NULL,
	id int IDENTITY(1,1) NOT NULL,
	CONSTRAINT division_PK PRIMARY KEY (id),
	CONSTRAINT division_institution_FK FOREIGN KEY (institution_id) REFERENCES mpimedianet_management.dbo.institution(id)
) GO;

-- Drop table

-- DROP TABLE mpimedianet_management.dbo.event

CREATE TABLE mpimedianet_management.dbo.event (
	name varchar(100) NOT NULL,
	location varchar(100) NOT NULL,
	info varchar(100) NOT NULL,
	done int NOT NULL,
	participant int NOT NULL,
	program_id int NOT NULL,
	user_id int NOT NULL,
	created_date datetime NULL,
	[date] datetime NULL,
	id int IDENTITY(1,1) NOT NULL,
	CONSTRAINT event_PK PRIMARY KEY (id),
	CONSTRAINT event_program_FK FOREIGN KEY (program_id) REFERENCES mpimedianet_management.dbo.program(id)
) GO;

-- Drop table

-- DROP TABLE mpimedianet_management.dbo.institution

CREATE TABLE mpimedianet_management.dbo.institution (
	name varchar(100) NOT NULL,
	description varchar(1000) NULL,
	created_date datetime NULL,
	id int IDENTITY(1,1) NOT NULL,
	CONSTRAINT institution_PK PRIMARY KEY (id)
) GO;

-- Drop table

-- DROP TABLE mpimedianet_management.dbo.[member]

CREATE TABLE mpimedianet_management.dbo.[member] (
	name varchar(100) NULL,
	description varchar(1000) NULL,
	position_id int NULL,
	created_date datetime NULL,
	id int IDENTITY(1,1) NOT NULL,
	section_id int NULL,
	CONSTRAINT member_PK PRIMARY KEY (id),
	CONSTRAINT member_position_FK FOREIGN KEY (position_id) REFERENCES mpimedianet_management.dbo.[position](id),
	CONSTRAINT member_section_FK FOREIGN KEY (section_id) REFERENCES mpimedianet_management.dbo.[section](id)
) GO;

-- Drop table

-- DROP TABLE mpimedianet_management.dbo.[position]

CREATE TABLE mpimedianet_management.dbo.[position] (
	name varchar(100) NOT NULL,
	parent_position_id int NULL,
	created_date datetime NULL,
	id int IDENTITY(1,1) NOT NULL,
	description varchar(500) NULL,
	CONSTRAINT position_PK PRIMARY KEY (id)
) GO;

-- Drop table

-- DROP TABLE mpimedianet_management.dbo.post

CREATE TABLE mpimedianet_management.dbo.post (
	name varchar(100) NULL,
	body varchar(1000) NULL,
	post_id int NULL,
	user_id int NOT NULL,
	[type] int NULL,
	[date] datetime NULL,
	created_date datetime NULL,
	id int IDENTITY(1,1) NOT NULL,
	title varchar(100) NULL,
	CONSTRAINT post_PK PRIMARY KEY (id),
	CONSTRAINT post_user_FK FOREIGN KEY (user_id) REFERENCES mpimedianet_management.dbo.[user](id)
) GO;

-- Drop table

-- DROP TABLE mpimedianet_management.dbo.program

CREATE TABLE mpimedianet_management.dbo.program (
	name varchar(100) NOT NULL,
	description varchar(1000) NULL,
	sect_id int NOT NULL,
	created_date datetime NULL,
	id int IDENTITY(1,1) NOT NULL,
	CONSTRAINT program_PK PRIMARY KEY (id),
	CONSTRAINT program_section_FK FOREIGN KEY (sect_id) REFERENCES mpimedianet_management.dbo.[section](id)
) GO;

-- Drop table

-- DROP TABLE mpimedianet_management.dbo.[section]

CREATE TABLE mpimedianet_management.dbo.[section] (
	name varchar(100) NOT NULL,
	division_id int NOT NULL,
	parent_section_id int NULL,
	created_date datetime NULL,
	id int IDENTITY(1,1) NOT NULL,
	CONSTRAINT section_PK PRIMARY KEY (id),
	CONSTRAINT section_division_FK FOREIGN KEY (division_id) REFERENCES mpimedianet_management.dbo.division(id)
) GO;

-- Drop table

-- DROP TABLE mpimedianet_management.dbo.[user]

CREATE TABLE mpimedianet_management.dbo.[user] (
	username varchar(100) NULL,
	name varchar(100) NULL,
	password varchar(100) NULL,
	email varchar(100) NULL,
	admin int NULL,
	institution_id int NULL,
	created_date datetime NULL,
	id int IDENTITY(1,1) NOT NULL,
	CONSTRAINT user_PK PRIMARY KEY (id),
	CONSTRAINT user_institution_FK FOREIGN KEY (institution_id) REFERENCES mpimedianet_management.dbo.institution(id)
) GO;
