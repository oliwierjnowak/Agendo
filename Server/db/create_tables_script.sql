--drop table  [dbo].[csmd_authorizations_domain_entity];
--drop table [dbo].[csmd_authorizations];
--drop table [dbo].[csti_do_shift];
--drop table [dbo].[csmd_domain];
--drop table [dbo].[csti_daily_schedule];


CREATE TABLE [dbo].[csmd_domain](

       [do_no] [bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL, --Fortlaufende Nummer

       [do_name] [nvarchar](1024) NOT NULL, --Angezeigter Name des MA)
	   [do_password] [nvarchar](1024) --password des MA)
	   )
GO
  
CREATE TABLE [dbo].[csti_do_shift](

       [dosh_no] [bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL, --Fortlaufende Nummer

       [dosh_do_no] [bigint] NOT NULL, --Eindeutige Nr. eines MA

       [dosh_week_number] [smallint] NOT NULL, --ISO Woche

       [dosh_year] [smallint] NOT NULL, --ISO Jahr

       [dosh_monday] [bigint] NOT NULL, --Schicht/Tagesprogramm Montag

       [dosh_tuesday] [bigint] NOT NULL, --Schicht/Tagesprogramm Dienstag

       [dosh_wednesday] [bigint] NOT NULL, --Schicht/Tagesprogramm Mittwoch

       [dosh_thursday] [bigint] NOT NULL, --Schicht/Tagesprogramm Donnerstag

       [dosh_friday] [bigint] NOT NULL, --Schicht/Tagesprogramm Freitag

       [dosh_saturday] [bigint] NOT NULL, --Schicht/Tagesprogramm Samstag

       [dosh_sunday] [bigint] NOT NULL, --Schicht/Tagesprogramm Sonntag

       [dosh_ws_no] [int] NULL

)
GO
CREATE TABLE [dbo].[csti_daily_schedule](

       [ds_no] [bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL, --Fortlaufende Nummer

       [ds_name] [nvarchar](1024) NOT NULL, --Angezeigter Schichtname

       [ds_hours] [smallint] NOT NULL --Stunden dieser Schicht, für Auswertung

)
GO
CREATE TABLE [dbo].[csmd_authorizations_domain_entity](
       [audoen_no] [int] NOT NULL, --Fortlaufende Nummer aus csmd_authorizations
       [audoen_do_no] [bigint] NOT NULL, --Eindeutige Nummer vom Vorgesetzten
       [audoen_en_no] [bigint] NOT NULL, --Eindeutige Nummer vom Mitarbeiter
)
GO

CREATE TABLE [dbo].[csmd_authorizations](
       [au_no] [int] IDENTITY(1,1) NOT NULL, --Fortlaufende Nummer
       [au_ri_no] [int] NOT NULL, --Nummer eines Rechts, Bsp. 719 für Schichtverwaltung
       [au_enabled] [bit] NOT NULL, --Ist das Recht aktiviert, oder deaktiviert
       [au_from] [date] NOT NULL, --Ab Wann ist dieser Eintrag gültig (bei uns meistens 01.01.1900)
       [au_to] [date] NOT NULL    --Bis wann ist dieser Eintrag gültig (bei uns meistens 31.12.3000)
)
GO



ALTER TABLE [dbo].[csti_do_shift]
ADD CONSTRAINT fk_dosh_do_no
FOREIGN KEY ([dosh_do_no])
REFERENCES [dbo].[csmd_domain]([do_no]);


ALTER TABLE [dbo].[csti_do_shift]
ADD CONSTRAINT fk_dosh_monday
FOREIGN KEY ([dosh_monday])
REFERENCES [dbo].[csti_daily_schedule]([ds_no]);


ALTER TABLE [dbo].[csti_do_shift]
ADD CONSTRAINT fk_dosh_tuesday
FOREIGN KEY ([dosh_tuesday])
REFERENCES[dbo].[csti_daily_schedule]([ds_no]);

ALTER TABLE [dbo].[csti_do_shift]
ADD CONSTRAINT fk_dosh_wednesday
FOREIGN KEY ([dosh_wednesday])
REFERENCES[dbo].[csti_daily_schedule]([ds_no]);

ALTER TABLE [dbo].[csti_do_shift]
ADD CONSTRAINT fk_dosh_thursday
FOREIGN KEY ([dosh_thursday])
REFERENCES[dbo].[csti_daily_schedule]([ds_no]);

ALTER TABLE [dbo].[csti_do_shift]
ADD CONSTRAINT fk_dosh_friday
FOREIGN KEY ([dosh_friday])
REFERENCES[dbo].[csti_daily_schedule]([ds_no]);

ALTER TABLE [dbo].[csti_do_shift]
ADD CONSTRAINT fk_dosh_saturday
FOREIGN KEY ([dosh_saturday])
REFERENCES[dbo].[csti_daily_schedule]([ds_no]);

ALTER TABLE [dbo].[csti_do_shift]
ADD CONSTRAINT fk_dosh_sunday
FOREIGN KEY ([dosh_sunday])
REFERENCES[dbo].[csti_daily_schedule]([ds_no]);

ALTER TABLE [dbo].[csmd_authorizations_domain_entity]
ADD CONSTRAINT PK_authorizations PRIMARY KEY ([audoen_do_no],[audoen_en_no]);

ALTER TABLE [dbo].[csmd_authorizations_domain_entity]
ADD CONSTRAINT FK_audoen_do_no_csmd_domain FOREIGN KEY ([audoen_do_no])
REFERENCES [dbo].[csmd_domain]([do_no]);
 
ALTER TABLE [dbo].[csmd_authorizations_domain_entity]
ADD CONSTRAINT FK_audoen_en_no_csmd_domain FOREIGN KEY ([audoen_en_no])
REFERENCES [dbo].[csmd_domain]([do_no]);