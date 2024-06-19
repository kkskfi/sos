SELECT * FROM Doctor
SELECT idPatient, Surname, Name, Patronymic FROM Patient
SELECT idPatient, Surname, Name, Patronymic FROM Patient WHERE idGender = 2
SELECT idDoctor, Surname, Name, Patronymic FROM Doctor WHERE idSpecialization IN (6, 12)
SELECT Title FROM HealingProcedures WHERE idHealingProcedures > 4
SELECT idDoctor FROM Visit GROUP BY idDoctor
UPDATE Doctor SET idSpecialization= 5 WHERE idDoctor= 12
INSERT INTO Doctor (Surname, Name,  Patronymic, idSpecialization) VALUES (С¬асильченькоТ, СƒмитрийТ, С¬ладимировичТ, 8)
DELETE FROM Doctor WHERE idDoctor=25
SELECT * FROM Patient WHERE Surname LIKE СA%Т
SELECT * FROM Medicament WHERE LENGTH(Title)>7
SELECT * FROM Specialization WHERE idSpecialization>=4 AND idSpecialization<=10
SELECT COUNT(*) FROM Classification
SELECT * FROM Doctor WHERE idSpecialization NOT IN (1, 3, 5)
SELECT * FROM Doctor WHERE idSpecialization > 11 ORDER BY idSpecialization DESC
