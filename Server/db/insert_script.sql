
-- domain
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Emmett','$2a$11$2GhIw4g1hqztupAeq1u1L.tU1tcNwSWGIe2m/Kz0HRRAvgI4Q5pLC'); --password
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Kettel','$2a$11$jXTOhzTvZo6CphJnV73xgO2g5ELaM2cvzKmOojEc4Y1B0H670IGda');
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Bachs','$2a$11$UXNGuDpUyALU4VlVJXVSvetBRimFExbEM4HbC7QeFHHRo3wfig9o6');
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Gartell','$2a$11$qQemBgIwFIBr81c0RqtBn.z8ohB9CJ888Q5JwxOYDObciuKnxtYbe');
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Tante','$2a$11$g0xU3zjXcJ4XxGll0c3UtOvMSYt9mZbPfDQvPO.YMIw0R37k.i5xO');
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Gemnett','$2a$11$g0xU3zjXcJ4XxGll0c3UtOvMSYt9mZbPfDQvPO.YMIw0R37k.i5xO');
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Chad','$2a$11$qQemBgIwFIBr81c0RqtBn.z8ohB9CJ888Q5JwxOYDObciuKnxtYbe');
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Soaper','$2a$11$qQemBgIwFIBr81c0RqtBn.z8ohB9CJ888Q5JwxOYDObciuKnxtYbe');
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Rack','$2a$11$qQemBgIwFIBr81c0RqtBn.z8ohB9CJ888Q5JwxOYDObciuKnxtYbe');
insert into [dbo].[csmd_domain] ([do_name],[do_password]) values ('Cheel','$2a$11$qQemBgIwFIBr81c0RqtBn.z8ohB9CJ888Q5JwxOYDObciuKnxtYbe');

-- daily_schedule
INSERT INTO [dbo].[csti_daily_schedule] ([ds_name], [ds_hours]) VALUES     ('empty', 0),     ('standard shift', 8),     ('part time shift', 4), ('babymonat', 3), ('senior', 6);

-- do_shift (currently every employee has only one week of a scedule 1-52 -> needs to be changed in the future)
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (2, 1, 2023, 3, 5, 5, 2, 5, 1, 5, 32);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (7, 2, 2023, 5, 1, 4, 4, 1, 4, 2, 69);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (9, 3, 2023, 3, 1, 4, 1, 1, 5, 4, 64);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (7, 4, 2022, 5, 4, 5, 1, 5, 5, 1, 20);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (10, 5, 2022, 2, 5, 3, 3, 3, 5, 1, 63);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (1, 6, 2023, 2, 4, 1, 1, 3, 2, 1, 18);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (6, 7, 2023, 4, 2, 2, 4, 3, 3, 2, 72);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (5, 8, 2022, 5, 1, 5, 1, 3, 5, 2, 18);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (7, 9, 2022, 2, 5, 4, 3, 3, 1, 3, 6);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (10, 10, 2022, 1, 3, 1, 1, 1, 5, 4, 90);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (9, 11, 2023, 1, 4, 2, 1, 4, 4, 5, 34);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (10, 12, 2022, 5, 4, 4, 4, 3, 3, 1, 55);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (3, 13, 2022, 2, 3, 4, 1, 1, 3, 1, 22);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (6, 14, 2023, 4, 5, 2, 2, 4, 2, 5, 46);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (5, 15, 2023, 1, 5, 3, 4, 2, 3, 3, 60);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (8, 16, 2023, 5, 5, 5, 1, 3, 2, 5, 3);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (9, 17, 2023, 2, 2, 1, 1, 1, 5, 2, 9);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (5, 18, 2023, 4, 3, 3, 4, 5, 3, 3, 46);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (8, 19, 2023, 4, 5, 2, 1, 4, 2, 5, 24);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (3, 20, 2023, 5, 1, 2, 4, 3, 5, 4, 51);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (3, 21, 2023, 5, 3, 3, 2, 1, 4, 2, 88);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (1, 22, 2022, 3, 3, 4, 4, 1, 1, 3, 52);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (1, 23, 2022, 2, 5, 5, 3, 2, 3, 2, 59);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (9, 24, 2022, 3, 5, 5, 2, 5, 2, 4, 20);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (1, 25, 2022, 1, 2, 4, 1, 5, 2, 3, 89);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (7, 26, 2023, 1, 3, 2, 3, 1, 4, 1, 52);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (3, 27, 2022, 4, 4, 2, 5, 5, 5, 5, 88);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (2, 28, 2022, 3, 2, 3, 5, 1, 4, 1, 58);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (6, 29, 2022, 3, 4, 4, 5, 5, 5, 3, 29);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (1, 30, 2023, 3, 2, 4, 1, 1, 4, 5, 95);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (9, 31, 2023, 1, 2, 1, 5, 1, 5, 3, 9);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (1, 32, 2022, 2, 2, 5, 3, 3, 3, 2, 46);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (10, 33, 2022, 3, 3, 3, 3, 3, 2, 5, 58);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (3, 34, 2022, 5, 2, 3, 2, 2, 5, 4, 89);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (9, 35, 2022, 1, 2, 1, 5, 1, 2, 1, 31);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (2, 36, 2023, 3, 5, 1, 4, 4, 3, 2, 94);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (4, 37, 2022, 5, 1, 5, 2, 3, 1, 3, 25);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (4, 38, 2022, 3, 1, 5, 4, 2, 2, 3, 84);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (10, 39, 2023, 2, 2, 3, 4, 5, 3, 1, 59);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (6, 40, 2022, 1, 2, 2, 5, 5, 5, 3, 68);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (2, 41, 2023, 4, 2, 3, 1, 5, 3, 2, 8);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (5, 42, 2023, 1, 3, 3, 2, 1, 5, 5, 11);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (10, 43, 2022, 1, 1, 3, 5, 3, 4, 1, 98);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (4, 44, 2022, 3, 2, 1, 3, 5, 5, 1, 13);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (2, 45, 2023, 5, 3, 3, 5, 3, 2, 3, 98);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (7, 46, 2022, 4, 2, 4, 1, 1, 5, 4, 21);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (10, 47, 2023, 2, 3, 1, 5, 5, 4, 3, 25);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (6, 48, 2022, 3, 1, 2, 1, 3, 3, 5, 46);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (2, 49, 2023, 5, 5, 2, 3, 4, 2, 4, 16);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (10, 50, 2023, 3, 1, 3, 5, 2, 3, 5, 55);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (7, 51, 2022, 4, 1, 4, 2, 5, 4, 2, 76);
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday], [dosh_ws_no]) values (3, 52, 2023, 3, 2, 2, 2, 3, 3, 5, 89);


insert into [dbo].[csmd_authorizations] ([au_ri_no], [au_enabled],[au_from],[au_to]) values (719,1,'1900-01-01','3000-12-31'); -- schichtverwalten 
insert into [dbo].[csmd_authorizations] ([au_ri_no], [au_enabled],[au_from],[au_to]) values (1000,1,'1900-01-01','3000-12-31'); -- schicht lesen

insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (719,1,1);
insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (1000,1,2);
insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (1000,1,3);
insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (1000,1,4);
insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (1000,1,5);
insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (719,6,6);
insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (1000,6,7);
insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (1000,6,8);
insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (1000,6,9);
insert into [dbo].[csmd_authorizations_domain_entity] ([audoen_no],[audoen_do_no], [audoen_en_no]) values (1000,6,10);